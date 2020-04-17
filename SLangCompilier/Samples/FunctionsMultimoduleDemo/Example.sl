module Example
// there is no "start"-"end" block, because it's not a Main module

public function(val integer arg): integer test // public -- is available from other modules
    return arg * 2;
end

private procedure(val integer temp) valuePass // private -- is not available from other modules
    let temp := temp + 1;
end

public procedure(ref integer temp) refPass 
    let temp := temp + 1;
end

public const integer myConst := 42;
public variable integer myField := myConst;
public readonly variable string moduleName := "Example";