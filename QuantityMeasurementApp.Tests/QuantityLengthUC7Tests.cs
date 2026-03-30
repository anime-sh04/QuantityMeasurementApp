// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using QuantityMeasurementApp.ModelLayer;

// namespace QuantityMeasurementApp.Tests;

// [TestClass]
// public class QuantityLengthUC7Tests
// {
//     private const double EPSILON = 1e-3;

//     [TestMethod]
//     public void testAddition_ExplicitTargetUnit_Feet()
//     {
//         var a = new QuantityLength(1.0, LengthUnit.Feet);
//         var b = new QuantityLength(12.0, LengthUnit.Inch);

//         var result = QuantityLength.Add(a, b, LengthUnit.Feet);

//         Assert.AreEqual(new QuantityLength(2.0, LengthUnit.Feet), result);
//     }

//     [TestMethod]
//     public void testAddition_ExplicitTargetUnit_Inches()
//     {
//         var a = new QuantityLength(1.0, LengthUnit.Feet);
//         var b = new QuantityLength(12.0, LengthUnit.Inch);

//         var result = QuantityLength.Add(a, b, LengthUnit.Inch);

//         Assert.AreEqual(new QuantityLength(24.0, LengthUnit.Inch), result);
//     }

//     [TestMethod]
//     public void testAddition_ExplicitTargetUnit_Yards()
//     {
//         var a = new QuantityLength(1.0, LengthUnit.Feet);
//         var b = new QuantityLength(12.0, LengthUnit.Inch);

//         var result = QuantityLength.Add(a, b, LengthUnit.Yard);
//         double expected = 0.6666667;    

//         Assert.AreEqual(expected, result.ConvertTo(LengthUnit.Yard), 1e-3);
//     }

//     [TestMethod]
//     public void testAddition_ExplicitTargetUnit_Centimeters()
//     {
//         var a = new QuantityLength(1.0, LengthUnit.Inch);
//         var b = new QuantityLength(1.0, LengthUnit.Inch);

//         var result = QuantityLength.Add(a, b, LengthUnit.Centimeter);

//         var expected = new QuantityLength(5.08, LengthUnit.Centimeter);

//         Assert.IsTrue(result.Equals(expected));
//     }

//     [TestMethod]
//     public void testAddition_ExplicitTargetUnit_SameAsFirstOperand()
//     {
//         var a = new QuantityLength(2.0, LengthUnit.Yard);
//         var b = new QuantityLength(3.0, LengthUnit.Feet);

//         var result = QuantityLength.Add(a, b, LengthUnit.Yard);

//         Assert.AreEqual(new QuantityLength(3.0, LengthUnit.Yard), result);
//     }

//     [TestMethod]
//     public void testAddition_ExplicitTargetUnit_SameAsSecondOperand()
//     {
//         var a = new QuantityLength(2.0, LengthUnit.Yard);
//         var b = new QuantityLength(3.0, LengthUnit.Feet);

//         var result = QuantityLength.Add(a, b, LengthUnit.Feet);

//         Assert.AreEqual(new QuantityLength(9.0, LengthUnit.Feet), result);
//     }

//     [TestMethod]
//     public void testAddition_ExplicitTargetUnit_Commutativity()
//     {
//         var a = new QuantityLength(1.0, LengthUnit.Feet);
//         var b = new QuantityLength(12.0, LengthUnit.Inch);

//         var result1 = QuantityLength.Add(a, b, LengthUnit.Yard);
//         var result2 = QuantityLength.Add(b, a, LengthUnit.Yard);

//         Assert.AreEqual(result1, result2);
//     }

//     [TestMethod]
//     public void testAddition_ExplicitTargetUnit_WithZero()
//     {
//         var a = new QuantityLength(5.0, LengthUnit.Feet);
//         var b = new QuantityLength(0.0, LengthUnit.Inch);

//         var result = QuantityLength.Add(a, b, LengthUnit.Yard);

//         double expected = 1.6666667;

//         Assert.AreEqual(expected, result.ConvertTo(LengthUnit.Yard), 1e-3);
//     }

//     [TestMethod]
//     public void testAddition_ExplicitTargetUnit_NegativeValues()
//     {
//         var a = new QuantityLength(5.0, LengthUnit.Feet);
//         var b = new QuantityLength(-2.0, LengthUnit.Feet);

//         var result = QuantityLength.Add(a, b, LengthUnit.Inch);

//         Assert.AreEqual(new QuantityLength(36.0, LengthUnit.Inch), result);
//     }

//     [TestMethod]
//     public void testAddition_ExplicitTargetUnit_NullTargetUnit()
//     {
//         try
//         {
//             var a = new QuantityLength(1.0, LengthUnit.Feet);
//             var b = new QuantityLength(12.0, LengthUnit.Inch);

//             QuantityLength.Add(a, b, (LengthUnit)(-1));

//             Assert.Fail("Expected exception not thrown.");
//         }
//         catch (Exception)
//         {
//             Assert.IsTrue(true);
//         }
//     }

//     [TestMethod]
//     public void testAddition_ExplicitTargetUnit_LargeToSmallScale()
//     {
//         var a = new QuantityLength(1000.0, LengthUnit.Feet);
//         var b = new QuantityLength(500.0, LengthUnit.Feet);

//         var result = QuantityLength.Add(a, b, LengthUnit.Inch);

//         Assert.AreEqual(new QuantityLength(18000.0, LengthUnit.Inch), result);
//     }

//     [TestMethod]
//     public void testAddition_ExplicitTargetUnit_SmallToLargeScale()
//     {
//         var a = new QuantityLength(12.0, LengthUnit.Inch);
//         var b = new QuantityLength(12.0, LengthUnit.Inch);

//         var result = QuantityLength.Add(a, b, LengthUnit.Yard);

//         double expected = 0.6666667;

//         Assert.AreEqual(expected, result.ConvertTo(LengthUnit.Yard), 1e-3);

//     }
// }