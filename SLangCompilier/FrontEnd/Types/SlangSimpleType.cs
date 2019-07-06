using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    public class SlangSimpleType: SlangBaseType
    {
        public static readonly SlangSimpleType Integer = new SlangSimpleType(CompilerConstants.IntegerType);
        public static readonly SlangSimpleType Real = new SlangSimpleType(CompilerConstants.RealType);
        public static readonly SlangSimpleType Boolean = new SlangSimpleType(CompilerConstants.BooleanType);
        public static readonly SlangSimpleType Character = new SlangSimpleType(CompilerConstants.CharacterType);

        public string TypeName { get; }

        public SlangSimpleType(string typeName)
        {
            TypeName = typeName;
        }

        public override bool Equals(SlangBaseType other)
        {
            return other is SlangSimpleType type && string.Equals(TypeName, type.TypeName);
        }

        public override bool IsAssignable(SlangBaseType other)
        {
            // типы совпадают
            if (Equals(other))
            {
                return true;
            }
            // тип == не является простым
            if (!(other is SlangSimpleType otherType))
            {
                return false;
            }
            // преобразование float -> integer
            return TypeName == CompilerConstants.RealType && otherType.TypeName == CompilerConstants.IntegerType;
        }

        public override int GetHashCode() => TypeName?.GetHashCode() ?? 0;

        public override string ToString() => TypeName;
    }
}
