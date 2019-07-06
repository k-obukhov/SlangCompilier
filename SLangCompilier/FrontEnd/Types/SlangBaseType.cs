using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    public abstract class SlangBaseType : IEquatable<SlangBaseType>
    {
        public abstract bool Equals(SlangBaseType other);
        public abstract bool IsAssignable(SlangBaseType other);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((SlangBaseType)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
