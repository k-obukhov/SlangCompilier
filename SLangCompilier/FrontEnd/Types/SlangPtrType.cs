using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    public class SlangPtrType : SlangBaseType
    {
        public string ClassName { get; }

        public SlangPtrType(string className)
        {
            ClassName = className;
        }

        public override string ToString()
        {
            var res = "pointer";
            if (ClassName != null)
            {
                res = $"{res} ({ClassName})";
            }
            return res;
        }

        // TODO: Продумать логику проверки на сравнение, возможно, понадобится зависимость в виде vtable или ее аналога
        // Объекты -- указательный тип, который явно задан, нужно проверять иерархию класса
        // Возможна новая сущность SlangClassType
        public override bool Equals(SlangBaseType other)
        {
            throw new NotImplementedException();
        }

        public override bool IsAssignable(SlangBaseType other)
        {
            throw new NotImplementedException();
        }
    }
}
