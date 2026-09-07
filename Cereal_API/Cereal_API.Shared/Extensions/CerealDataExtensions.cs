using Cereal_API.Shared.Models;

namespace Cereal_API.Shared.Extensions;

public static class CerealDataExtensions
{
    public static void Validate(this CerealData cereal)
    {
        if (string.IsNullOrWhiteSpace(cereal.Name))
            throw new ArgumentException("Cereal name is required.");
        
        if (!Enum.IsDefined(typeof(CerealEnums.Manufacturer), cereal.Manufacturer))
            throw new ArgumentException("Invalid cereal manufacturer.");

        if (!Enum.IsDefined(typeof(CerealEnums.Type), cereal.Type))
            throw new ArgumentException("Invalid cereal type.");

        if (cereal.Calories < 0 || 
            cereal.Protein < 0 || 
            cereal.Fat < 0 || 
            cereal.Sodium < 0 ||
            cereal.Fiber < 0 || 
            cereal.Carbo < 0 || 
            cereal.Sugars < 0 || 
            cereal.Potass < 0 ||
            cereal.Vitamins < 0 || 
            cereal.Shelf < 0 || 
            cereal.Weight < 0 || 
            cereal.Cups < 0 ||
            cereal.Rating < 0)
            throw new ArgumentException("Invalid cereal values.");
    }

    public static void SetValues(this CerealData original, CerealData newData)
    {
        original.Name = newData.Name;
        original.Manufacturer = newData.Manufacturer;
        original.Type = newData.Type;
        original.Calories = newData.Calories;
        original.Protein = newData.Protein;
        original.Fat = newData.Fat;
        original.Sodium = newData.Sodium;
        original.Fiber = newData.Fiber;
        original.Carbo = newData.Carbo;
        original.Sugars = newData.Sugars;
        original.Potass = newData.Potass;
        original.Vitamins = newData.Vitamins;
        original.Shelf = newData.Shelf;
        original.Weight = newData.Weight;
        original.Cups = newData.Cups;
        original.Rating = newData.Rating;
        original.DbVersion = newData.DbVersion;
    }

    public static CerealData Clone(this CerealData original)
    {
        return new CerealData
        {
            Id = original.Id,
            Name = original.Name,
            Manufacturer = original.Manufacturer,
            Type = original.Type,
            Calories = original.Calories,
            Protein = original.Protein,
            Fat = original.Fat,
            Sodium = original.Sodium,
            Fiber = original.Fiber,
            Carbo = original.Carbo,
            Sugars = original.Sugars,
            Potass = original.Potass,
            Vitamins = original.Vitamins,
            Shelf = original.Shelf,
            Weight = original.Weight,
            Cups = original.Cups,
            Rating = original.Rating,
            DbVersion = original.DbVersion,
        };
    }

    public static bool IsEqual(this CerealData original, CerealData other)
    {
        return
            original.Id == other.Id &&
            original.Name == other.Name &&
            original.Manufacturer == other.Manufacturer &&
            original.Type == other.Type &&
            original.Calories == other.Calories &&
            original.Protein == other.Protein &&
            original.Fat == other.Fat &&
            original.Sodium == other.Sodium &&
            original.Fiber == other.Fiber &&
            original.Carbo == other.Carbo &&
            original.Sugars == other.Sugars &&
            original.Potass == other.Potass &&
            original.Vitamins == other.Vitamins &&
            original.Shelf == other.Shelf &&
            original.Weight == other.Weight &&
            original.Cups == other.Cups &&
            original.Rating == other.Rating &&
            original.DbVersion == other.DbVersion;
    }
}
