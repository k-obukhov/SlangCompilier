using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    public class SlangRoutineArgType
    {
        public string Modifier { get; }
        public SlangBaseType Type { get; }

        public SlangRoutineArgType(string modifier, SlangBaseType type)
        {
            Modifier = modifier;
            Type = type;
        }

        public override string ToString() => $"{Modifier} {Type}";

        public override bool Equals(object obj)
        {
            if (obj is null)
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

            return Equals((SlangRoutineArgType)obj);
        }

        protected bool Equals(SlangRoutineArgType other)
        {
            return string.Equals(Modifier, other.Modifier) && Equals(Type, other.Type);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
