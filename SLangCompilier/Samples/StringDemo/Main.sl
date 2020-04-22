import String
module Main
start
    variable string first := "hello";
    variable string second := "world";

    output String.Equals(first, second);
    output String.Less(first, second);
    output String.Greater(first, second);
    output String.LessOrEquals(first, second);
    output String.GreaterOrEquals(first, second);
    output String.NotEquals(first, second);

    variable string temp := String.AddChar(first, ' ');
    variable string res := String.Concat(temp, second);
    output '\n', res, '\n';
    output "Length = ", String.Length(res), '\n';

    variable integer myInt := 5;
    variable real myReal := 456.345;
    output String.Concat(String.IntToString(myInt), String.RealToString(myReal));
end