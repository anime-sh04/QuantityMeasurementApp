using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Console.Controller;
using QuantityMeasurementApp.Console.Menu;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementServices.Services;
using QuantityMeasurementAppRepositoryLayer.Cache;
using  QuantityMeasurementAppRepositoryLayer.Interface;
using QuantityMeasurementAppModelLayer.Models;
using QuantityMeasurementAppModelLayer.DTOs;
using System.Collections.Generic;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    [TestCategory("QuantityMeasurementUC15")]
    public class NTierTesting
    {
        private QuantityMeasurementService _service;
        private QuantityMeasurementController _controller;

        // Fake Repository 
        private class FakeRepository : IQuantityMeasurementRepository
        {
            private readonly List<QuantityMeasurementEntity> _data = new();

            public void Save(QuantityMeasurementEntity entity)
            {
                entity.Id = _data.Count + 1;
                _data.Add(entity);
            }

            public List<QuantityMeasurementEntity> GetAll()
            {
                return _data;
            }
        }

        [TestInitialize]
        public void Setup()
        {
            var repo = new FakeRepository();
            _service = new QuantityMeasurementService(repo);
            _controller = new QuantityMeasurementController(_service);
        }

        // SERVICE TESTS

        [TestMethod]
        public void Compare_SameUnit_ShouldReturnTrue()
        {
            var a = new QuantityDTO { Value = 12, Unit = "inch", Category = "Length" };
            var b = new QuantityDTO { Value = 12, Unit = "inch", Category = "Length" };

            Assert.IsTrue(_service.Compare(a, b));
        }

        [TestMethod]
        public void Compare_DifferentUnit_ShouldReturnTrue()
        {
            var a = new QuantityDTO { Value = 1, Unit = "feet", Category = "Length" };
            var b = new QuantityDTO { Value = 12, Unit = "inch", Category = "Length" };

            Assert.IsTrue(_service.Compare(a, b));
        }

        [TestMethod]
        public void Convert_Length_ShouldWork()
        {
            var dto = new QuantityDTO { Value = 1, Unit = "feet", Category = "Length" };

            var result = _service.Convert(dto, "inch");

            Assert.AreEqual(12, result.BaseValue, 0.001);
            Assert.AreEqual("inch", result.Unit.ToLower());
        }

        [TestMethod]
        public void Add_WithTargetUnit_ShouldConvertAndAdd()
        {
            var a = new QuantityDTO { Value = 1, Unit = "feet", Category = "Length" };
            var b = new QuantityDTO { Value = 12, Unit = "inch", Category = "Length" };

            var result = _service.Add(a, b, "feet");

            Assert.AreEqual(2, result.BaseValue, 0.001);
            Assert.AreEqual("feet", result.Unit.ToLower());
        }

        [TestMethod]
        public void Add_WithoutTargetUnit_SameUnit_ShouldWork()
        {
            var a = new QuantityDTO { Value = 2, Unit = "kg", Category = "Weight" };
            var b = new QuantityDTO { Value = 3, Unit = "kg", Category = "Weight" };

            var result = _service.Add(a, b);

            Assert.AreEqual(5, result.BaseValue, 0.001);
            Assert.AreEqual("kg", result.Unit.ToLower());
        }


        [TestMethod]
        public void Subtract_ShouldWork()
        {
            var a = new QuantityDTO { Value = 5, Unit = "kg", Category = "Weight" };
            var b = new QuantityDTO { Value = 2, Unit = "kg", Category = "Weight" };

            var result = _service.Subtract(a, b);

            Assert.AreEqual(3, result.BaseValue, 0.001);
        }

        [TestMethod]
        public void Divide_ShouldReturnRatio()
        {
            var a = new QuantityDTO { Value = 10, Unit = "kilogram", Category = "Weight" };
            var b = new QuantityDTO { Value = 2, Unit = "kilogram", Category = "Weight" };

            var result = _service.Divide(a, b);

            Assert.AreEqual(5, result, 0.001);
        }

        // CONTROLLER TESTS

        [TestMethod]
        public void Controller_Add_ShouldReturnResult()
        {
            var a = new QuantityDTO { Value = 1, Unit = "feet", Category = "Length" };
            var b = new QuantityDTO { Value = 12, Unit = "inch", Category = "Length" };

            var result = _controller.AddQuantities(a, b, "feet");

            Assert.AreEqual(2, result.BaseValue, 0.001);
        }

        [TestMethod]
        public void Controller_Convert_ShouldWork()
        {
            var dto = new QuantityDTO { Value = 1, Unit = "kilogram", Category = "Weight" };

            var result = _controller.ConvertQuantity(dto, "gram");

            Assert.AreEqual(1000, result.BaseValue, 0.001);
        }

        [TestMethod]
        public void Controller_Divide_ShouldReturnRatio()
        {
            var a = new QuantityDTO { Value = 10, Unit = "kilogram", Category = "Weight" };
            var b = new QuantityDTO { Value = 2, Unit = "kilogram", Category = "Weight" };

            var result = _controller.DivideQuantities(a, b);

            Assert.AreEqual(5, result, 0.001);
        }

        // HISTORY TEST

        [TestMethod]
        public void History_ShouldStoreData()
        {
            var a = new QuantityDTO { Value = 1, Unit = "feet", Category = "Length" };
            var b = new QuantityDTO { Value = 12, Unit = "inch", Category = "Length" };

            _service.Add(a, b, "feet");

            var history = _service.GetHistory();

            Assert.IsTrue(history.Count >= 2);
        }
    }
}