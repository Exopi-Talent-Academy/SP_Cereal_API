using Cereal_API.Shared.Models;
using System.ComponentModel;

namespace Cereal_API.WPF.ViewModels;

public class CerealDataRowViewModel(CerealData data) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public CerealData Data { get; private set; } = data;
}
