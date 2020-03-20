using Antlr4.Runtime.Misc;
using SLangCompiler.FrontEnd.Tables;
using SLangCompiler.FrontEnd.Types;
using SLangGrammar;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SLangCompiler.BackEnd.Translator
{
    public partial class CppTranslator : SLangGrammarBaseVisitor<object>, IDisposable
    {
        private readonly IndentedTextWriter headerText;
        private readonly IndentedTextWriter cppText;
        private readonly SourceCodeTable source;

        private Scope scope;
        private RoutineNameTableItem currentRoutine;
        private SlangCustomType currentType;
        private bool inProgramBlock = false;
        private readonly string moduleName;
        private readonly DirectoryInfo directoryGen;

        private readonly ModuleNameTable currentModule;

        public CppTranslator(TextWriter headerWriter, TextWriter cppWriter, SourceCodeTable src, ModuleNameTable curModule, DirectoryInfo directoryGen)
        {
            headerText = new IndentedTextWriter(headerWriter);
            cppText = new IndentedTextWriter(cppWriter);
            source = src;
            currentModule = curModule;
            moduleName = currentModule.ModuleData.Name;
            this.directoryGen = directoryGen;

            scope = new Scope();
        }

        public void Dispose()
        {
            headerText.InnerWriter?.Dispose();
            cppText.InnerWriter?.Dispose();
            headerText?.Dispose();
            cppText?.Dispose();
        }

        public override object VisitStart([NotNull] SLangGrammarParser.StartContext context)
        {
            headerText.WriteLine($"#ifndef {moduleName}_H");
            headerText.WriteLine($"#define {moduleName}_H");

            // header includes
            headerText.WriteLine("#include <iostream>");
            headerText.WriteLine("#include <vector>");
            headerText.WriteLine("#include <memory>");
            headerText.WriteLine("#include <string>");

            var importedFiles = new List<string>();
            IEnumerable<IImportable> importableItems = currentModule.Routines.Select(i => i.Value);
            importableItems = importableItems.Concat(currentModule.Classes.Select(i => i.Value));

            foreach (var item in importableItems)
            {
                if (item.Header != null)
                {
                    if (!importedFiles.Contains(item.Header.File))
                    {
                        importedFiles.Add(item.Header.File);
                    }
                }
            }
            foreach (var file in importedFiles)
            {
                headerText.WriteLine($"#include {file}");
                var replacedStr = file.Replace("\"", "");
                var sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, replacedStr);
                var destPath = Path.Combine(directoryGen.FullName, replacedStr);
                Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                File.Copy(sourcePath, destPath, true);
            }
            

            // cpp includes
            cppText.WriteLine($"#include \"{moduleName}.h\"");

            base.VisitStart(context);

            headerText.WriteLine();
            headerText.WriteLine($"#endif");

            // end write
            headerText.Flush();
            cppText.Flush();

            return null;
        }

        public override object VisitModuleImport([NotNull] SLangGrammarParser.ModuleImportContext context)
        {
            headerText.WriteLine($"#include \"{context.Id().GetText()}.h\"");
            return null;
        }

        public override object VisitModuleDeclareSeq([NotNull] SLangGrammarParser.ModuleDeclareSeqContext context)
        {
            WriteLineAll($"namespace {moduleName}");
            WriteLineAll("{");
            headerText.Indent++;
            cppText.Indent++;
            base.VisitModuleDeclareSeq(context);
            cppText.Indent--;
            headerText.Indent--;
            WriteLineAll("}");
            return null;
        }
        // declares 
        public override object VisitTypeDecl([NotNull] SLangGrammarParser.TypeDeclContext context)
        {
            TranslateCustomType(context.Id().GetText());
            return null;
        }

        public override object VisitFunctionDecl([NotNull] SLangGrammarParser.FunctionDeclContext context)
        {
            // init states, check of abstract methods or imported functions
            // add entries in h & cpp
            // visit sequence

            InitRoutines(context.thisHeader(), context.Id().GetText());
            TranslateRoutines(context.statementSeq());
            currentRoutine = null;
            currentType = null;
            return null;
        }

        public override object VisitProcedureDecl([NotNull] SLangGrammarParser.ProcedureDeclContext context)
        {
            InitRoutines(context.thisHeader(), context.Id().GetText());
            TranslateRoutines(context.statementSeq());
            currentRoutine = null;
            currentType = null;
            return null;
        }

        private void InitRoutines(SLangGrammarParser.ThisHeaderContext thisHeader, string routineName)
        {
            if (thisHeader != null)
            {
                currentType = GetCustomTypeContext(thisHeader.customType());
            }
            if (currentType != null)
            {
                currentRoutine = source.FindClass(currentType).Methods[routineName];
            }
            else
            {
                currentRoutine = currentModule.Routines[routineName];
            }
        }

        private void TranslateRoutines(SLangGrammarParser.StatementSeqContext ctx)
        {
            if (!(currentRoutine is MethodNameTableItem))
            {
                // write in header
                WriteRoutineHeader(headerText, currentRoutine);
                headerText.WriteLine(';');
            }
            // write to cpp
            cppText.Write($"{GetStringFromType(currentRoutine.ReturnType)} ");
            if (currentRoutine is MethodNameTableItem)
            {
                cppText.Write($"{currentType.Name}::");
            }
            cppText.Write(currentRoutine.Name);
            WriteParameters(cppText, currentRoutine.Params);
            Visit(ctx);
        }

        public override object VisitStatementSeq([NotNull] SLangGrammarParser.StatementSeqContext context)
        {
            var newScope = new Scope(scope);
            scope = newScope;

            cppText.WriteLine();
            cppText.WriteLine('{');
            cppText.Indent++;
            foreach (var stmt in context.statement())
            {
                Visit(stmt);
            }
            cppText.Indent--;
            cppText.WriteLine('}');

            scope = scope?.Outer;
            return null;
        }

        public override object VisitModuleStatementsSeq([NotNull] SLangGrammarParser.ModuleStatementsSeqContext context)
        {
            inProgramBlock = true;
            cppText.WriteLine($"using namespace {CompilerConstants.MainModuleName};");
            cppText.Write("int main()");
            Visit(context.statementSeq());
            return null;
        }

        public override object VisitModuleFieldDecl([NotNull] SLangGrammarParser.ModuleFieldDeclContext context)
        {
            headerText.Write("extern ");
            base.VisitModuleFieldDecl(context);
            WriteLineAll(";");
            return null;
        }

        public override object VisitDeclare([NotNull] SLangGrammarParser.DeclareContext context)
        {
            return base.VisitDeclare(context);
        }

        // declares
        public override object VisitSimpleDecl([NotNull] SLangGrammarParser.SimpleDeclContext context)
        {
            SlangType type;
            if (context.simpleType() != null)
            {
                type = Visit(context.simpleType()) as SlangType;
            }
            else
            {
                type = Visit(context.customType()) as SlangType;
            }

            var nameToken = context.Id().Symbol;
            var res = new VariableNameTableItem { Type = type, IsConstant = false, Column = nameToken.Column, Line = nameToken.Line, Name = nameToken.Text };
            PutVariableIfInBlock(res);
            TranslateDeclare(res, context.exp());
            return null;
        }

        public override object VisitArrayDecl([NotNull] SLangGrammarParser.ArrayDeclContext context)
        {
            var type = Visit(context.arrayDeclType()) as SlangType;
            var nameToken = context.Id().Symbol;
            var res = new VariableNameTableItem { Type = type, IsConstant = false, Column = nameToken.Column, Line = nameToken.Line, Name = nameToken.Text };

            if (!inProgramBlock && currentRoutine == null)
            {
                TranslateDeclareHead(res, headerText);
            }
            TranslateDeclareHead(res, cppText);

            // write constructor
            var exps = context.arrayDeclType().exp();
            int dimensionCount = exps.Length;
            int n = dimensionCount - 1;
            foreach (var exp in context.arrayDeclType().exp())
            {
                cppText.Write("(");
                Visit(exp);

                if (n > 0)
                {
                    cppText.Write(", ");
                    cppText.Write(GetVectorTypeStart(dimensionCount));
                    var arrType = Visit(context.arrayDeclType().scalarType()) as SlangType;
                    cppText.Write(GetStringFromType(arrType));
                    cppText.Write(GetVectorTypeEnd(dimensionCount));
                }

                n--;
            }

            for (int i = 0; i < exps.Length; ++i)
            {
                cppText.Write(")");
            }

            PutVariableIfInBlock(res);
            return null;
        }

        private void PutVariableIfInBlock(VariableNameTableItem res)
        {
            if (inProgramBlock || currentRoutine != null)
            {
                scope.PutVariable(res);
            }
        }

        public override object VisitPtrDecl([NotNull] SLangGrammarParser.PtrDeclContext context)
        {
            var type = Visit(context.ptrType()) as SlangType;
            var nameToken = context.Id().Symbol;
            var res = new VariableNameTableItem { Type = type, IsConstant = false, Column = nameToken.Column, Line = nameToken.Line, Name = nameToken.Text };
            PutVariableIfInBlock(res);
            TranslateDeclare(res, context.exp());
            return null;
        }

        public override object VisitConstDecl([NotNull] SLangGrammarParser.ConstDeclContext context)
        {
            var type = Visit(context.scalarType()) as SlangType;
            var nameToken = context.Id().Symbol;
            var item = new VariableNameTableItem { Type = type, IsConstant = false, Column = nameToken.Column, Line = nameToken.Line, Name = nameToken.Text };
            PutVariableIfInBlock(item);
            TranslateDeclare(item, context.exp());
            return null;
        }

        private void TranslateDeclare(VariableNameTableItem item, SLangGrammarParser.ExpContext expContext)
        {
            if (!inProgramBlock && currentRoutine == null)
            {
                TranslateDeclareHead(item, headerText);
            }
            TranslateDeclareHead(item, cppText);
            if (expContext != null)
            {
                cppText.Write(" = ");
                Visit(expContext);
            }
        }

        private void TranslateDeclareHead(VariableNameTableItem item, IndentedTextWriter writer)
        {
            // module var
            if (item.IsConstant)
            {
                writer.Write($"const ");
            }
            writer.Write($"{GetStringFromType(item.Type)} {item.Name}");
        }

        public void TranslateCustomType(string typeName)
        {
            var typeData = currentModule.Classes[typeName];
            headerText.Write($"class {typeName}");
            if (!typeData.TypeIdent.Equals(SlangCustomType.Object))
            {
                headerText.Write($": public {GetStringFromType(typeData.Base)}");
            }
            headerText.WriteLine();
            headerText.WriteLine("{");
            headerText.Indent++;

            headerText.WriteLine($"public:");
            headerText.Indent++;
            TranslateClassData(typeData, AccessModifier.Public);
            headerText.Indent--;

            headerText.WriteLine($"private:");
            headerText.Indent++;
            TranslateClassData(typeData, AccessModifier.Private);
            headerText.Indent--;

            headerText.Indent--;
            headerText.WriteLine("};");
        }

        private void TranslateClassData(ClassNameTableItem typeData, AccessModifier modifier)
        {
            foreach (var item in typeData.GetItems(modifier))
            {
                if (item is MethodNameTableItem method)
                {
                    TranslateMethodDecl(method);
                }
                else if (item is VariableNameTableItem field)
                {
                    TranslateClassFieldDecl(field);
                }
            }
        }

        private void TranslateClassFieldDecl(VariableNameTableItem field)
        {
            headerText.Write($"{GetStringFromType(field.Type)} {field.Name};");
        }

        private void TranslateMethodDecl(MethodNameTableItem method)
        {
            headerText.Write("virtual ");
            WriteRoutineHeader(headerText, method);
            if (method.IsAbstract)
            {
                headerText.Write(" = 0");
            }
            headerText.WriteLine(";");
        }

        private void WriteRoutineHeader(IndentedTextWriter writer, RoutineNameTableItem item)
        {
            writer.Write(GetStringFromType(item.ReturnType));
            writer.Write($" {item.Name}");
            WriteParameters(writer, item.Params);
        }

        private void WriteParameters(IndentedTextWriter writer, IList<RoutineArgNameTableItem> args)
        {
            writer.Write('(');
            foreach (var arg in args)
            {
                writer.Write(GetStringFromType(arg.TypeArg.Type));
                if (arg.TypeArg.Modifier == ParamModifier.Ref)
                {
                    writer.Write('&');
                }
                writer.Write($" {arg.Name}");
                if (arg != args.Last())
                {
                    writer.Write(", ");
                }
            }
            writer.Write(')');
        }
    }
}
