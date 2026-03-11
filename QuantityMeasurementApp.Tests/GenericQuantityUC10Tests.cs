using Microsoft.VisualStudio.TestTools.UnitTesting;

using QuantityMeasurementApp.ModelLayer;

namespace QuantityMeasurementApp.Tests;

[TestClass]
public class GenericQuantityUC10Tests
{
    private const double EPSILON = 1e-3;

    // -------------------------
    // Interface Implementation
    // -------------------------

    [TestMethod]
    public void testIMeasurableInterface_LengthUnitImplementation()
    {
        LengthUnit unit = LengthUnit.Feet;

        double baseValue = unit.ConvertToBaseUnit(1.0);

        Assert.AreEqual(1.0, baseValue, EPSILON);
    }

    [TestMethod]
    public void testIMeasurableInterface_WeightUnitImplementation()
    {
        WeightUnit unit = WeightUnit.Kilogram;

        double baseValue = unit.ConvertToBaseUnit(1.0);

        Assert.AreEqual(1.0, baseValue, EPSILON);
    }

    [TestMethod]
    public void testIMeasurableInterface_ConsistentBehavior()
    {
        LengthUnit l = LengthUnit.Feet;
        WeightUnit w = WeightUnit.Kilogram;

        Assert.IsNotNull(l.GetConversionFactor());
        Assert.IsNotNull(w.GetConversionFactor());
    }

    // -------------------------
    // Generic Quantity Equality
    // -------------------------

    [TestMethod]
    public void testGenericQuantity_LengthOperations_Equality()
    {
        var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
        var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

        Assert.IsTrue(q1.Equals(q2));
    }

    [TestMethod]
    public void testGenericQuantity_WeightOperations_Equality()
    {
        var q1 = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
        var q2 = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);

        Assert.IsTrue(q1.Equals(q2));
    }

    // -------------------------
    // Conversion
    // -------------------------

    [TestMethod]
    public void testGenericQuantity_LengthOperations_Conversion()
    {
        var q = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

        var result = q.ConvertTo(LengthUnit.Inch);

        Assert.AreEqual(12.0, result.GetValue(), EPSILON);
    }

    [TestMethod]
    public void testGenericQuantity_WeightOperations_Conversion()
    {
        var q = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

        var result = q.ConvertTo(WeightUnit.Gram);

        Assert.AreEqual(1000.0, result.GetValue(), EPSILON);
    }

    // -------------------------
    // Addition
    // -------------------------

    [TestMethod]
    public void testGenericQuantity_LengthOperations_Addition()
    {
        var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
        var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

        var result = q1.Add(q2, LengthUnit.Feet);

        Assert.AreEqual(2.0, result.GetValue(), EPSILON);
    }

    [TestMethod]
    public void testGenericQuantity_WeightOperations_Addition()
    {
        var q1 = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
        var q2 = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);

        var result = q1.Add(q2, WeightUnit.Kilogram);

        Assert.AreEqual(2.0, result.GetValue(), EPSILON);
    }

    // -------------------------
    // Cross Category Prevention
    // -------------------------

    [TestMethod]
    public void testCrossCategoryPrevention_LengthVsWeight()
    {
        var length = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
        var weight = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

        Assert.IsFalse(length.Equals(weight));
    }

    // -------------------------
    // Constructor Validation
    // -------------------------

    [TestMethod]
    public void testGenericQuantity_ConstructorValidation_InvalidValue()
    {
        Assert.ThrowsException<ArgumentException>(() =>
        {
            new Quantity<LengthUnit>(double.NaN, LengthUnit.Feet);
        });
    }

    // -------------------------
    // HashCode Consistency
    // -------------------------

    [TestMethod]
    public void testHashCode_GenericQuantity_Consistency()
    {
        var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
        var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

        Assert.AreEqual(q1.GetHashCode(), q2.GetHashCode());
    }

    // -------------------------
    // Equals Contract
    // -------------------------

    [TestMethod]
    public void testEquals_GenericQuantity_ContractPreservation()
    {
        var a = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
        var b = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);
        var c = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

        Assert.IsTrue(a.Equals(b));
        Assert.IsTrue(b.Equals(c));
        Assert.IsTrue(a.Equals(c));
    }

    // -------------------------
    // Immutability
    // -------------------------

    [TestMethod]
    public void testImmutability_GenericQuantity()
    {
        var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
        var q2 = q1.ConvertTo(LengthUnit.Inch);

        Assert.AreNotSame(q1, q2);
    }
}