// // using Microsoft.VisualStudio.TestTools.UnitTesting;
// using QuantityMeasurementApp;

// namespace QuantityMeasurementApp.Tests;

// [TestClass]
// public class QuantityLengthUC4Tests
// {
//     // Verifies that Quantity(1.0, YARDS) equals Quantity(1.0, YARDS)
//     [TestMethod]
//     public void testEquality_YardToYard_SameValue()
//     {
//         var q1 = new QuantityLength(1.0, LengthUnit.Yard);
//         var q2 = new QuantityLength(1.0, LengthUnit.Yard);

//         Assert.IsTrue(q1.Equals(q2));
//     }

//     // Verifies that Quantity(1.0, YARDS) != Quantity(2.0, YARDS)
//     [TestMethod]
//     public void testEquality_YardToYard_DifferentValue()
//     {
//         var q1 = new QuantityLength(1.0, LengthUnit.Yard);
//         var q2 = new QuantityLength(2.0, LengthUnit.Yard);

//         Assert.IsFalse(q1.Equals(q2));
//     }

//     // 1 Yard = 3 Feet
//     [TestMethod]
//     public void testEquality_YardToFeet_EquivalentValue()
//     {
//         var yard = new QuantityLength(1.0, LengthUnit.Yard);
//         var feet = new QuantityLength(3.0, LengthUnit.Feet);

//         Assert.IsTrue(yard.Equals(feet));
//     }

//     // Symmetry: 3 Feet = 1 Yard
//     [TestMethod]
//     public void testEquality_FeetToYard_EquivalentValue()
//     {
//         var feet = new QuantityLength(3.0, LengthUnit.Feet);
//         var yard = new QuantityLength(1.0, LengthUnit.Yard);

//         Assert.IsTrue(feet.Equals(yard));
//     }

//     // 1 Yard = 36 Inches
//     [TestMethod]
//     public void testEquality_YardToInches_EquivalentValue()
//     {
//         var yard = new QuantityLength(1.0, LengthUnit.Yard);
//         var inches = new QuantityLength(36.0, LengthUnit.Inch);

//         Assert.IsTrue(yard.Equals(inches));
//     }

//     // Symmetry: 36 Inches = 1 Yard
//     [TestMethod]
//     public void testEquality_InchesToYard_EquivalentValue()
//     {
//         var inches = new QuantityLength(36.0, LengthUnit.Inch);
//         var yard = new QuantityLength(1.0, LengthUnit.Yard);

//         Assert.IsTrue(inches.Equals(yard));
//     }

//     // Non-equivalent conversion
//     [TestMethod]
//     public void testEquality_YardToFeet_NonEquivalentValue()
//     {
//         var yard = new QuantityLength(1.0, LengthUnit.Yard);
//         var feet = new QuantityLength(2.0, LengthUnit.Feet);

//         Assert.IsFalse(yard.Equals(feet));
//     }

//     // 1 cm = 0.393701 inches
//     [TestMethod]
//     public void testEquality_centimetersToInches_EquivalentValue()
//     {
//         var cm = new QuantityLength(1.0, LengthUnit.Centimeter);
//         var inches = new QuantityLength(0.393701, LengthUnit.Inch);

//         Assert.IsTrue(cm.Equals(inches));
//     }

//     // 1 cm != 1 foot
//     [TestMethod]
//     public void testEquality_centimetersToFeet_NonEquivalentValue()
//     {
//         var cm = new QuantityLength(1.0, LengthUnit.Centimeter);
//         var feet = new QuantityLength(1.0, LengthUnit.Feet);

//         Assert.IsFalse(cm.Equals(feet));
//     }

//     // Transitive Property: 1 Yard = 3 Feet = 36 Inches
//     [TestMethod]
//     public void testEquality_MultiUnit_TransitiveProperty()
//     {
//         var yard = new QuantityLength(1.0, LengthUnit.Yard);
//         var feet = new QuantityLength(3.0, LengthUnit.Feet);
//         var inches = new QuantityLength(36.0, LengthUnit.Inch);

//         Assert.IsTrue(yard.Equals(feet));
//         Assert.IsTrue(feet.Equals(inches));
//         Assert.IsTrue(yard.Equals(inches));
//     }

//     // Same reference (Reflexive property)
//     [TestMethod]
//     public void testEquality_YardSameReference()
//     {
//         var yard = new QuantityLength(2.0, LengthUnit.Yard);

//         Assert.IsTrue(yard.Equals(yard));
//     }

//     // Null comparison (Null Safety)
//     [TestMethod]
//     public void testEquality_YardNullComparison()
//     {
//         var yard = new QuantityLength(1.0, LengthUnit.Yard);

//         Assert.IsFalse(yard.Equals(null));
//     }

//     // Same reference for Centimeters
//     [TestMethod]
//     public void testEquality_CentimetersSameReference()
//     {
//         var cm = new QuantityLength(5.0, LengthUnit.Centimeter);

//         Assert.IsTrue(cm.Equals(cm));
//     }

//     // Null comparison for Centimeters
//     [TestMethod]
//     public void testEquality_CentimetersNullComparison()
//     {
//         var cm = new QuantityLength(2.0, LengthUnit.Centimeter);

//         Assert.IsFalse(cm.Equals(null));
//     }

//     // Complex scenario: 2 Yard = 6 Feet = 72 Inches
//     [TestMethod]
//     public void testEquality_AllUnits_ComplexScenario()
//     {
//         var yard = new QuantityLength(2.0, LengthUnit.Yard);
//         var feet = new QuantityLength(6.0, LengthUnit.Feet);
//         var inches = new QuantityLength(72.0, LengthUnit.Inch);

//         Assert.IsTrue(yard.Equals(feet));
//         Assert.IsTrue(feet.Equals(inches));
//         Assert.IsTrue(yard.Equals(inches));
//     }
// }