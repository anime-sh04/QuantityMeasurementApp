using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer;

namespace QuantityMeasurementApp.Tests;

[TestClass]
public class QuantityLengthConversionTests
{
    private const double EPSILON = 1e-6;

    [TestMethod]
    public void testConversion_FeetToInches()
    {
        double result = QuantityLength.Convert(1.0, LengthUnit.Feet, LengthUnit.Inch);
        Assert.AreEqual(12.0, result, EPSILON);
    }

    [TestMethod]
    public void testConversion_InchesToFeet()
    {
        double result = QuantityLength.Convert(24.0, LengthUnit.Inch, LengthUnit.Feet);
        Assert.AreEqual(2.0, result, EPSILON);
    }

    [TestMethod]
    public void testConversion_YardsToInches()
    {
        double result = QuantityLength.Convert(1.0, LengthUnit.Yard, LengthUnit.Inch);
        Assert.AreEqual(36.0, result, EPSILON);
    }

    [TestMethod]
    public void testConversion_InchesToYards()
    {
        double result = QuantityLength.Convert(72.0, LengthUnit.Inch, LengthUnit.Yard);
        Assert.AreEqual(2.0, result, EPSILON);
    }

    [TestMethod]
    public void testConversion_CentimetersToInches()
    {
        double result = QuantityLength.Convert(2.54, LengthUnit.Centimeter, LengthUnit.Inch);
        Assert.AreEqual(1.0, result, 1e-3);
    }

    [TestMethod]
    public void testConversion_FeetToYard()
    {
        double result = QuantityLength.Convert(6.0, LengthUnit.Feet, LengthUnit.Yard);
        Assert.AreEqual(2.0, result, EPSILON);
    }

    [TestMethod]
    public void testConversion_RoundTrip_PreservesValue()
    {
        double original = 5.0;

        double converted = QuantityLength.Convert(original, LengthUnit.Feet, LengthUnit.Inch);
        double roundTrip = QuantityLength.Convert(converted, LengthUnit.Inch, LengthUnit.Feet);

        Assert.AreEqual(original, roundTrip, EPSILON);
    }

    [TestMethod]
    public void testConversion_ZeroValue()
    {
        double result = QuantityLength.Convert(0.0, LengthUnit.Feet, LengthUnit.Inch);
        Assert.AreEqual(0.0, result, EPSILON);
    }

    [TestMethod]
    public void testConversion_NegativeValue()
    {
        double result = QuantityLength.Convert(-1.0, LengthUnit.Feet, LengthUnit.Inch);
        Assert.AreEqual(-12.0, result, EPSILON);
    }

    [TestMethod]
    public void testConversion_NaNOrInfinite_Throws()
    {
        try
        {
            QuantityLength.Convert(double.NaN, LengthUnit.Feet, LengthUnit.Inch);

            // If no exception occurs → test fails
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
            // Test passes
            Assert.IsTrue(true);
        }
    }
}