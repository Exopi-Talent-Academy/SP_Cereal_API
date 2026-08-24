using System.ComponentModel;

namespace Cereal_API.WPF.ViewModels;

public class ColumnHeaderViewModel : ViewModelBase
{
    public string PropertyName { get; }

    public ColumnFilterViewModel Filter { get; }

    public string FilterButtonText => Filter.IsActive ? "⫧" : "▼";

    public ColumnHeaderViewModel(string propertyName, Type propertyType)
    {
        PropertyName = propertyName;

        Filter = new(propertyName, propertyType);

        Filter.PropertyChanged += FilterPropertyChanged;
    }

    public void FilterPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(FilterButtonText));
    }
}
