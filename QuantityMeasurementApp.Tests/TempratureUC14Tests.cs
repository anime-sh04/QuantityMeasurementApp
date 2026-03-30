using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class TemperatureUC14Tests
    {
        const double EPS = 0.0001;

        // -------------------------
        // EQUALITY TESTS
        // -------------------------

        [TestMethod]
        public void testTemperatureEquality_CelsiusToCelsius_SameValue()
        {
            var t1 = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);

            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void testTemperatureEquality_FahrenheitToFahrenheit_SameValue()
        {
            var t1 = new Quantity<TemperatureUnit>(32, TemperatureUnit.Fahrenheit);
            var t2 = new Quantity<TemperatureUnit>(32, TemperatureUnit.Fahrenheit);

            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void testTemperatureEquality_CelsiusToFahrenheit_0Celsius32Fahrenheit()
        {
            var t1 = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(32, TemperatureUnit.Fahrenheit);

            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void testTemperatureEquality_CelsiusToFahrenheit_100Celsius212Fahrenheit()
        {
            var t1 = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(212, TemperatureUnit.Fahrenheit);

            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void testTemperatureEquality_CelsiusToFahrenheit_Negative40Equal()
        {
            var t1 = new Quantity<TemperatureUnit>(-40, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(-40, TemperatureUnit.Fahrenheit);

            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void testTemperatureEquality_SymmetricProperty()
        {
            var a = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(32, TemperatureUnit.Fahrenheit);

            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(b.Equals(a));
        }

        [TestMethod]
        public void testTemperatureEquality_ReflexiveProperty()
        {
            var a = new Quantity<TemperatureUnit>(25, TemperatureUnit.Celsius);

            Assert.IsTrue(a.Equals(a));
        }

        // -------------------------
        // CONVERSION TESTS
        // -------------------------

        [TestMethod]
        public void testTemperatureConversion_CelsiusToFahrenheit_VariousValues()
        {
            var t = new Quantity<TemperatureUnit>(50, TemperatureUnit.Celsius);
            var result = t.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.AreEqual(122, result.GetValue(), EPS);
        }

        [TestMethod]
        public void testTemperatureConversion_FahrenheitToCelsius_VariousValues()
        {
            var t = new Quantity<TemperatureUnit>(122, TemperatureUnit.Fahrenheit);
            var result = t.ConvertTo(TemperatureUnit.Celsius);

            Assert.AreEqual(50, result.GetValue(), EPS);
        }

        [TestMethod]
        public void testTemperatureConversion_RoundTrip_PreservesValue()
        {
            var t = new Quantity<TemperatureUnit>(37, TemperatureUnit.Celsius);

            var f = t.ConvertTo(TemperatureUnit.Fahrenheit);
            var c = f.ConvertTo(TemperatureUnit.Celsius);

            Assert.AreEqual(37, c.GetValue(), EPS);
        }

        [TestMethod]
        public void testTemperatureConversion_SameUnit()
        {
            var t = new Quantity<TemperatureUnit>(25, TemperatureUnit.Celsius);
            var result = t.ConvertTo(TemperatureUnit.Celsius);

            Assert.AreEqual(25, result.GetValue());
        }

        [TestMethod]
        public void testTemperatureConversion_ZeroValue()
        {
            var t = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);
            var result = t.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.AreEqual(32, result.GetValue(), EPS);
        }

        [TestMethod]
        public void testTemperatureConversion_NegativeValues()
        {
            var t = new Quantity<TemperatureUnit>(-20, TemperatureUnit.Celsius);
            var result = t.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.AreEqual(-4, result.GetValue(), EPS);
        }

        [TestMethod]
        public void testTemperatureConversion_LargeValues()
        {
            var t = new Quantity<TemperatureUnit>(1000, TemperatureUnit.Celsius);
            var result = t.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.AreEqual(1832, result.GetValue(), EPS);
        }

        // -------------------------
        // UNSUPPORTED OPERATIONS
        // -------------------------

        [TestMethod]
        public void testTemperatureUnsupportedOperation_Add()
        {
            var a = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(50, TemperatureUnit.Celsius);

            Assert.Throws<NotSupportedException>(() => a.Add(b));
        }

        [TestMethod]
        public void testTemperatureUnsupportedOperation_Subtract()
        {
            var a = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(50, TemperatureUnit.Celsius);

            Assert.Throws<NotSupportedException>(() => a.Subtract(b));
        }

        [TestMethod]
        public void testTemperatureUnsupportedOperation_Divide()
        {
            var a = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var b = new Quantity<TemperatureUnit>(50, TemperatureUnit.Celsius);

            Assert.Throws<NotSupportedException>(() => a.Divide(b));
        }

        // -------------------------
        // CROSS CATEGORY TESTS
        // -------------------------

        [TestMethod]
        public void testTemperatureVsLengthIncompatibility()
        {
            var temp = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var length = new Quantity<LengthUnit>(100, LengthUnit.Feet);

            Assert.IsFalse(temp.Equals(length));
        }

        [TestMethod]
        public void testTemperatureVsWeightIncompatibility()
        {
            var temp = new Quantity<TemperatureUnit>(50, TemperatureUnit.Celsius);
            var weight = new Quantity<WeightUnit>(50, WeightUnit.Kilogram);

            Assert.IsFalse(temp.Equals(weight));
        }

        [TestMethod]
        public void testTemperatureVsVolumeIncompatibility()
        {
            var temp = new Quantity<TemperatureUnit>(25, TemperatureUnit.Celsius);
            var volume = new Quantity<VolumeUnit>(25, VolumeUnit.Litre);

            Assert.IsFalse(temp.Equals(volume));
        }

        // -------------------------
        // EDGE TESTS
        // -------------------------

        [TestMethod]
        public void testTemperatureNullOperandValidation_InComparison()
        {
            var temp = new Quantity<TemperatureUnit>(25, TemperatureUnit.Celsius);

            Assert.IsFalse(temp.Equals(null));
        }

        [TestMethod]
        public void testTemperatureDifferentValuesInequality()
        {
            var t1 = new Quantity<TemperatureUnit>(50, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);

            Assert.IsFalse(t1.Equals(t2));
        }

        [TestMethod]
        public void testTemperatureConversionPrecision_Epsilon()
        {
            var c = new Quantity<TemperatureUnit>(37, TemperatureUnit.Celsius);
            var f = c.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.AreEqual(98.6, f.GetValue(), 0.01);
        }

        [TestMethod]
        public void testTemperatureIntegrationWithGenericQuantity()
        {
            var temp = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);
            var converted = temp.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.AreEqual(32, converted.GetValue(), EPS);
        }
    }
}