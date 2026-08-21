using Cereal_API.Shared.Models;
using Cereal_API.WPF.Commands;
using Cereal_API.WPF.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Cereal_API.WPF.ViewModels;

public class CerealDataTableViewModel : ViewModelBase
{
    private readonly CerealHttpService _api;
    //private readonly INavigationService _navigation;
    //private readonly LoginViewModel _loginVM;

    public RelayCommand LoadCerealsCommand {  get; private set; }
    public ObservableCollection<CerealData> Cereals { get; private set; } = [];

    public CerealDataTableViewModel(CerealHttpService api/*, INavigationService navigation*//*, LoginViewModel loginVM*/)
    {
        _api = api;
        //_navigation = navigation;
        //_loginVM = loginVM;

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
}
