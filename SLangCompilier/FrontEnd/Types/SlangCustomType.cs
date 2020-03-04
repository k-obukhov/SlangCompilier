namespace SLangCompiler.FrontEnd.Types
{
    /// <summary>
    /// base custom type checks
    /// </summary>
    public class SlangCustomType : SlangType
    {
        public static SlangCustomType Object => new SlangCustomType(CompilerConstants.SystemModuleName, CompilerConstants.ObjectClassName);

        public string Name { get; set; }
        public string ModuleName { get; set; }

        public SlangCustomType(string moduleName, string name)
        {
            ModuleName = moduleName;
            Name = name;
        }

        public override string ToString() => $"{ModuleName}.{Name}";

        /// <summary>
        /// Base type check, does not check LSP principle
        /// </summary>
        /// <param name="other">what type we check</param>
        /// <returns></returns>
        public override bool Equals(SlangType other) => (other is SlangCustomType t) && (t.Name == Name) && (t.ModuleName == ModuleName);
    }
}
