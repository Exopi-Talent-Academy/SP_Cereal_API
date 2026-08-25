using Cereal_API.Shared;
using Cereal_API.Shared.Models;

namespace Cereal_API.WPF.ViewModels;

public class CerealRowViewModel : ViewModelBase
{
    public CerealData Cereal { get; }

    #region Cereal Data
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

    public bool IsNew { get; }

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
        }
    }

    public bool ShowSave => IsNew;
    public bool ShowUpdate => !IsNew && IsModified;

    public CerealRowViewModel(CerealData cereal, bool isNew = false)
    {
        Cereal = cereal;
        IsNew = isNew;
    }

    private void MakeModifiable()
    {
        IsModified = true;
        OnPropertyChanged(nameof(ShowUpdate));
    }
}
