using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    class SlangNullType : SlangType
    {
        public override bool Equals(SlangType other)
        {
            if (other is SlangPointerType)
            {
                return true;
            }
            return false;
        }

        public override string ToString() => CompilerConstants.NullType;
    }
}
