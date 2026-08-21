namespace Cereal_API.Shared;

public static class EnumProvider
{
    public static Array Manufacturers => Enum.GetValues(typeof(CerealEnums.Manufacturer));
    public static Array Types => Enum.GetValues(typeof(CerealEnums.Type));
}
