using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer;

namespace QuantityMeasurementApp.Tests;

[TestClass]
public class QuantityLengthUC8Tests
{
    private const double EPSILON = 1e-6;

    // -------------------------
    // LengthUnit enum tests
    // -------------------------

    [TestMethod]
    public void testLengthUnitEnum_FeetConstant()
    {
        double result = LengthUnit.Feet.ConvertToBaseUnit(1.0);
        Assert.AreEqual(1.0, result, EPSILON);
    }

    [TestMethod]
    public void testLengthUnitEnum_InchesConstant()
    {
        double result = LengthUnit.Inch.ConvertToBaseUnit(12.0);
        Assert.AreEqual(1.0, result, EPSILON);
    }

    [TestMethod]
    public void testLengthUnitEnum_YardsConstant()
    {
        double result = LengthUnit.Yard.ConvertToBaseUnit(1.0);
        Assert.AreEqual(3.0, result, EPSILON);
    }

    [TestMethod]
    public void testLengthUnitEnum_CentimetersConstant()
    {
        double result = LengthUnit.Centimeter.ConvertToBaseUnit(30.48);
        Assert.AreEqual(1.0, result, 1e-3);
    }

    // -------------------------
    // Convert TO base unit
    // -------------------------

    [TestMethod]
    public void testConvertToBaseUnit_FeetToFeet()
    {
        double result = LengthUnit.Feet.ConvertToBaseUnit(5.0);
        Assert.AreEqual(5.0, result, EPSILON);
    }

    [TestMethod]
    public void testConvertToBaseUnit_InchesToFeet()
    {
        double result = LengthUnit.Inch.ConvertToBaseUnit(12.0);
        Assert.AreEqual(1.0, result, EPSILON);
    }

    [TestMethod]
    public void testConvertToBaseUnit_YardsToFeet()
    {
        double result = LengthUnit.Yard.ConvertToBaseUnit(1.0);
        Assert.AreEqual(3.0, result, EPSILON);
    }

    [TestMethod]
    public void testConvertToBaseUnit_CentimetersToFeet()
    {
        double result = LengthUnit.Centimeter.ConvertToBaseUnit(30.48);
        Assert.AreEqual(1.0, result, 1e-3);
    }

    // -------------------------
    // Convert FROM base unit
    // -------------------------

    [TestMethod]
    public void testConvertFromBaseUnit_FeetToFeet()
    {
        double result = LengthUnit.Feet.ConvertFromBaseUnit(2.0);
        Assert.AreEqual(2.0, result, EPSILON);
    }

    [TestMethod]
    public void testConvertFromBaseUnit_FeetToInches()
    {
        double result = LengthUnit.Inch.ConvertFromBaseUnit(1.0);
        Assert.AreEqual(12.0, result, EPSILON);
    }

    [TestMethod]
    public void testConvertFromBaseUnit_FeetToYards()
    {
        double result = LengthUnit.Yard.ConvertFromBaseUnit(3.0);
        Assert.AreEqual(1.0, result, EPSILON);
    }

    [TestMethod]
    public void testConvertFromBaseUnit_FeetToCentimeters()
    {
        double result = LengthUnit.Centimeter.ConvertFromBaseUnit(1.0);
        Assert.AreEqual(30.48, result, 1e-2);
    }

    // -------------------------
    // QuantityLength tests
    // -------------------------

    [TestMethod]
    public void testQuantityLengthRefactored_Equality()
    {
        QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);
        QuantityLength q2 = new QuantityLength(12.0, LengthUnit.Inch);

        Assert.IsTrue(q1.Equals(q2));
    }

    [TestMethod]
    public void testQuantityLengthRefactored_ConvertTo()
    {
        QuantityLength q = new QuantityLength(1.0, LengthUnit.Feet);

        double result = q.ConvertTo(LengthUnit.Inch);

        Assert.AreEqual(12.0, result, EPSILON);
    }

    [TestMethod]
    public void testQuantityLengthRefactored_Add()
    {
        QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);
        QuantityLength q2 = new QuantityLength(12.0, LengthUnit.Inch);

        QuantityLength result = QuantityLength.Add(q1, q2, LengthUnit.Feet);

        QuantityLength expected = new QuantityLength(2.0, LengthUnit.Feet);

        Assert.IsTrue(result.Equals(expected));
    }

    [TestMethod]
    public void testQuantityLengthRefactored_AddWithTargetUnit()
    {
        QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);
        QuantityLength q2 = new QuantityLength(12.0, LengthUnit.Inch);

        QuantityLength result = QuantityLength.Add(q1, q2, LengthUnit.Yard);

        double expected = 0.6666667;

        Assert.AreEqual(expected, result.ConvertTo(LengthUnit.Yard), 1e-3);
    }
}