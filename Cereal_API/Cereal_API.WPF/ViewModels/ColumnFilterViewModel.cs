using Cereal_API.Shared;
using Cereal_API.WPF.Filters;
using System.Collections;

namespace Cereal_API.WPF.ViewModels;

public class ColumnFilterViewModel : ViewModelBase
{
    public static readonly FilterOperator[] textOperators =
    [
        FilterOperator.Equals,
        FilterOperator.NotEquals,
        FilterOperator.Contains,
        FilterOperator.StartsWith,
        FilterOperator.EndsWith
    ];

    public static readonly FilterOperator[] numberOperators = 
    [
        FilterOperator.Equals,
        FilterOperator.NotEquals,
        FilterOperator.GreaterThan,
        FilterOperator.GreaterThanOrEqual,
        FilterOperator.LessThan,
        FilterOperator.LessThanOrEqual
    ];

    public static readonly FilterOperator[] enumOperators =
    [
        FilterOperator.Equals,
        FilterOperator.NotEquals
    ];

    public string PropertyName { get; }

    public Type PropertyType { get; }

    public FilterOperator[] AvailableOperators { get; } = [];

    private FilterOperator _selectedOperator;
    public FilterOperator SelectedOperator 
    {
        get => _selectedOperator; 
        set
        {
            _selectedOperator = value;
            OnPropertyChanged();
        }
    }

    private object? _filterValue;
    public object? FilterValue 
    {
        get => _filterValue; 
        set
        {
            _filterValue = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable? AvailableValues { get; }

    public bool IsActive => FilterValue != null && !string.IsNullOrWhiteSpace(FilterValue.ToString());

    public ColumnFilterViewModel(string propertyName, Type propertyType)
    {
        PropertyName = propertyName;
        PropertyType = propertyType;

        AvailableOperators = PropertyType switch
        {
            Type _ when propertyType == typeof(string) => textOperators,
            Type _ when propertyType == typeof(int) => numberOperators,
            Type _ when propertyType == typeof(float) => numberOperators,
            Type _ when propertyType == typeof(CerealEnums.Manufacturer) => enumOperators,
            Type _ when propertyType == typeof(CerealEnums.Type) => enumOperators,
            _ => []
        };

        AvailableValues = propertyType switch
        {
            Type _ when propertyType == typeof(CerealEnums.Manufacturer) => Enum.GetValues<CerealEnums.Manufacturer>(),
            Type _ when propertyType == typeof(CerealEnums.Type) => Enum.GetValues<CerealEnums.Type>(),
            _ => null
        };
    }
}
