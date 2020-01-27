using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.BackEnd.Translator
{
    public abstract class BaseBackEndTranslator
    {
        private readonly StringBuilder builder;

        public BaseBackEndTranslator()
        {
            builder = new StringBuilder();
        }

        public string GetText() => builder.ToString();
    }
}
