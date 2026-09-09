namespace Cereal_API.WPF.Filters;

public static class CerealFilter
{
    public static bool Matches(CerealData cereal, ColumnFilterViewModel filter)
    {
        var property = typeof(CerealData).GetProperty(filter.PropertyName);

        if (property == null)
            return true;

        var value = property.GetValue(cereal);
        var filterValue = ConvertValue(filter.FilterValue, property.PropertyType);

        return Compare(value, filterValue, filter.SelectedOperator);
    }

    private static object? ConvertValue(object? value, Type targetType)
    {
        if (value == null)
            return null;

        if (targetType.IsEnum)
        {
            if (value is string stringValue)
                return Enum.Parse(targetType, stringValue);

            return value;
        }

        return Convert.ChangeType(value, targetType);
    }

    private static bool Compare(object? actual, object? filterValue, FilterOperator operation)
    {
        if (actual == null || filterValue == null)
            return false;

        switch (operation)
        {
            case FilterOperator.Equals:
                return Equals(actual, filterValue);

            case FilterOperator.NotEquals:
                return !Equals(actual, filterValue);

            case FilterOperator.Contains:
                return actual.ToString()!.Contains(
                    filterValue.ToString()!, 
                    StringComparison.OrdinalIgnoreCase);

            case FilterOperator.StartsWith:
                return actual.ToString()!.StartsWith(
                    filterValue.ToString()!,
                    StringComparison.OrdinalIgnoreCase);

            case FilterOperator.EndsWith:
                return actual.ToString()!.EndsWith(
                    filterValue.ToString()!,
                    StringComparison.OrdinalIgnoreCase);

            case FilterOperator.GreaterThan:
                return CompareValues(actual, filterValue) > 0;

            case FilterOperator.GreaterThanOrEqual:
                return CompareValues(actual, filterValue) >= 0;

            case FilterOperator.LessThan:
                return CompareValues(actual, filterValue) < 0;

            case FilterOperator.LessThanOrEqual:
                return CompareValues(actual, filterValue) <= 0;

            default:
                return false;
        }
    }

    private static int CompareValues(object actual, object filterValue)
    {
        if (actual is IComparable comparable)
        {
            return comparable.CompareTo(filterValue);
        }
        return 0;
    }
}
