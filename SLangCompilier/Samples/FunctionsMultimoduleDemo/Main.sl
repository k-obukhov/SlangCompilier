import Example
module Main

start
    variable integer i := 0;
    while (i < 5) do
        output Example.test(i), '\n';
        let i := i + 1;
    end

    variable integer arg := Example.myConst - 1;

    output "Before = ", Example.myField, '\n';
    let Example.myField := Example.myField + 1;
    output "After = ", Example.myField, '\n';

    //call Example.valuePass(arg); // Compiler Error -- procedure is private
    output arg, '\n';
    call Example.refPass(arg); // "call refPass(arg);" is the same
    output arg, '\n';
end