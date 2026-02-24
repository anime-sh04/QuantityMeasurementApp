using System.Reflection;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests;

[TestClass]
public sealed class QuantityMeasurementInchesTests
{
    // UC1: Verifies that two Feet objects with the same value (1.0 ft and 1.0 ft)
    // are considered equal using value-based equality.
    [TestMethod]
    public void TestEqualtity_SameValue()
    {
        Feet f1 = new Feet(1.0);
        Feet f2 = new Feet(1.0);

        bool result = f1.Equals(f2);

        Assert.IsTrue(result, "1.0 ft should be equal to 1.0 ft");
    }

    // UC1: Verifies that two Feet objects with different values (1.0 ft and 2.0 ft)
    // are not equal.
    [TestMethod]
    public void TestEquality_DifferentValue()
    {
        Feet f1 = new Feet(1.0);
        Feet f2 = new Feet(2.0);

        bool result = f1.Equals(f2);
        Assert.IsFalse(result, "1.0 ft should not be equal to 2.0 ft");
    }

    // UC1: Verifies that a Feet object is not equal to null.
    [TestMethod]
    public void TestEquality_NullComparison()
    {
        Feet f1 = new Feet(1.0);

        bool result = f1.Equals(null);
        Assert.IsFalse(result, "Feet value should not be equal to null");
    }

    // UC1: Verifies that a Feet object is not equal to a non-numeric
    [TestMethod]
    public void TestEquality_NonNumericInput()
    {
        Feet f1 = new Feet(1);
        object nonNumeric = "NotInteger";
        bool result = f1.Equals(nonNumeric);

        Assert.IsFalse(result, "Feet should not be equal to non-numeric input");
    }

    // UC1: Verifies reflexive property of equals() method,
    // meaning an object must be equal to itself.
    [TestMethod]
    public void TestEquality_SameReference()
    {
        Feet f1 = new Feet(1.0);
        bool result = f1.Equals(f1);

        Assert.IsTrue(result, "Object should be equal to itself.");
    }
}