namespace Cereal_API.Shared.Models;

public class CerealData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CerealEnums.Manufacturer Manufacturer { get; set; }
    public CerealEnums.Type Type { get; set; }
    public int Calories { get; set; }
    public int Protein { get; set; }
    public int Fat { get; set; }
    public int Sodium { get; set; }
    public float Fiber { get; set; }
    public float Carbo { get; set; }
    public int Sugars { get; set; }
    public int Potass { get; set; }
    public int Vitamins { get; set; }
    public int Shelf { get; set; }
    public float Weight { get; set; }
    public float Cups { get; set; }
    public float Rating { get; set; }
    public DateTime? DbVersion { get; set; } = null;
    public bool MarkedAsDeleted { get; set; } = false;
    public List<string> ChangeHistory { get; set; } = [];
}
