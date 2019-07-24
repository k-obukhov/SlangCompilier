using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    /// <summary>
    /// base custom type checks
    /// maybe need to store module name?
    /// </summary>
    class SlangCustomType: SlangType
    {
        public static SlangCustomType Object => new SlangCustomType("Object");

        public string Name { get; set; }

        public SlangCustomType(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;

        /// <summary>
        /// Base type check, does not check LSP principle
        /// </summary>
        /// <param name="other">what type we check</param>
        /// <returns></returns>
        public override bool Equals(SlangType other) => (other is SlangCustomType t) && (t.Name == Name);
    }
}
