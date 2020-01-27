using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    /// <summary>
    /// Simple types -- int, float, char, bool
    /// </summary>
    public class SlangSimpleType: SlangType
    {
        public static SlangSimpleType Int => new SlangSimpleType(CompilerConstants.IntegerType);
        public static SlangSimpleType Real => new SlangSimpleType(CompilerConstants.RealType);
        public static SlangSimpleType Character => new SlangSimpleType(CompilerConstants.CharacterType);
        public static SlangSimpleType Boolean => new SlangSimpleType(CompilerConstants.BooleanType);
        public static SlangSimpleType String => new SlangSimpleType(CompilerConstants.StringType);

        public SlangSimpleType(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override string ToString() => Name;

        public override bool Equals(SlangType other) => other is SlangSimpleType s && s.Name == Name;
    }
}
