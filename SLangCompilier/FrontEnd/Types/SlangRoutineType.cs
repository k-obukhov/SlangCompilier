using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    public abstract class SlangRoutineType: SlangBaseType
    {
        public IList<SlangRoutineArgType> Args { get; }

        public SlangRoutineType(IList<SlangRoutineArgType> args)
        {
            Args = args;
        }
    }
}
