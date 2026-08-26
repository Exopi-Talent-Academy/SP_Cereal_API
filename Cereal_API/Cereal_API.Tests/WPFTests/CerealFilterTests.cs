using Cereal_API.WPF.Filters;
using Cereal_API.Shared.Models;
using Cereal_API.WPF.ViewModels;
using Cereal_API.Shared;

namespace Cereal_API.Tests.WPFTests;

[TestClass]
public class CerealFilterTests
{
    [TestMethod]
    [DataRow(FilterOperator.Contains, "ppl", true)]
    [DataRow(FilterOperator.StartsWith, "App", true)]
    [DataRow(FilterOperator.EndsWith, "ple", true)]
    [DataRow(FilterOperator.Contains, "no", false)]
    [DataRow(FilterOperator.StartsWith, "no", false)]
    [DataRow(FilterOperator.EndsWith, "no", false)]
    public void Matches_NameOperations_RetrunsExpected(FilterOperator filterOperator, string filterValue, bool expected)
    {
        // Arrange
        var cereal = new CerealData { Name = "Apple" };
        var filter = new ColumnFilterViewModel("Name", typeof(string))
        {
            FilterValue = filterValue,
            SelectedOperator = filterOperator
        };

        // Act
        bool actual = CerealFilter.Matches(cereal, filter);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(5, FilterOperator.Equals, 5, true)]
    [DataRow(5, FilterOperator.NotEquals, 3, true)]
    [DataRow(5, FilterOperator.GreaterThan, 3, true)]
    [DataRow(5, FilterOperator.GreaterThanOrEqual, 5, true)]
    [DataRow(3, FilterOperator.LessThan, 5, true)]
    [DataRow(5, FilterOperator.LessThanOrEqual, 5, true)]
    [DataRow(3, FilterOperator.Equals, 5, false)]
    [DataRow(5, FilterOperator.NotEquals, 5, false)]
    [DataRow(5, FilterOperator.GreaterThan, 5, false)]
    [DataRow(4, FilterOperator.GreaterThanOrEqual, 5, false)]
    [DataRow(5, FilterOperator.LessThan, 5, false)]
    [DataRow(5, FilterOperator.LessThanOrEqual, 4, false)]
    public void Matches_NumberOperations_RetrunsExpected(int value, FilterOperator filterOperator, int filterValue, bool expected)
    {
        // Arrange
        var cereal = new CerealData { Calories = value };
        var filter = new ColumnFilterViewModel("Calories", typeof(string))
        {
            FilterValue = filterValue,
            SelectedOperator = filterOperator
        };

        // Act
        bool actual = CerealFilter.Matches(cereal, filter);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(FilterOperator.Equals, CerealEnums.Manufacturer.A, true)]
    [DataRow(FilterOperator.NotEquals, CerealEnums.Manufacturer.K, true)]
    [DataRow(FilterOperator.Equals, CerealEnums.Manufacturer.K, false)]
    [DataRow(FilterOperator.NotEquals, CerealEnums.Manufacturer.A, false)]
    public void Matches_EnumOperations_RetrunsExpected(FilterOperator filterOperator, CerealEnums.Manufacturer filterValue, bool expected)
    {
        // Arrange
        var cereal = new CerealData { Manufacturer = CerealEnums.Manufacturer.A };
        var filter = new ColumnFilterViewModel("Calories", typeof(CerealEnums.Manufacturer))
        {
            FilterValue = filterValue,
            SelectedOperator = filterOperator
        };

        // Act
        bool actual = CerealFilter.Matches(cereal, filter);

        // Assert
        Assert.AreEqual(expected, actual);
    }
}
