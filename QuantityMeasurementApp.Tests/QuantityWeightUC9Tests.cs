// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using QuantityMeasurementApp.ModelLayer;

// namespace QuantityMeasurementApp.Tests;

// [TestClass]
// public class QuantityWeightUC9Tests
// {
//     private const double EPSILON = 1e-3;

//     // ------------------------
//     // Equality Tests
//     // ------------------------

//     [TestMethod]
//     public void testEquality_KilogramToKilogram_SameValue()
//     {
//         var w1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
//         var w2 = new QuantityWeight(1.0, WeightUnit.Kilogram);

//         Assert.IsTrue(w1.Equals(w2));
//     }

//     [TestMethod]
//     public void testEquality_KilogramToKilogram_DifferentValue()
//     {
//         var w1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
//         var w2 = new QuantityWeight(2.0, WeightUnit.Kilogram);

//         Assert.IsFalse(w1.Equals(w2));
//     }

//     [TestMethod]
//     public void testEquality_KilogramToGram_EquivalentValue()
//     {
//         var w1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
//         var w2 = new QuantityWeight(1000.0, WeightUnit.Gram);

//         Assert.IsTrue(w1.Equals(w2));
//     }

//     [TestMethod]
//     public void testEquality_GramToKilogram_EquivalentValue()
//     {
//         var w1 = new QuantityWeight(1000.0, WeightUnit.Gram);
//         var w2 = new QuantityWeight(1.0, WeightUnit.Kilogram);

//         Assert.IsTrue(w1.Equals(w2));
//     }

//     [TestMethod]
//     public void testEquality_WeightVsLength_Incompatible()
//     {
//         var w = new QuantityWeight(1.0, WeightUnit.Kilogram);
//         var l = new QuantityLength(1.0, LengthUnit.Feet);

//         Assert.IsFalse(w.Equals(l));
//     }

//     [TestMethod]
//     public void testEquality_NullComparison()
//     {
//         var w = new QuantityWeight(1.0, WeightUnit.Kilogram);

//         Assert.IsFalse(w.Equals(null));
//     }

//     [TestMethod]
//     public void testEquality_SameReference()
//     {
//         var w = new QuantityWeight(1.0, WeightUnit.Kilogram);

//         Assert.IsTrue(w.Equals(w));
//     }

//     [TestMethod]
//     public void testEquality_ZeroValue()
//     {
//         var w1 = new QuantityWeight(0.0, WeightUnit.Kilogram);
//         var w2 = new QuantityWeight(0.0, WeightUnit.Gram);

//         Assert.IsTrue(w1.Equals(w2));
//     }

//     [TestMethod]
//     public void testEquality_NegativeWeight()
//     {
//         var w1 = new QuantityWeight(-1.0, WeightUnit.Kilogram);
//         var w2 = new QuantityWeight(-1000.0, WeightUnit.Gram);

//         Assert.IsTrue(w1.Equals(w2));
//     }

//     [TestMethod]
//     public void testEquality_LargeWeightValue()
//     {
//         var w1 = new QuantityWeight(1000000.0, WeightUnit.Gram);
//         var w2 = new QuantityWeight(1000.0, WeightUnit.Kilogram);

//         Assert.IsTrue(w1.Equals(w2));
//     }

//     [TestMethod]
//     public void testEquality_SmallWeightValue()
//     {
//         var w1 = new QuantityWeight(0.001, WeightUnit.Kilogram);
//         var w2 = new QuantityWeight(1.0, WeightUnit.Gram);

//         Assert.IsTrue(w1.Equals(w2));
//     }

//     // ------------------------
//     // Conversion Tests
//     // ------------------------

//     [TestMethod]
//     public void testConversion_PoundToKilogram()
//     {
//         var w = new QuantityWeight(2.20462, WeightUnit.Pound);

//         var result = w.ConvertTo(WeightUnit.Kilogram);

//         Assert.AreEqual(1.0, result.GetValue(), EPSILON);
//     }

//     [TestMethod]
//     public void testConversion_KilogramToPound()
//     {
//         var w = new QuantityWeight(1.0, WeightUnit.Kilogram);

//         var result = w.ConvertTo(WeightUnit.Pound);

//         Assert.AreEqual(2.20462, result.GetValue(), EPSILON);
//     }

//     [TestMethod]
//     public void testConversion_SameUnit()
//     {
//         var w = new QuantityWeight(5.0, WeightUnit.Kilogram);

