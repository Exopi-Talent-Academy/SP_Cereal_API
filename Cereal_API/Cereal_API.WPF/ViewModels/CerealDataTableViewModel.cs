using Cereal_API.Shared;
using Cereal_API.Shared.Models;
using Cereal_API.WPF.Commands;
using Cereal_API.WPF.Filters;
using Cereal_API.WPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cereal_API.WPF.ViewModels;

public class CerealDataTableViewModel : ViewModelBase
{
    private readonly CerealHttpService _api;
    //private readonly LoginViewModel _loginVM;

    public RelayCommand LoadCerealsCommand {  get; private set; }
    public ObservableCollection<CerealData> AllCereals { get; private set; } = [];
    public ObservableCollection<CerealData> FilteredCereals { get; private set; } = [];
    public ObservableCollection<ColumnFilterViewModel> ColumnFilters { get; private set; }

    public CerealEnums.Manufacturer[] ManufacturerOptions { get; private set; }

    public CerealDataTableViewModel(CerealHttpService api/*, LoginViewModel loginVM*/)
    {
        _api = api;
        //_loginVM = loginVM;

        ManufacturerOptions = (CerealEnums.Manufacturer[])Enum.GetValues(typeof(CerealEnums.Manufacturer));

        ColumnFilters =
        [
            new ColumnFilterViewModel("Id", typeof(int)),
            new ColumnFilterViewModel("Name", typeof(string)),
            new ColumnFilterViewModel("Manufacturer", typeof(CerealEnums.Manufacturer)),
            new ColumnFilterViewModel("Type", typeof(CerealEnums.Type)),
            new ColumnFilterViewModel("Calories", typeof(int)),
            new ColumnFilterViewModel("Protein", typeof(int)),
            new ColumnFilterViewModel("Fat", typeof(int)),
            new ColumnFilterViewModel("Sodium", typeof(int)),
            new ColumnFilterViewModel("Fiber", typeof(float)),
            new ColumnFilterViewModel("Carbo", typeof(float)),
            new ColumnFilterViewModel("Sugars", typeof(int)),
            new ColumnFilterViewModel("Potass", typeof(int)),
            new ColumnFilterViewModel("Vitamins", typeof(int)),
            new ColumnFilterViewModel("Shelf", typeof(int)),
            new ColumnFilterViewModel("Weight", typeof(float)),
            new ColumnFilterViewModel("Cups", typeof(float)),
            new ColumnFilterViewModel("Rating", typeof(float)),
        ];

        foreach (var filter in ColumnFilters)
        {
            filter.PropertyChanged += FilterPropertyChanged;
        }

        LoadCerealsCommand = new RelayCommand(
            execute: _ => _ = LoadCereals(),
            canExecute: _ => true);
    }

    public async Task LoadCereals()
    {
        AllCereals.Clear();

        foreach (CerealData cereal in await _api.GetAllCereals())
        {
            AllCereals.Add(cereal);
        }

        ApplyFilters();
    }

    private void FilterPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ColumnFilterViewModel.FilterValue) ||
            e.PropertyName == nameof(ColumnFilterViewModel.SelectedOperator))
        {
            ApplyFilters();
        }
    }

    public void ApplyFilters()
    {
        FilteredCereals.Clear();

        foreach(CerealData cereal in AllCereals)
        {
            bool matches = ColumnFilters
                .Where(filter => filter.IsActive)
                .All(filter => CerealFilter.Matches(cereal, filter));

            if (matches)
            {
                FilteredCereals.Add(cereal);
            }
        }
    }
}
