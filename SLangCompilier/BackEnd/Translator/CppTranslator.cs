using SLangCompiler.FrontEnd.Tables;
using SLangCompiler.FrontEnd.Types;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SLangCompiler.BackEnd.Translator
{
    public class CppTranslator: BaseBackEndTranslator
    {
        private readonly IndentedTextWriter headerText;
        private readonly IndentedTextWriter cppText;
        private readonly SourceCodeTable source;
        private ModuleNameTable currentModule;

        public CppTranslator(TextWriter headerWriter, TextWriter cppWriter, SourceCodeTable src)
        {
            headerText = new IndentedTextWriter(headerWriter);
            cppText = new IndentedTextWriter(cppWriter);
            source = src;
        }

        public void TranslateOnStart(string moduleName)
        {
            currentModule = source.Modules[moduleName];
            headerText.WriteLine($"#ifndef {moduleName}_H");
            headerText.WriteLine($"#define {moduleName}_H");

            // header includes
            headerText.WriteLine("#include <iostream>");
            headerText.WriteLine("#include <vector>");
            headerText.WriteLine("#include <memory>");
            headerText.WriteLine("#include <string>");

            foreach (var module in currentModule.ImportedModules)
            {
                headerText.WriteLine($"#include \"{module}.h\"");
            }

            foreach (var key in currentModule.Routines.Keys)
            {
                if (currentModule.Routines[key].Header != null)
                {
                    headerText.WriteLine($"#include \"{currentModule.Routines[key].Header.File}.h\"");
                }
            }

            // cpp includes
            cppText.WriteLine($"#include \"{moduleName}.h\"");
        }

        public void TranslateInitalizeBlockOnStart()
        {
            WriteLineAll($"namespace {currentModule.ModuleData.Name}");
            WriteLineAll("{");
            headerText.Indent++;
            cppText.Indent++;
            // all declarations starts here...
        }

        public void TranslateInitializeBlockOnFinish()
        {
            headerText.Indent--;
            cppText.Indent--;
            WriteLineAll("}");
        }

        public void TranslateOnFinish()
        {
            headerText.WriteLine($"#endif");
        }

        public void TranslateCustomType(string typeName)
        {
            var typeData = currentModule.Classes[typeName];
            headerText.WriteLine($"class {typeName}");
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

        public void TranslateClassFieldDecl(VariableNameTableItem field)
        {
            headerText.Write($"{GetStringFromType(field.Type)} {field.Name};");
        }

        public void TranslateMethodDecl(MethodNameTableItem method)
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

        private void TranslateRoutineDecl(RoutineNameTableItem routine)
        {
            headerText.Write(routine.IsFunction() ? GetStringFromType(routine.ReturnType) : "void");
            headerText.Write($" {routine.Name}(");
            WriteParameters(headerText, routine.Params);
            headerText.Write(");");
            headerText.WriteLine();
        }

        private void TranslateFieldDecl(ModuleFieldNameTableItem item)
        {
            headerText.WriteLine($"extern {GetStringFromType(item.Type)} {item.Name};");
        }

        // common functions

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
                for (int i = 0; i < at.Dimension; ++i)
                {
                    res += "std::vector<";
                }
                res += GetStringFromType(at.Type);
                for (int i = 0; i < at.Dimension; ++i)
                {
                    res += '>';
                }
            }
            else if (returnType is SlangCustomType ct)
            {
                res = $"{ct.ModuleName}::{ct.Name}";
            }
            else if (returnType is SlangPointerType pt)
            {
                res = $"std::shared_ptr<{GetStringFromType(pt.PtrType)}>";
            }
            return res;
        }

        private void WriteAll(string text)
        {
            headerText.Write(text);
            cppText.Write(text);
        }

        private void WriteLineAll(string text)
        {
            headerText.WriteLine(text);
            cppText.WriteLine(text);
        }
    }
}
