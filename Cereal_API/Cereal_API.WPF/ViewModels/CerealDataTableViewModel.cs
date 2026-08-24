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

    public ObservableCollection<CerealData> AllCereals { get; private set; } = [];
    public ObservableCollection<CerealData> FilteredCereals { get; private set; } = [];

    #region Columns
    public ColumnHeaderViewModel IdColumn { get; private set; } = new ColumnHeaderViewModel("Id", typeof(int));
    public ColumnHeaderViewModel NameColumn { get; private set; } = new ColumnHeaderViewModel("Name", typeof(string));
    public ColumnHeaderViewModel ManufacturerColumn { get; private set; } = new ColumnHeaderViewModel("Manufacturer", typeof(CerealEnums.Manufacturer));
    public ColumnHeaderViewModel TypeColumn { get; private set; } = new ColumnHeaderViewModel("Type", typeof(CerealEnums.Type));
    public ColumnHeaderViewModel CaloriesColumn { get; private set; } = new ColumnHeaderViewModel("Calories", typeof(int));
    public ColumnHeaderViewModel ProteinColumn { get; private set; } = new ColumnHeaderViewModel("Protein", typeof(int));
    public ColumnHeaderViewModel FatColumn { get; private set; } = new ColumnHeaderViewModel("Fat", typeof(int));
    public ColumnHeaderViewModel SodiumColumn { get; private set; } = new ColumnHeaderViewModel("Sodium", typeof(int));
    public ColumnHeaderViewModel FiberColumn { get; private set; } = new ColumnHeaderViewModel("Fiber", typeof(float));
    public ColumnHeaderViewModel CarboColumn { get; private set; } = new ColumnHeaderViewModel("Carbo", typeof(float));
    public ColumnHeaderViewModel SugarsColumn { get; private set; } = new ColumnHeaderViewModel("Sugars", typeof(int));
    public ColumnHeaderViewModel PotassColumn { get; private set; } = new ColumnHeaderViewModel("Potass", typeof(int));
    public ColumnHeaderViewModel VitaminsColumn { get; private set; } = new ColumnHeaderViewModel("Vitamins", typeof(int));
    public ColumnHeaderViewModel ShelfColumn { get; private set; } = new ColumnHeaderViewModel("Shelf", typeof(int));
    public ColumnHeaderViewModel WeightColumn { get; private set; } = new ColumnHeaderViewModel("Weight", typeof(float));
    public ColumnHeaderViewModel CupsColumn { get; private set; } = new ColumnHeaderViewModel("Cups", typeof(float));
    public ColumnHeaderViewModel RatingColumn { get; private set; } = new ColumnHeaderViewModel("Rating", typeof(float));
    #endregion
    public ObservableCollection<ColumnHeaderViewModel> Columns { get; private set; }

    public RelayCommand LoadCerealsCommand {  get; private set; }

    public CerealEnums.Manufacturer[] ManufacturerOptions { get; private set; }

    public CerealDataTableViewModel(CerealHttpService api/*, LoginViewModel loginVM*/)
    {
        _api = api;
        //_loginVM = loginVM;

        ManufacturerOptions = (CerealEnums.Manufacturer[])Enum.GetValues(typeof(CerealEnums.Manufacturer));

        Columns =
        [
            IdColumn,
            NameColumn,
            ManufacturerColumn,
            TypeColumn,
            CaloriesColumn,
            ProteinColumn,
            FatColumn,
            SodiumColumn,
            FiberColumn,
            CarboColumn,
            SugarsColumn,
            PotassColumn,
            VitaminsColumn,
            ShelfColumn,
            WeightColumn,
            CupsColumn,
            RatingColumn
        ];

        foreach (var column in Columns)
        {
            column.Filter.PropertyChanged += FilterPropertyChanged;
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
            bool matches = Columns
                .Where(c => c.Filter.IsActive)
                .All(c => CerealFilter.Matches(cereal, c.Filter));

            if (matches)
            {
                FilteredCereals.Add(cereal);
            }
        }
    }
}
