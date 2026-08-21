namespace Cereal_API.WPF.Filters;

public class FilterRule
{
    public string PropertyName { get; set; } = string.Empty;

    public FilterOperator Operator { get; set; }

    public object? Value { get; set; }
}
