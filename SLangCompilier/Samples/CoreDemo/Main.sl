import Core
module Main
start
    output Core.ToReal(1234), '\n';
    output Core.ToInteger(123.093), '\n';
    output Core.Ord('f'), '\n';
    output Core.Chr(Core.Ord('f'));
    call Core.Pause();
end