//         var result = w.ConvertTo(WeightUnit.Kilogram);

//         Assert.AreEqual(5.0, result.GetValue(), EPSILON);
//     }

//     [TestMethod]
//     public void testConversion_ZeroValue()
//     {
//         var w = new QuantityWeight(0.0, WeightUnit.Kilogram);

//         var result = w.ConvertTo(WeightUnit.Gram);

//         Assert.AreEqual(0.0, result.GetValue(), EPSILON);
//     }

//     [TestMethod]
//     public void testConversion_NegativeValue()
//     {
//         var w = new QuantityWeight(-1.0, WeightUnit.Kilogram);

//         var result = w.ConvertTo(WeightUnit.Gram);

//         Assert.AreEqual(-1000.0, result.GetValue(), EPSILON);
//     }

//     [TestMethod]
//     public void testConversion_RoundTrip()
//     {
//         var w = new QuantityWeight(1.5, WeightUnit.Kilogram);

//         var result = w.ConvertTo(WeightUnit.Gram)
//                       .ConvertTo(WeightUnit.Kilogram);

//         Assert.AreEqual(1.5, result.GetValue(), EPSILON);
//     }

//     // ------------------------
//     // Addition Tests
//     // ------------------------

//     [TestMethod]
//     public void testAddition_SameUnit_KilogramPlusKilogram()
//     {
//         var w1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
//         var w2 = new QuantityWeight(2.0, WeightUnit.Kilogram);

//         var result = QuantityWeight.Add(w1, w2);

//         Assert.AreEqual(3.0, result.GetValue(), EPSILON);
//     }

//     [TestMethod]
//     public void testAddition_CrossUnit_KilogramPlusGram()
//     {
//         var w1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
//         var w2 = new QuantityWeight(1000.0, WeightUnit.Gram);

//         var result = QuantityWeight.Add(w1, w2);

//         Assert.AreEqual(2.0, result.GetValue(), EPSILON);
//     }

//     [TestMethod]
//     public void testAddition_CrossUnit_PoundPlusKilogram()
//     {
//         var w1 = new QuantityWeight(2.20462, WeightUnit.Pound);
//         var w2 = new QuantityWeight(1.0, WeightUnit.Kilogram);

//         var result = QuantityWeight.Add(w1, w2);

//         Assert.AreEqual(4.40924, result.GetValue(), EPSILON);
//     }

//     [TestMethod]
//     public void testAddition_ExplicitTargetUnit_Kilogram()
//     {
//         var w1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
//         var w2 = new QuantityWeight(1000.0, WeightUnit.Gram);

//         var result = QuantityWeight.Add(w1, w2, WeightUnit.Gram);

//         Assert.AreEqual(2000.0, result.GetValue(), EPSILON);
//     }

//     [TestMethod]
//     public void testAddition_Commutativity()
//     {
//         var w1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
//         var w2 = new QuantityWeight(1000.0, WeightUnit.Gram);

//         var r1 = QuantityWeight.Add(w1, w2);
//         var r2 = QuantityWeight.Add(w2, w1);

//         Assert.IsTrue(r1.Equals(r2));
//     }

//     [TestMethod]
//     public void testAddition_WithZero()
//     {
//         var w1 = new QuantityWeight(5.0, WeightUnit.Kilogram);
//         var w2 = new QuantityWeight(0.0, WeightUnit.Gram);

//         var result = QuantityWeight.Add(w1, w2);

//         Assert.AreEqual(5.0, result.GetValue(), EPSILON);
//     }

//     [TestMethod]
//     public void testAddition_NegativeValues()
//     {
//         var w1 = new QuantityWeight(5.0, WeightUnit.Kilogram);
//         var w2 = new QuantityWeight(-2000.0, WeightUnit.Gram);

//         var result = QuantityWeight.Add(w1, w2);

//         Assert.AreEqual(3.0, result.GetValue(), EPSILON);
//     }

//     [TestMethod]
//     public void testAddition_LargeValues()
//     {
//         var w1 = new QuantityWeight(1e6, WeightUnit.Kilogram);
//         var w2 = new QuantityWeight(1e6, WeightUnit.Kilogram);

//         var result = QuantityWeight.Add(w1, w2);

//         Assert.AreEqual(2e6, result.GetValue(), EPSILON);
//     }
// }