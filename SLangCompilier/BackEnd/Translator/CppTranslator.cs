using Antlr4.Runtime.Misc;
using SLangCompiler.FrontEnd.Tables;
using SLangGrammar;
using System;
using System.CodeDom.Compiler;
using SLangCompiler.FrontEnd.Types;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace SLangCompiler.BackEnd.Translator
{
    public class CppTranslator: SLangGrammarBaseVisitor<object>
    {
        private readonly IndentedTextWriter headerText;
        private readonly IndentedTextWriter cppText;
        private readonly SourceCodeTable source;

        private Scope scope;
        private RoutineNameTableItem currentRoutine;
        private bool inProgramBlock = false;
        private string moduleName;

        private ModuleNameTable currentModule;
        private bool canWriteHeader = true;

        public CppTranslator(TextWriter headerWriter, TextWriter cppWriter, SourceCodeTable src, ModuleNameTable curModule)
        {
            headerText = new IndentedTextWriter(headerWriter);
            cppText = new IndentedTextWriter(cppWriter);
            source = src;
            moduleName = currentModule.ModuleData.Name;

            scope = new Scope();
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

            foreach (var key in currentModule.Routines.Keys)
            {
                if (currentModule.Routines[key].Header != null)
                {
                    headerText.WriteLine($"#include \"{currentModule.Routines[key].Header.File}.h\"");
                }
            }

            // cpp includes
            cppText.WriteLine($"#include \"{moduleName}.h\"");
            
            base.VisitStart(context);

            headerText.WriteLine($"#endif");

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
            return base.VisitFunctionDecl(context);
        }

        public override object VisitProcedureDecl([NotNull] SLangGrammarParser.ProcedureDeclContext context)
        {
            return base.VisitProcedureDecl(context);
        }

        public override object VisitRoutineArgList([NotNull] SLangGrammarParser.RoutineArgListContext context)
        {
            return base.VisitRoutineArgList(context);
        }

        public override object VisitModuleFieldDecl([NotNull] SLangGrammarParser.ModuleFieldDeclContext context)
        {
            return base.VisitModuleFieldDecl(context);
        }
        // types logic
        public override object VisitSimpleType([NotNull] SLangGrammarParser.SimpleTypeContext context)
        {
            var res = new SlangSimpleType(context.SimpleType().GetText());
            WriteAll(GetStringFromType(res));
            return res;
        }

        public override object VisitArrayType([NotNull] SLangGrammarParser.ArrayTypeContext context)
        {
            var dimension = context.arrayDimention().Length;
            WriteVectorTypeStart(dimension);
            var type = Visit(context.scalarType()) as SlangType;
            WriteVectorTypeEnd(dimension);

            return new SlangArrayType(type, dimension);
        }

        public override object VisitPtrType([NotNull] SLangGrammarParser.PtrTypeContext context)
        {
            WriteAll("std::shared_ptr<");
            SlangCustomType customType = null;
            if (context.customType() != null)
            {
                customType = Visit(context.customType()) as SlangCustomType;
            } 
            else 
            {
                customType = SlangCustomType.Object;
                WriteAll(GetStringFromType(customType));
            };
            WriteAll(">");
            return new SlangPointerType(customType);
        }

        public override object VisitCustomType([NotNull] SLangGrammarParser.CustomTypeContext context)
        {
            string moduleName = "", typeName = "";
            var ids = context.qualident().Id().Select(x => x.GetText()).ToArray();

            if (ids.Count() == 1)
            {
                moduleName = this.moduleName;
                typeName = ids[0];
            }
            else if (ids.Count() == 2)
            {
                moduleName = ids[0];
                typeName = ids[1];

                WriteAll($"{ids[0]}::");
            }
            WriteAll(typeName);
            return new SlangCustomType(moduleName, typeName);
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
            headerText.WriteLine("}");
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
            headerText.Write(method.IsFunction() ? GetStringFromType(method.ReturnType) : "void");
            headerText.Write($" {method.Name}(");
            WriteParameters(headerText, method.Params);
            headerText.Write(")");
            if (method.IsAbstract)
            {
                headerText.Write(" = 0");
            }
            headerText.WriteLine(";");
        }

        private void WriteParameters(IndentedTextWriter writer, IList<RoutineArgNameTableItem> args)
        {
            foreach (var arg in args)
            {
                writer.Write(GetStringFromType(arg.TypeArg.Type));
                if (arg.TypeArg.Modifier == ParamModifier.Ref)
                {
                    headerText.Write('&');
                }
                writer.Write($" {arg.Name}");
                if (arg != args.Last())
                {
                    writer.Write(", ");
                }
            }
        }

        // helpers
        private string GetStringFromType(SlangType returnType)
        {
            var res = "";
            var simpleTypes = new Dictionary<string, string>
            {
                { CompilerConstants.IntegerType, "int" },
                { CompilerConstants.RealType, "float" },
                { CompilerConstants.CharacterType, "char" },
                { CompilerConstants.BooleanType, "bool" },
                { CompilerConstants.StringType, "std::string" }
            };
            if (returnType is SlangSimpleType st)
            {
                res = simpleTypes[st.Name];
            }
            else if (returnType is SlangArrayType at)
            {
                WriteVectorTypeStart(at.Dimension);
                res += GetStringFromType(at.Type);
                WriteVectorTypeEnd(at.Dimension);
            }
            else if (returnType is SlangCustomType ct)
            {
                res = ct.ModuleName == moduleName ? ct.Name : $"{ct.ModuleName}::{ct.Name}";
            }
            else if (returnType is SlangPointerType pt)
            {
                res = $"std::shared_ptr<{GetStringFromType(pt.PtrType)}>";
            }
            return res;
        }

        private void WriteEverywhere() => canWriteHeader = true;
        private void WriteNotInHeader() => canWriteHeader = false;

        private void WriteAll(string text)
        {
            if (canWriteHeader)
            {
                headerText.Write(text);
            }
            
            cppText.Write(text);
        }

        private void WriteLineAll(string text)
        {
            if (canWriteHeader)
            {
                headerText.WriteLine(text);
            }

            cppText.WriteLine(text);
        }

        private void WriteVectorTypeStart(int dimension)
        {
            for (int i = 0; i < dimension; ++i)
            {
                WriteAll("std::vector<");
            }
        }

        private void WriteVectorTypeEnd(int dimension)
        {
            for (int i = 0; i < dimension; ++i)
            {
                WriteAll(">");
            }
        }
    }
}
