namespace SLangCompiler.FrontEnd
{
    enum ExpressionValueType
    {
        Value, // l-value + readonly fields and consts
        Variable, // r-value
        Nothing // return type is null (void) -- for procedure calls
    }
}