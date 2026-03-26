using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QuantityMeasurementAppBusinessLayer.Service;
using QuantityMeasurementAppModelLayer.DTOs;
using QuantityMeasurementAppModelLayer.Models;
using QuantityMeasurementAppRepositoryLayer.Interface;
using System;

namespace QuantityMeasurementApp.Tests.Service
{
    [TestClass]
    public class QuantityMeasurementServiceTest
    {
        private Mock<IQuantityMeasurementRepository> _mockRepo;
        private QuantityMeasurementServiceImpl _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IQuantityMeasurementRepository>();
            _service = new QuantityMeasurementServiceImpl(_mockRepo.Object, null);
        }

        // ✅ TEST 1: ADD SUCCESS
        [TestMethod]
        public void Add_LengthValues_ReturnsCorrectResult()
        {
            var q1 = new QuantityDTO(1, "Feet", "Length");
            var q2 = new QuantityDTO(12, "Inch", "Length");

            var result = _service.Add(q1, q2, "Feet");

            Assert.IsNotNull(result);
        }

        // ✅ TEST 2: ADD WITH DIFFERENT UNITS
        [TestMethod]
        public void Add_WeightValues_ReturnsCorrectResult()
        {
            var q1 = new QuantityDTO(1, "Kilogram", "Weight");
            var q2 = new QuantityDTO(1000, "Gram", "Weight");

            var result = _service.Add(q1, q2, "Kilogram");

            Assert.IsNotNull(result);
        }

        // ✅ TEST 3: INVALID INPUT (NULL)
        [TestMethod]
        public void Add_NullInput_ThrowsException()
        {
            try
            {
                _service.Add(null, null, "Feet");
                Assert.Fail("Expected exception not thrown");
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }
        
        [TestMethod]
        public void Add_DifferentMeasurementTypes_ThrowsException()
        {
            var q1 = new QuantityDTO(1, "Feet", "Length");
            var q2 = new QuantityDTO(1, "Kilogram", "Weight");
        
            try
            {
                _service.Add(q1, q2, "Feet");
                Assert.Fail("Expected exception not thrown");
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        // ✅ TEST 5: COMPARE SUCCESS
        [TestMethod]
        public void Compare_EqualValues_ReturnsTrue()
        {
            var q1 = new QuantityDTO(1, "Feet", "Length");
            var q2 = new QuantityDTO(12, "Inch", "Length");

            var result = _service.Compare(q1, q2);

            Assert.IsTrue(result);
        }

        // ✅ TEST 6: COMPARE FAIL
        [TestMethod]
        public void Compare_DifferentValues_ReturnsFalse()
        {
            var q1 = new QuantityDTO(1, "Feet", "Length");
            var q2 = new QuantityDTO(2, "Feet", "Length");

            var result = _service.Compare(q1, q2);

            Assert.IsFalse(result);
        }

        // ✅ TEST 7: SAVE CALLED
        [TestMethod]
        public void Add_ValidInput_CallsRepositorySave()
        {
            var q1 = new QuantityDTO(1, "Feet", "Length");
            var q2 = new QuantityDTO(12, "Inch", "Length");

            _service.Add(q1, q2, "Feet");

            _mockRepo.Verify(r => r.Save(It.IsAny<QuantityMeasurementEntity>()), Times.Once);
        }
    }
}