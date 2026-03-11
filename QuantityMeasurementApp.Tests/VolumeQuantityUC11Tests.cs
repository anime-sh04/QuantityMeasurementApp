using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer;

namespace QuantityMeasurementApp.Tests;

[TestClass]
public class VolumeQuantityUC11Tests
{
    private const double EPS = 1e-4;

    // ---------------------------
    // EQUALITY TESTS
    // ---------------------------

    [TestMethod]
    public void testEquality_LitreToLitre_SameValue()
    {
        var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
        var q2 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

        Assert.IsTrue(q1.Equals(q2));
    }

    [TestMethod]
    public void testEquality_LitreToLitre_DifferentValue()
    {
        var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
        var q2 = new Quantity<VolumeUnit>(2.0, VolumeUnit.Litre);

        Assert.IsFalse(q1.Equals(q2));
    }

    [TestMethod]
    public void testEquality_LitreToMillilitre_EquivalentValue()
    {
        var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
        var q2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

        Assert.IsTrue(q1.Equals(q2));
    }

    [TestMethod]
    public void testEquality_MillilitreToLitre_EquivalentValue()
    {
        var q1 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);
        var q2 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

        Assert.IsTrue(q1.Equals(q2));
    }

    [TestMethod]
    public void testEquality_LitreToGallon_EquivalentValue()
    {
        var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
        var q2 = new Quantity<VolumeUnit>(0.264172, VolumeUnit.Gallon);

        Assert.IsTrue(q1.Equals(q2));
    }

    [TestMethod]
    public void testEquality_GallonToLitre_EquivalentValue()
    {
        var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Gallon);
        var q2 = new Quantity<VolumeUnit>(3.78541, VolumeUnit.Litre);

        Assert.IsTrue(q1.Equals(q2));
    }

    [TestMethod]
    public void testEquality_VolumeVsLength_Incompatible()
    {
        var volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
        var length = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

        Assert.IsFalse(volume.Equals(length));
    }

    [TestMethod]
    public void testEquality_VolumeVsWeight_Incompatible()
    {
        var volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
        var weight = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

        Assert.IsFalse(volume.Equals(weight));
    }

    [TestMethod]
    public void testEquality_NullComparison()
    {
        var q = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

        Assert.IsFalse(q.Equals(null));
    }

    [TestMethod]
    public void testEquality_SameReference()
    {
        var q = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

        Assert.IsTrue(q.Equals(q));
    }

    // ---------------------------
    // CONVERSION TESTS
    // ---------------------------

    [TestMethod]
    public void testConversion_LitreToMillilitre()
    {
        var q = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

        var result = q.ConvertTo(VolumeUnit.Millilitre);

        Assert.AreEqual(1000.0, result.GetValue(), EPS);
    }

    [TestMethod]
    public void testConversion_MillilitreToLitre()
    {
        var q = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

        var result = q.ConvertTo(VolumeUnit.Litre);

        Assert.AreEqual(1.0, result.GetValue(), EPS);
    }

    [TestMethod]
    public void testConversion_GallonToLitre()
    {
        var q = new Quantity<VolumeUnit>(1.0, VolumeUnit.Gallon);

        var result = q.ConvertTo(VolumeUnit.Litre);

        Assert.AreEqual(3.78541, result.GetValue(), EPS);
    }

    [TestMethod]
    public void testConversion_LitreToGallon()
    {
        var q = new Quantity<VolumeUnit>(3.78541, VolumeUnit.Litre);

        var result = q.ConvertTo(VolumeUnit.Gallon);

        Assert.AreEqual(1.0, result.GetValue(), EPS);
    }

    [TestMethod]
    public void testConversion_RoundTrip()
    {
        var q = new Quantity<VolumeUnit>(1.5, VolumeUnit.Litre);

        var result = q.ConvertTo(VolumeUnit.Millilitre)
                      .ConvertTo(VolumeUnit.Litre);

        Assert.AreEqual(1.5, result.GetValue(), EPS);
    }

    // ---------------------------
    // ADDITION TESTS
    // ---------------------------

    [TestMethod]
    public void testAddition_SameUnit_LitrePlusLitre()
    {
        var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
        var q2 = new Quantity<VolumeUnit>(2.0, VolumeUnit.Litre);

        var result = q1.Add(q2);

        Assert.AreEqual(3.0, result.GetValue(), EPS);
    }

    [TestMethod]
    public void testAddition_CrossUnit_LitrePlusMillilitre()
    {
        var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
        var q2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

        var result = q1.Add(q2);

        Assert.AreEqual(2.0, result.GetValue(), EPS);
    }

    [TestMethod]
    public void testAddition_ExplicitTargetUnit_Millilitre()
    {
        var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
        var q2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

        var result = q1.Add(q2, VolumeUnit.Millilitre);

        Assert.AreEqual(2000.0, result.GetValue(), EPS);
    }

    [TestMethod]
    public void testAddition_WithZero()
    {
        var q1 = new Quantity<VolumeUnit>(5.0, VolumeUnit.Litre);
        var q2 = new Quantity<VolumeUnit>(0.0, VolumeUnit.Millilitre);

        var result = q1.Add(q2);

        Assert.AreEqual(5.0, result.GetValue(), EPS);
    }

    [TestMethod]
    public void testAddition_NegativeValues()
    {
        var q1 = new Quantity<VolumeUnit>(5.0, VolumeUnit.Litre);
        var q2 = new Quantity<VolumeUnit>(-2000.0, VolumeUnit.Millilitre);

        var result = q1.Add(q2);

        Assert.AreEqual(3.0, result.GetValue(), EPS);
    }

    // ---------------------------
    // ENUM TESTS
    // ---------------------------

    [TestMethod]
    public void testVolumeUnitEnum_LitreConstant()
    {
        Assert.AreEqual(1.0, VolumeUnit.Litre.GetConversionFactor(), EPS);
    }

    [TestMethod]
    public void testVolumeUnitEnum_MillilitreConstant()
    {
        Assert.AreEqual(0.001, VolumeUnit.Millilitre.GetConversionFactor(), EPS);
    }

    [TestMethod]
    public void testVolumeUnitEnum_GallonConstant()
    {
        Assert.AreEqual(3.78541, VolumeUnit.Gallon.GetConversionFactor(), EPS);
    }

    [TestMethod]
    public void testConvertToBaseUnit_MillilitreToLitre()
    {
        Assert.AreEqual(1.0, VolumeUnit.Millilitre.ConvertToBaseUnit(1000.0), EPS);
    }

    [TestMethod]
    public void testConvertFromBaseUnit_LitreToMillilitre()
    {
        Assert.AreEqual(1000.0, VolumeUnit.Millilitre.ConvertFromBaseUnit(1.0), EPS);
    }
}