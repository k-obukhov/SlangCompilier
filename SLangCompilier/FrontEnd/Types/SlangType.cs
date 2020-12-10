namespace SLangCompiler.FrontEnd.Types
{
    /// <summary>
    /// Base type infrastructure
    /// </summary>
    public abstract class SlangType
    {
        public abstract bool Equals(SlangType other);
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

            return Equals((SlangType)obj);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }
    }
}
