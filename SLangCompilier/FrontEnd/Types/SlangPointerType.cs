namespace SLangCompiler.FrontEnd.Types
{
    class SlangPointerType : SlangType
    {
        public SlangCustomType PtrType { get; set; }

        public SlangPointerType(SlangCustomType type)
        {
            PtrType = type;
        }

        public SlangPointerType()
        {
            PtrType = SlangCustomType.Object;
        }
        public override string ToString() => $"pointer ({PtrType})";

        public override bool Equals(SlangType other) => other is SlangPointerType t && t.PtrType.Equals(PtrType);
    }
}
