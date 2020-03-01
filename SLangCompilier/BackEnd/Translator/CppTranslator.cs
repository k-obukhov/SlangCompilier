using SLangCompiler.FrontEnd.Tables;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SLangCompiler.BackEnd.Translator
{
    public class CppTranslator: BaseBackEndTranslator
    {
        private IndentedTextWriter headerText;
        private IndentedTextWriter cppText;
        private SourceCodeTable source;
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
            headerText.WriteLine($"class {typeName}");
            headerText.WriteLine("{");
            headerText.Indent++;
            
            headerText.Indent--;
            headerText.WriteLine("}");
        }
    }
}
