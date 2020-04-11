import Random
module Main
start
    variable integer i := 0;
    while (i < 10) do
        output Random.Rnd(), '\n';
        output "Random int = ", Random.RangeInteger(0, 6), '\n';
        output "Random real = ", Random.RangeReal(1.0, 2.0), '\n';
        let i := i + 1;
    end
end