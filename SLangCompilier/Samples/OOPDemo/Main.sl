import System
module Main

public base class Base // class marked as base class
    public variable integer myField;
    private variable integer myPrivateField;
end

public (Base b) function (): integer myMethod
    let b.myField := 42;
    return b.myField;
end

public class Derived inherit(Base) // if Base was not marked as "base" then compiler error
end

public override (Derived d) function (): integer myMethod // if you override method you should mark this as override (field overriding is not supported)
    return 42;
end

// abstract classes

public base class AbstractDemo
end

public abstract (AbstractDemo d) procedure () Demo 
end

public base class AlsoAbstract inherit(AbstractDemo)
end

public class NotAbstract inherit(AlsoAbstract)
end

public override (NotAbstract obj) procedure () Demo
    output "Hello, World!";
end

start
    variable Derived d;
    output d.myMethod(); // myMethod was inherited from Base class, because it's marked as public
    //variable AbstractDemo g; // Error!
    //variable AlsoAbstract a; // Error!
    //let d.myPrivateField := 0; // Error!
    variable NotAbstract n;
    call n.Demo();
end