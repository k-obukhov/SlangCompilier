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
            headerText.WriteLine($"namespace {currentModule.ModuleData.Name}");
            headerText.WriteLine("{");
            headerText.Indent++;
        }

        public void TranslateInitializeBlockOnFinish()
        {
            headerText.Indent--;
            headerText.WriteLine("}");
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

            headerText.WriteLine($"private");
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
            headerText.Write(" (");
            WriteParameters(method.Params);
            headerText.Write(")");
            if (method.IsAbstract)
            {
                headerText.Write(" = 0");
            }
            headerText.WriteLine(";");
        }

        private void WriteParameters(IList<RoutineArgNameTableItem> args)
        {
            foreach (var arg in args)
            {
                headerText.Write(GetStringFromType(arg.TypeArg.Type));
                if (arg.TypeArg.Modifier == ParamModifier.Ref)
                {
                    headerText.Write('&');
                }
                headerText.Write($" {arg.Name}");
                if (arg != args.Last())
                {
                    headerText.Write(", ");
                }
            }
        }

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
                    res += "vector<";
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
    }
}
