using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.BackEnd.Translator
{
    public abstract class BaseBackendTranslator
    {
        private readonly StringBuilder builder;

        public BaseBackendTranslator()
        {
            builder = new StringBuilder();
        }

        public string GetText() => builder.ToString();
    }
}
