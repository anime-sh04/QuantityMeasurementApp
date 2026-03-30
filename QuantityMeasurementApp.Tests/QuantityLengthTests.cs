// using System.Reflection;
// using QuantityMeasurementApp;

// namespace QuantityMeasurementApp.Tests;

// [TestClass]
// public class QuantityLengthTests
// {
//     // Feet to Feet (Same Value)
//     [TestMethod]
//     public void TestEquality_FeetToFeet_SameValue()
//     {
//         var q1 = new QuantityLength(1.0, LengthUnit.Feet);
//         var q2 = new QuantityLength(1.0, LengthUnit.Feet);

//         Assert.IsTrue(q1.Equals(q2));
//     }

//     // Inch to Inch (Same Value)
//     [TestMethod]
//     public void TestEquality_InchToInch_SameValue()
//     {
//         var q1 = new QuantityLength(1.0, LengthUnit.Inch);
//         var q2 = new QuantityLength(1.0, LengthUnit.Inch);

//         Assert.IsTrue(q1.Equals(q2));
//     }

//     // Cross-Unit Equality (1 ft = 12 inch)
//     [TestMethod]
//     public void TestEquality_FeetToInch_EquivalentValue()
//     {
//         var q1 = new QuantityLength(1.0, LengthUnit.Feet);
//         var q2 = new QuantityLength(12.0, LengthUnit.Inch);

//         Assert.IsTrue(q1.Equals(q2));
//     }

//     // Symmetry check (12 inch = 1 ft)
//     [TestMethod]
//     public void TestEquality_InchToFeet_EquivalentValue()
//     {
//         var q1 = new QuantityLength(12.0, LengthUnit.Inch);
//         var q2 = new QuantityLength(1.0, LengthUnit.Feet);

//         Assert.IsTrue(q1.Equals(q2));
//     }

//     // Different Values
//     [TestMethod]
//     public void TestEquality_FeetToFeet_DifferentValue()
//     {
//         var q1 = new QuantityLength(1.0, LengthUnit.Feet);
//         var q2 = new QuantityLength(2.0, LengthUnit.Feet);

//         Assert.IsFalse(q1.Equals(q2));
//     }

//     // Null comparison (Null Safety)
//     [TestMethod]
//     public void TestEquality_NullComparison()
//     {
//         var q1 = new QuantityLength(1.0, LengthUnit.Feet);

//         Assert.IsFalse(q1.Equals(null));
//     }

//     // Same reference (Reflexive property)
//     [TestMethod]
//     public void TestEquality_SameReference()
//     {
//         var q1 = new QuantityLength(1.0, LengthUnit.Feet);

//         Assert.IsTrue(q1.Equals(q1));
//     }
// }