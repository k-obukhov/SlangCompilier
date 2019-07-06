using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    public class SlangArrayType : SlangBaseType
    {
        // скольки мерный массив
        public int Dimensions { get; }
        public SlangBaseType Type { get; }
        public SlangArrayType(int dimensions, SlangBaseType type)
        {
            Dimensions = dimensions;
            Type = type;
        }

        public override bool Equals(SlangBaseType other)
        {
            return IsAssignable(other);
        }

        public override bool IsAssignable(SlangBaseType other)
        {
            if (!(other is SlangArrayType arrType))
            {
                return false;
            }
            return arrType.Dimensions == Dimensions && Type.Equals(arrType.Type);
        }

        public override int GetHashCode()
        {
            // maybe fix that
            return base.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("array ");
            for (int i = 0; i < Dimensions; ++i)
            {
                sb.Append("[]");
            }
            sb.Append($" {Type.ToString()}");
            return sb.ToString();
        }
    }
}
