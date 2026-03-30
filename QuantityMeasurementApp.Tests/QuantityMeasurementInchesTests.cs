// using System.Reflection;
// using QuantityMeasurementApp;

// namespace QuantityMeasurementApp.Tests;

// [TestClass]
// public sealed class QuantityMeasurementTests
// {
//     // UC2: Verifies that two Inches objects with the same value (1.0 ft and 1.0 ft)
//     // are considered equal using value-based equality.
//     [TestMethod]
//     public void TestEqualtity_SameValue()
//     {
//         Inches i1 = new Inches(1.0);
//         Inches i2 = new Inches(1.0);

//         bool result = i1.Equals(i2);

//         Assert.IsTrue(result, "1.0 ft should be equal to 1.0 ft");
//     }

//     // UC2: Verifies that two Inches objects with different values (1.0 ft and 2.0 ft)
//     // are not equal.
//     [TestMethod]
//     public void TestEquality_DifferentValue()
//     {
//         Inches i1 = new Inches(1.0);
//         Inches i2 = new Inches(2.0);

//         bool result = i1.Equals(i2);
//         Assert.IsFalse(result, "1.0 ft should not be equal to 2.0 ft");
//     }

//     // UC2: Verifies that a Inches object is not equal to null.
//     [TestMethod]
//     public void TestEquality_NullComparison()
//     {
//         Inches i1 = new Inches(1.0);

//         bool result = i1.Equals(null);
//         Assert.IsFalse(result, "Inches value should not be equal to null");
//     }

//     // UC2: Verifies that a Inches object is not equal to a non-numeric
//     [TestMethod]
//     public void TestEquality_NonNumericInput()
//     {
//         Inches i1 = new Inches(1);
//         object nonNumeric = "NotInteger";
//         bool result = i1.Equals(nonNumeric);

//         Assert.IsFalse(result, "Inches should not be equal to non-numeric input");
//     }

//     // UC2: Verifies reflexive property of equals() method,
//     // meaning an object must be equal to itself.
//     [TestMethod]
//     public void TestEquality_SameReference()
//     {
//         Inches i1 = new Inches(1.0);
//         bool result = i1.Equals(i1);

//         Assert.IsTrue(result, "Object should be equal to itself.");
//     }
// }