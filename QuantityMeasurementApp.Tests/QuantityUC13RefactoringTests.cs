using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityUC13RefactoringTests
    {

        // =============================
        // HELPER CONSTANT
        // =============================

        const double EPS = 0.0001;

        // =============================
        // REFACTORING DELEGATION TESTS
        // =============================

        [TestMethod]
        public void testRefactoring_Add_DelegatesViaHelper()
        {
            var a = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            var result = a.Add(b);

            Assert.AreEqual(2, result.GetValue(), EPS);
        }

        [TestMethod]
        public void testRefactoring_Subtract_DelegatesViaHelper()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(6, LengthUnit.Inch);

            var result = a.Subtract(b);

            Assert.AreEqual(9.5, result.GetValue(), EPS);
        }

        [TestMethod]
        public void testRefactoring_Divide_DelegatesViaHelper()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(2, LengthUnit.Feet);

            var result = a.Divide(b);

            Assert.AreEqual(5, result, EPS);
        }

        // =============================
        // VALIDATION TESTS
        // =============================

        [TestMethod]
        public void testValidation_NullOperand_ConsistentAcrossOperations()
        {
            var q = new Quantity<LengthUnit>(10, LengthUnit.Feet);

            Assert.Throws<ArgumentException>(() => q.Add(null));
            Assert.Throws<ArgumentException>(() => q.Subtract(null));
            Assert.Throws<ArgumentException>(() => q.Divide(null));
        }

        // =============================
        // UC12 BEHAVIOR PRESERVED
        // =============================

        [TestMethod]
        public void testAdd_UC12_BehaviorPreserved()
        {
            var a = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            var result = a.Add(b);

            Assert.AreEqual(2, result.GetValue(), EPS);
        }

        [TestMethod]
        public void testSubtract_UC12_BehaviorPreserved()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(120, LengthUnit.Inch);

            var result = a.Subtract(b);

            Assert.AreEqual(0, result.GetValue(), EPS);
        }

        [TestMethod]
        public void testDivide_UC12_BehaviorPreserved()
        {
            var a = new Quantity<LengthUnit>(24, LengthUnit.Inch);
            var b = new Quantity<LengthUnit>(2, LengthUnit.Feet);

            var result = a.Divide(b);

            Assert.AreEqual(1, result, EPS);
        }

        // =============================
        // ROUNDING TESTS
        // =============================

        [TestMethod]
        public void testRounding_AddSubtract_TwoDecimalPlaces()
        {
            var a = new Quantity<LengthUnit>(1.11111, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(0.22222, LengthUnit.Feet);

            var result = a.Add(b);

            Assert.AreEqual(1.33, result.GetValue(), EPS);
        }

        [TestMethod]
        public void testRounding_Divide_NoRounding()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(3, LengthUnit.Feet);

            var result = a.Divide(b);

            Assert.AreEqual(3.333333333, result, 0.01);
        }

        // =============================
        // TARGET UNIT TESTS
        // =============================

        [TestMethod]
        public void testImplicitTargetUnit_AddSubtract()
        {
            var a = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            var result = a.Add(b);

            Assert.AreEqual(LengthUnit.Feet, result.GetUnit());
        }

        [TestMethod]
        public void testExplicitTargetUnit_AddSubtract_Overrides()
        {
            var a = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            var result = a.Add(b, LengthUnit.Inch);

            Assert.AreEqual(24, result.GetValue(), EPS);
            Assert.AreEqual(LengthUnit.Inch, result.GetUnit());
        }

        // =============================
        // IMMUTABILITY TESTS
        // =============================

        [TestMethod]
        public void testImmutability_AfterAdd_ViaCentralizedHelper()
        {
            var a = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            a.Add(b);

            Assert.AreEqual(1, a.GetValue());
        }

        [TestMethod]
        public void testImmutability_AfterSubtract_ViaCentralizedHelper()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(5, LengthUnit.Feet);

            a.Subtract(b);

            Assert.AreEqual(10, a.GetValue());
        }

        [TestMethod]
        public void testImmutability_AfterDivide_ViaCentralizedHelper()
        {
            var a = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(5, LengthUnit.Feet);

            a.Divide(b);

            Assert.AreEqual(10, a.GetValue());
        }

        // =============================
        // MULTI CATEGORY TEST
        // =============================

        [TestMethod]
        public void testAllOperations_AcrossAllCategories()
        {
            var length1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var length2 = new Quantity<LengthUnit>(2, LengthUnit.Feet);

            var weight1 = new Quantity<WeightUnit>(10, WeightUnit.Kilogram);
            var weight2 = new Quantity<WeightUnit>(5, WeightUnit.Kilogram);

            var volume1 = new Quantity<VolumeUnit>(5, VolumeUnit.Litre);
            var volume2 = new Quantity<VolumeUnit>(2, VolumeUnit.Litre);

            Assert.AreEqual(12, length1.Add(length2).GetValue());
            Assert.AreEqual(5, weight1.Subtract(weight2).GetValue());
            Assert.AreEqual(2.5, volume1.Divide(volume2), EPS);
        }

        // =============================
        // CHAIN OPERATION TEST
        // =============================

        [TestMethod]
        public void testArithmetic_Chain_Operations()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.Feet);
            var q3 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q4 = new Quantity<LengthUnit>(3, LengthUnit.Feet);

            var result = q1.Add(q2).Subtract(q3).Divide(q4);

            Assert.AreEqual(11.0 / 3.0, result, 0.01);
        }

    }
}