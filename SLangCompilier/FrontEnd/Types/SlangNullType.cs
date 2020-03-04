namespace SLangCompiler.FrontEnd.Types
{
    public class SlangNullType : SlangType
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
