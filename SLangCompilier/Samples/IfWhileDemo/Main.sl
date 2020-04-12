module Main
start
    variable integer first;
    variable integer second;

    input first, second;
    output "first value is ", first, '\n';
    output "second value is ", second, '\n';

    if ((first == 42) && (second == 42)) then
        output "first == second == 42!\n";
    end

    if (first == second) then
        output "first and second are the same!\n";
    else
        output "first and second are not the same!\n";
    end

    if (first > second) then
        output "first is more than second\n";
    elseif (first < second) then
        output "first is less than second\n";
    else
        output "first and second are the same\n";
    end

    variable integer i := 0;
    while (i < first) do
        output "*";
        let i := i + 1;
    end
    output '\n';

    variable integer j := 0;
    repeat
        output "*";
        let j := j + 1;
    while (j < first)
end