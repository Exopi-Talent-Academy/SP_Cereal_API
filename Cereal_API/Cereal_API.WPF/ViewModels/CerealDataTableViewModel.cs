using Cereal_API.Shared;
using Cereal_API.Shared.Models;
using Cereal_API.WPF.Commands;
using Cereal_API.WPF.Filters;
using Cereal_API.WPF.Services;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cereal_API.WPF.ViewModels;

public class CerealDataTableViewModel : ViewModelBase
{
    private readonly CerealHttpService _api;
    //private readonly LoginViewModel _loginVM;

    public RelayCommand LoadCerealsCommand {  get; private set; }
    public ObservableCollection<CerealData> Cereals { get; private set; } = [];
    public ObservableCollection<CerealData> FilteredCereals { get; private set; } = [];
    public ObservableCollection<FilterRule> Filters { get; private set; } = []; // Figure out how to assing this!

    public CerealEnums.Manufacturer[] ManufacturerOptions { get; private set; }

    public CerealDataTableViewModel(CerealHttpService api/*, LoginViewModel loginVM*/)
    {
        _api = api;
        //_loginVM = loginVM;

        ManufacturerOptions = (CerealEnums.Manufacturer[])Enum.GetValues(typeof(CerealEnums.Manufacturer));

        LoadCerealsCommand = new RelayCommand(
            execute: _ => _ = LoadCereals(),
            canExecute: _ => true);
    }

    public async Task LoadCereals()
    {
        Cereals.Clear();

        foreach (CerealData cereal in await _api.GetAllCereals())
        {
            Cereals.Add(cereal);
        }
    }

    /*public void OnFilterChanged()
    {
        FilteredCereals.Clear();

        foreach (var cereal in Cereals)
        {
            if (MatchesAllFilters(cereal))
                FilteredCereals.Add(cereal);
        }
    }*/

    /*public void MatchesAllFilters(CerealData data)
    {
        var value = typeof(CerealData)
    .GetProperty(filter.PropertyName)!
    .GetValue(cereal);
    }*/
}
