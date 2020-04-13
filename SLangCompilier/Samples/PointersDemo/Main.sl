import System
module Main

public base class Base // class marked as base class
    public variable integer myField;
    private variable integer myPrivateField;
end

public (Base b) function (): integer myMethod
    return 41;
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
    pointer(Base) ptrToBase := nil; // is null
    let ptrToBase := new(Derived);
    output ptrToBase.myMethod(), '\n';
    let ptrToBase := new(Base);
    output ptrToBase.myMethod(), '\n';

    //pointer(AbstractDemo) secondPtr := new(AbstractDemo); // Error!
    //pointer(AbstractDemo) secondPtr := new(AlsoAbstract); // Error!
    pointer(AbstractDemo) secondPtr := new(NotAbstract);
    call secondPtr.Demo();
end