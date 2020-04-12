module Main

public function(val integer arg): integer test // public -- is available from other modules
    return arg * 2;
end

private procedure(val integer temp) valuePass // private -- is not available from other modules
    let temp := temp + 1;
end

public procedure(ref integer temp) refPass 
    let temp := temp + 1;
end

start
    variable integer i := 0;
    while (i < 5) do
        output test(i), '\n';
        let i := i + 1;
    end

    variable integer arg := 41;
    call valuePass(arg);
    output arg, '\n';
    call Main.refPass(arg); // "call refPass(arg);" is the same
    output arg, '\n';
end