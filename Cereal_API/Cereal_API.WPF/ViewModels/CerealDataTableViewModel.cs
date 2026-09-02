using Cereal_API.Shared;
using Cereal_API.Shared.Models;
using Cereal_API.WPF.Commands;
using Cereal_API.WPF.Filters;
using Cereal_API.WPF.Helpers;
using Cereal_API.WPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Http;

namespace Cereal_API.WPF.ViewModels;

public class CerealDataTableViewModel : ViewModelBase
{
    private readonly CerealHttpService _api;
    private readonly AuthenticationState _authenticationState;

    public ObservableCollection<CerealRowViewModel> AllCereals { get; private set; } = [];
    
    private ObservableCollection<CerealRowViewModel> _filteredCereals = [];
    public ObservableCollection<CerealRowViewModel> FilteredCereals
    {
        get => _filteredCereals;
        private set
        {
            _filteredCereals = value;
            OnPropertyChanged();
        }
    }

    private CerealRowViewModel? _selectedCereal;
    public CerealRowViewModel? SelectedCereal   // This isn't used so it should probably be removed!
    {
        get => _selectedCereal;
        set
        {
            if (_selectedCereal == value)
                return;

            _selectedCereal = value;
            OnPropertyChanged();

            UpdateModifiedCerealCommand.RaiseCanExecuteChanged();
            DeleteCerealCommand.RaiseCanExecuteChanged();
        }
    }

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
    public RelayCommand AddCerealCommand { get; private set; }
    public RelayCommand SaveNewCerealCommand { get; private set; }
    public RelayCommand CancelNewCerealCommand { get; private set; }
    public RelayCommand UpdateModifiedCerealCommand { get; private set; }
    public RelayCommand CancelModifiedCerealCommand { get; private set; }
    public RelayCommand DeleteCerealCommand { get; private set; }

    public CerealEnums.Manufacturer[] ManufacturerOptions { get; private set; }

    public bool IsLoggedIn => _authenticationState.IsLoggedIn;

    private CancellationTokenSource? _filterCancellation;

    public CerealDataTableViewModel(CerealHttpService api, AuthenticationState authenticationState)
    {
        _api = api;
        _authenticationState = authenticationState;

        _authenticationState.PropertyChanged += AuthenticationState_PropertyChanged;

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
            execute: _ => LoadCereals(),
            canExecute: _ => true);

        AddCerealCommand = new RelayCommand(
            execute: _ => AddCereal(),
            canExecute: _ => true);

        SaveNewCerealCommand = new RelayCommand(
            execute: parameter => SaveNewCereal((CerealRowViewModel)parameter!),
            canExecute: parameter => parameter is CerealRowViewModel);

        CancelNewCerealCommand = new RelayCommand(
            execute: parameter => CancelNewCereal((CerealRowViewModel)parameter!),
            canExecute: parameter => parameter is CerealRowViewModel);

        UpdateModifiedCerealCommand = new RelayCommand(
            execute: parameter => UpdateModifiedCereal((CerealRowViewModel)parameter!),
            canExecute: parameter => parameter is CerealRowViewModel);

        CancelModifiedCerealCommand = new RelayCommand(
            execute: parameter => CancelModifiedCereal((CerealRowViewModel)parameter!),
            canExecute: parameter => parameter is CerealRowViewModel);

        DeleteCerealCommand = new RelayCommand(
            execute: parameter => DeleteCereal((CerealRowViewModel)parameter!),
            canExecute: parameter => parameter is CerealRowViewModel);
    }

    private async void LoadCereals()
    {
        if (AllCereals.Any(x => x.IsModified || x.IsNew) && !MessageBoxHelper.ConfirmLoadCereals())
            return;

        AllCereals.Clear();

        foreach (CerealData cereal in await _api.GetAllCereals())
        {
            AllCereals.Add(new CerealRowViewModel(cereal, await _api.GetCerealImage(cereal.Id)));
        }

        ApplyFilters();
    }

    private async void AddCereal()
    {
        var cereal = new CerealData();
        var row = new CerealRowViewModel(cereal, [], true);

        AllCereals.Add(row);
        ApplyFilters();
    }

    private async void SaveNewCereal(CerealRowViewModel row)
    {
        if (!MessageBoxHelper.ConfirmSaveNewCereal())
            return;

        try
        {
            await _api.CreateCereal(row.Cereal);

            row.SaveNew(AllCereals.Max(c => c.Id) + 1); // The new ID isn't calculated correctly if the newest entry in the database was deleted!
            ApplyFilters();
        }
        catch (HttpRequestException ex)
        {
            MessageBoxHelper.ShowError(ex.Message);
        }
    }

    private async void CancelNewCereal(CerealRowViewModel row)
    {
        if (row.IsModified && !MessageBoxHelper.ConfirmCancelNewCereal())
            return;

        AllCereals.Remove(row);
        ApplyFilters();
    }

    private async void UpdateModifiedCereal(CerealRowViewModel row)
    {
        if (!MessageBoxHelper.ConfirmUpdateChanges(row.Cereal.Name))
            return;

        try
        {
            await _api.UpdateCereal(row.Cereal);

            row.OverrideOriginal();
            ApplyFilters();
        }
        catch (HttpRequestException ex)
        {
            switch (ex.StatusCode)
            {
                case HttpStatusCode.Conflict:
                    MessageBoxHelper.ShowError($"A concurrent update has been detected for \"{row.Cereal.Name}\"\nReload the database and try again!", "Concurrent Update Conflict!");
                    break;

                default:
                    MessageBoxHelper.ShowError(ex.Message); 
                    break;
            }
        }
    }

    private async void CancelModifiedCereal(CerealRowViewModel row)
    {
        if (!MessageBoxHelper.ConfirmCancelChanges(row.Cereal.Name))
            return;

        row.RevertToOriginal();
        ApplyFilters();
    }

    private async void DeleteCereal(CerealRowViewModel row)
    {
        if (!MessageBoxHelper.ConfirmDelete(row.Cereal.Name))
            return;

        try
        {
            await _api.DeleteCereal(row.Cereal.Id);

            AllCereals.Remove(row);
            ApplyFilters();
        }
        catch (HttpRequestException ex)
        {
            MessageBoxHelper.ShowError(ex.Message);
        }
    }

    private async void FilterPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ColumnFilterViewModel.FilterValue) ||
            e.PropertyName == nameof(ColumnFilterViewModel.SelectedOperator))
        {
            _filterCancellation?.Cancel();
            _filterCancellation?.Dispose();

            _filterCancellation = new CancellationTokenSource();

            try
            {
                await Task.Delay(300, _filterCancellation.Token);
                ApplyFilters();
            }
            catch (TaskCanceledException)
            {
                // Ignore - another filter change occurred
            }
        }
    }

    private void ApplyFilters()
    {
        var activeFilters = Columns
            .Where(c => c.Filter.IsActive)
            .Select(c => c.Filter)
            .ToList();

        var results = AllCereals
            .Where(row =>
                activeFilters.All(filter =>
                    CerealFilter.Matches(row.Cereal, filter)))
            .ToList();

        FilteredCereals.Clear();

        foreach (var row in results)
        {
            FilteredCereals.Add(row);
        }
    }

    private void AuthenticationState_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AuthenticationState.IsLoggedIn))
        {
            OnPropertyChanged(nameof(IsLoggedIn));
        }
    }
}
