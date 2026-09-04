using Cereal_API.Shared;
using Cereal_API.Shared.Extensions;
using Cereal_API.Shared.Models;
using System.ComponentModel;

namespace Cereal_API.WPF.ViewModels;

public class CerealRowViewModel : ViewModelBase
{
    private readonly AuthenticationState _authenticationState;

    public CerealData Cereal { get; }
    private CerealData Original { get; }

    #region Cereal Data
    public int Id
    {
        get => Cereal.Id;
        set
        {
            if (Cereal.Id == value)
                return;

            Cereal.Id = value;

            OnPropertyChanged();
        }
    }

    public string Name
    {
        get => Cereal.Name;
        set
        {
            if (Cereal.Name == value)
                return;

            Cereal.Name = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }

    public CerealEnums.Manufacturer Manufacturer
    {
        get => Cereal.Manufacturer;
        set
        {
            if (Cereal.Manufacturer == value)
                return;

            Cereal.Manufacturer = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }

    public CerealEnums.Type Type
    {
        get => Cereal.Type;
        set
        {
            if (Cereal.Type == value)
                return;

            Cereal.Type = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }

    public int Calories
    {
        get => Cereal.Calories;
        set
        {
            if (Cereal.Calories == value)
                return;

            Cereal.Calories = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }

    public int Protein
    {
        get => Cereal.Protein;
        set
        {
            if (Cereal.Protein == value)
                return;

            Cereal.Protein = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }

    public int Fat
    {
        get => Cereal.Fat;
        set
        {
            if (Cereal.Fat == value)
                return;

            Cereal.Fat = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }

    public int Sodium
    {
        get => Cereal.Sodium;
        set
        {
            if (Cereal.Sodium == value)
                return;

            Cereal.Sodium = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }

    public float Fiber
    {
        get => Cereal.Fiber;
        set
        {
            if (Cereal.Fiber == value)
                return;

            Cereal.Fiber = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }

    public float Carbo
    {
        get => Cereal.Carbo;
        set
        {
            if (Cereal.Carbo == value)
                return;

            Cereal.Carbo = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }

    public int Sugars
    {
        get => Cereal.Sugars;
        set
        {
            if (Cereal.Sugars == value)
                return;

            Cereal.Sugars = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }

    public int Potass
    {
        get => Cereal.Potass;
        set
        {
            if (Cereal.Potass == value)
                return;

            Cereal.Potass = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }

    public int Vitamins
    {
        get => Cereal.Vitamins;
        set
        {
            if (Cereal.Vitamins == value)
                return;

            Cereal.Vitamins = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }

    public int Shelf
    {
        get => Cereal.Shelf;
        set
        {
            if (Cereal.Shelf == value)
                return;

            Cereal.Shelf = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }

    public float Weight
    {
        get => Cereal.Weight;
        set
        {
            if (Cereal.Weight == value)
                return;

            Cereal.Weight = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }

    public float Cups
    {
        get => Cereal.Cups;
        set
        {
            if (Cereal.Cups == value)
                return;

            Cereal.Cups = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }

    public float Rating
    {
        get => Cereal.Rating;
        set
        {
            if (Cereal.Rating == value)
                return;

            Cereal.Rating = value;

            OnPropertyChanged();
            MakeModifiable();
        }
    }
    #endregion

    public byte[] Image { get; private set; }

    public bool IsNew { get; set; }

    private bool _isModified;

    public bool IsModified
    {
        get => _isModified;
        set
        {
            if (_isModified == value)
                return;

            _isModified = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ShowUpdate));
            OnPropertyChanged(nameof(ShowDelete));
        }
    }

    public bool ShowSave => IsNew && _authenticationState.RoleHasEditPermission;
    public bool ShowUpdate => !IsNew && IsModified && _authenticationState.RoleHasEditPermission;
    public bool ShowDelete => !IsNew && !IsModified && _authenticationState.RoleHasDeletePermission;

    public CerealRowViewModel(AuthenticationState authenticationState, CerealData cereal, byte[] image, bool isNew = false)
    {
        _authenticationState = authenticationState;
        Cereal = cereal;
        Image = image;
        IsNew = isNew;

        Original = isNew ? new() : cereal.Clone();

        _authenticationState.PropertyChanged += AuthenticationState_PropertyChanged;
    }

    public void SaveNew(int newID)
    {
        Id = newID;
        IsNew = false;

        OnPropertyChanged(nameof(ShowSave));
        OverrideOriginal();
    }

    public void RevertToOriginal()
    {
        Cereal.SetValues(Original);
        IsModified = false;
    }

    public void OverrideOriginal()
    {
        Original.SetValues(Cereal);
        IsModified = false;
    }

    private void MakeModifiable()
    {
        IsModified = !Cereal.IsEqual(Original);
    }

    private void AuthenticationState_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AuthenticationState.IsLoggedIn))
        {
            OnPropertyChanged(nameof(ShowSave));
            OnPropertyChanged(nameof(ShowUpdate));
            OnPropertyChanged(nameof(ShowDelete));
        }
    }
}
