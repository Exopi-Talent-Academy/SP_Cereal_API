using System.ComponentModel;

namespace Cereal_API.Shared;

public static class CerealEnums
{
    public enum Manufacturer
    {
        [Description("American Home Food Products")]
        A,
        [Description("General Mills")]
        G,
        [Description("Kelloggs")]
        K,
        [Description("Nabisco")]
        N,
        [Description("Post")]
        P,
        [Description("Quaker Oats")]
        Q,
        [Description("Ralston Purina")]
        R
    }

    public enum Type
    {
        [Description("Cold")]
        C,
        [Description("Hot")]
        H
    }

    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        var attribute = field?
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .Cast<DescriptionAttribute>()
            .FirstOrDefault();

        return attribute?.Description ?? value.ToString();
    }
}
