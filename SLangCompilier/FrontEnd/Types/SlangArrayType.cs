using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    /// <summary>
    /// Base class for arrays
    /// </summary>
    class SlangArrayType: SlangType
    {
        public SlangType Type { get; set; }
        public int Dimension { get; set; }

        public SlangArrayType(SlangType type, int dimension)
        {
            Type = type;
            Dimension = dimension;
        }

        public override bool Equals(SlangType other) => other is SlangArrayType arr && arr.Type.Equals(Type) && (arr.Dimension == Dimension);

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("array ");
            for (int i = 0; i < Dimension; ++i)
            {
                sb.Append("[]");
            }
            sb.Append($" {Type.ToString()}");
            return sb.ToString();
        }
    }
}
