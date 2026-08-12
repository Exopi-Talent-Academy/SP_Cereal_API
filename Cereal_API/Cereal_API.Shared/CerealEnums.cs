namespace Cereal_API.Shared;

public static class CerealEnums
{
    public enum Manufacturer
    {
        A,
        G,
        K,
        N,
        P,
        Q,
        R
    }

    public static string GetManuFacturerName(Manufacturer mfr)
    {
        return mfr switch
        {
            Manufacturer.A => "American Home Food Products",
            Manufacturer.G => "General Mills",
            Manufacturer.K => "Kelloggs",
            Manufacturer.N => "Nabisco",
            Manufacturer.P => "Post",
            Manufacturer.Q => "Quaker Oats",
            Manufacturer.R => "Ralston Purina",
            _ => throw new Exception("Cereal Manufacturer not found!")
        };
    }

    public enum Type
    {
        C,
        H
    }

    public static string GetTypeName(Type type)
    {
        return type switch
        {
            Type.C => "Cold",
            Type.H => "Hot",
            _ => throw new Exception("Cereal type not found!")
        };
    }
}
