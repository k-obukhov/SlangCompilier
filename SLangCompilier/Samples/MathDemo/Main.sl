import Math
module Main
start
    output "PI = ", Math.PI, '\n';
    output "E = ", Math.E, '\n';
    variable real x := 1.5678;
    variable integer y := -12;
    output "Sin = ", Math.Sin(x), '\n';
    output "Cos = ", Math.Cos(x), '\n';
    output "Tan = ", Math.Tan(x), '\n';
    output "ATan = ", Math.ATan(x), '\n';
    output "ACos = ", Math.ACos(x), '\n';
    output "ASin = ", Math.ASin(x), '\n';
    output "ATan2 = ", Math.ATan2(x, x), '\n';
    output "Exp = ", Math.Exp(x), '\n';
    output "Log = ", Math.Log(x), '\n';
    output "Log10 = ", Math.Log10(x), '\n';
    output "Log2 = ", Math.Log2(x), '\n';
    output "Exp2 = ", Math.Exp2(x), '\n';
    output "Pow = ", Math.Pow(x, x), '\n';
    output "Sqrt = ", Math.Sqrt(x), '\n';
    output "Ceil = ", Math.Ceil(x), '\n';
    output "Floor = ", Math.Floor(x), '\n';
    output "Round = ", Math.Round(x), '\n';
    output "Fabs = ", Math.Fabs(x), '\n';
    output "Abs = ", Math.Abs(y), '\n';
end