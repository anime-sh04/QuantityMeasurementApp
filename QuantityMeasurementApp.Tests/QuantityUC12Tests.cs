using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityUC12Tests
    {

        // ---------------- SUBTRACTION ----------------

        [TestMethod]
        public void testSubtraction_SameUnit_FeetMinusFeet()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(5, LengthUnit.Feet);

            var result = a.Subtract(b);

            Assert.AreEqual(5, result.GetValue());
            Assert.AreEqual(LengthUnit.Feet, result.GetUnit());
        }

        [TestMethod]
        public void testSubtraction_SameUnit_LitreMinusLitre()
        {
            var a = new Quantity<VolumeUnit>(10, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(3, VolumeUnit.Litre);

            var result = a.Subtract(b);

            Assert.AreEqual(7, result.GetValue());
        }

        [TestMethod]
        public void testSubtraction_CrossUnit_FeetMinusInches()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(6, LengthUnit.Inch);

            var result = a.Subtract(b);

            Assert.AreEqual(9.5, result.GetValue());
        }

        [TestMethod]
        public void testSubtraction_CrossUnit_InchesMinusFeet()
        {
            var a = new Quantity<LengthUnit>(120, LengthUnit.Inch);
            var b = new Quantity<LengthUnit>(5, LengthUnit.Feet);

            var result = a.Subtract(b);

            Assert.AreEqual(60, result.GetValue());
        }

        [TestMethod]
        public void testSubtraction_ExplicitTargetUnit_Inches()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(6, LengthUnit.Inch);

            var result = a.Subtract(b, LengthUnit.Inch);

            Assert.AreEqual(114, result.GetValue());
        }

        [TestMethod]
        public void testSubtraction_ResultingInNegative()
        {
            var a = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(10, LengthUnit.Feet);

            var result = a.Subtract(b);

            Assert.AreEqual(-5, result.GetValue());
        }

        [TestMethod]
        public void testSubtraction_ResultingInZero()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(120, LengthUnit.Inch);

            var result = a.Subtract(b);

            Assert.AreEqual(0, result.GetValue());
        }

        [TestMethod]
        public void testSubtraction_WithZeroOperand()
        {
            var a = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(0, LengthUnit.Inch);

            var result = a.Subtract(b);

            Assert.AreEqual(5, result.GetValue());
        }

        [TestMethod]
        public void testSubtraction_WithNegativeValues()
        {
            var a = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(-2, LengthUnit.Feet);

            var result = a.Subtract(b);

            Assert.AreEqual(7, result.GetValue());
        }

        [TestMethod]
        public void testSubtraction_NonCommutative()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(5, LengthUnit.Feet);

            var r1 = a.Subtract(b);
            var r2 = b.Subtract(a);

            Assert.AreNotEqual(r1.GetValue(), r2.GetValue());
        }

        [TestMethod]
        public void testSubtraction_WithLargeValues()
        {
            var a = new Quantity<WeightUnit>(1e6, WeightUnit.Kilogram);
            var b = new Quantity<WeightUnit>(5e5, WeightUnit.Kilogram);

            var result = a.Subtract(b);

            Assert.AreEqual(5e5, result.GetValue());
        }

        [TestMethod]
        public void testSubtraction_ChainedOperations()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(2, LengthUnit.Feet);
            var c = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            var result = a.Subtract(b).Subtract(c);

            Assert.AreEqual(7, result.GetValue());
        }


        // ---------------- DIVISION ----------------

        [TestMethod]
        public void testDivision_SameUnit_FeetDividedByFeet()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(2, LengthUnit.Feet);

            var result = a.Divide(b);

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void testDivision_SameUnit_LitreDividedByLitre()
        {
            var a = new Quantity<VolumeUnit>(10, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(5, VolumeUnit.Litre);

            var result = a.Divide(b);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void testDivision_CrossUnit_FeetDividedByInches()
        {
            var a = new Quantity<LengthUnit>(24, LengthUnit.Inch);
            var b = new Quantity<LengthUnit>(2, LengthUnit.Feet);

            var result = a.Divide(b);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void testDivision_RatioLessThanOne()
        {
            var a = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(10, LengthUnit.Feet);

            var result = a.Divide(b);

            Assert.AreEqual(0.5, result);
        }

        [TestMethod]
        public void testDivision_RatioEqualToOne()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(10, LengthUnit.Feet);

            var result = a.Divide(b);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void testDivision_NonCommutative()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(5, LengthUnit.Feet);

            var r1 = a.Divide(b);
            var r2 = b.Divide(a);

            Assert.AreNotEqual(r1, r2);
        }

        [TestMethod]
        public void testDivision_WithLargeRatio()
        {
            var a = new Quantity<WeightUnit>(1e6, WeightUnit.Kilogram);
            var b = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);

            var result = a.Divide(b);

            Assert.AreEqual(1e6, result);
        }

        [TestMethod]
        public void testDivision_WithSmallRatio()
        {
            var a = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);
            var b = new Quantity<WeightUnit>(1e6, WeightUnit.Kilogram);

            var result = a.Divide(b);

            Assert.AreEqual(1e-6, result);
        }
    }
}