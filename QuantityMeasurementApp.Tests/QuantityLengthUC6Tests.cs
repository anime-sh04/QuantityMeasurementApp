using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer;

namespace QuantityMeasurementApp.Tests;

[TestClass]
public class QuantityLengthUC6Tests
{
    private const double EPSILON = 1e-6;

    [TestMethod]
    public void testAddition_SameUnit_FeetPlusFeet()
    {
        var a = new QuantityLength(1.0, LengthUnit.Feet);
        var b = new QuantityLength(2.0, LengthUnit.Feet);

        var result = QuantityLength.Add(a,b);

        Assert.AreEqual(new QuantityLength(3.0, LengthUnit.Feet), result);
    }

    [TestMethod]
    public void testAddition_SameUnit_InchPlusInch()
    {
        var a = new QuantityLength(6.0, LengthUnit.Inch);
        var b = new QuantityLength(6.0, LengthUnit.Inch);

        var result = QuantityLength.Add(a,b);

        Assert.AreEqual(new QuantityLength(12.0, LengthUnit.Inch), result);
    }

    [TestMethod]
    public void testAddition_CrossUnit_FeetPlusInches()
    {
        var a = new QuantityLength(1.0, LengthUnit.Feet);
        var b = new QuantityLength(12.0, LengthUnit.Inch);

        var result = QuantityLength.Add(a,b);

        Assert.AreEqual(new QuantityLength(2.0, LengthUnit.Feet), result);
    }

    [TestMethod]
    public void testAddition_CrossUnit_InchPlusFeet()
    {
        var a = new QuantityLength(12.0, LengthUnit.Inch);
        var b = new QuantityLength(1.0, LengthUnit.Feet);

        var result = QuantityLength.Add(a,b);

        Assert.AreEqual(new QuantityLength(24.0, LengthUnit.Inch), result);
    }

    [TestMethod]
    public void testAddition_CrossUnit_YardPlusFeet()
    {
        var a = new QuantityLength(1.0, LengthUnit.Yard);
        var b = new QuantityLength(3.0, LengthUnit.Feet);

        var result = QuantityLength.Add(a,b);

        Assert.AreEqual(new QuantityLength(2.0, LengthUnit.Yard), result);
    }

    [TestMethod]
    public void testAddition_CrossUnit_CentimeterPlusInch()
    {
        var a = new QuantityLength(2.54, LengthUnit.Centimeter);
        var b = new QuantityLength(1.0, LengthUnit.Inch);

        var result = QuantityLength.Add(a,b);

        var expected = new QuantityLength(5.08, LengthUnit.Centimeter);

        Assert.IsTrue(result.Equals(expected));
    }

    [TestMethod]
    public void testAddition_Commutativity()
    {
        var a = new QuantityLength(1.0, LengthUnit.Feet);
        var b = new QuantityLength(12.0, LengthUnit.Inch);

        var result1 = QuantityLength.Add(a,b);
        var result2 = QuantityLength.Add(b,a);

        Assert.AreEqual(result1, result2);
    }

    [TestMethod]
    public void testAddition_WithZero()
    {
        var a = new QuantityLength(5.0, LengthUnit.Feet);
        var b = new QuantityLength(0.0, LengthUnit.Inch);

        var result = QuantityLength.Add(a,b);

        Assert.AreEqual(new QuantityLength(5.0, LengthUnit.Feet), result);
    }

    [TestMethod]
    public void testAddition_NegativeValues()
    {
        var a = new QuantityLength(5.0, LengthUnit.Feet);
        var b = new QuantityLength(-2.0, LengthUnit.Feet);

        var result = QuantityLength.Add(a,b);

        Assert.AreEqual(new QuantityLength(3.0, LengthUnit.Feet), result);
    }

    [TestMethod]
    public void testAddition_NullSecondOperand()
    {
        try
        {
            var a = new QuantityLength(1.0, LengthUnit.Feet);

            QuantityLength result = QuantityLength.Add(a,null);

            Assert.Fail("Expected exception was not thrown.");
        }
        catch (ArgumentException)
        {
            Assert.IsTrue(true);
        }
    }

    [TestMethod]
    public void testAddition_LargeValues()
    {
        var a = new QuantityLength(1e6, LengthUnit.Feet);
        var b = new QuantityLength(1e6, LengthUnit.Feet);

        var result = QuantityLength.Add(a,b);

        Assert.AreEqual(new QuantityLength(2e6, LengthUnit.Feet), result);
    }

    [TestMethod]
    public void testAddition_SmallValues()
    {
        var a = new QuantityLength(0.001, LengthUnit.Feet);
        var b = new QuantityLength(0.002, LengthUnit.Feet);

        var result = QuantityLength.Add(a,b);

        var expected = new QuantityLength(0.003, LengthUnit.Feet);

        Assert.IsTrue(result.Equals(expected));
    }
}