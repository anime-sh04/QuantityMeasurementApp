using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QuantityMeasurementApp.Api.Controller;
using QuantityMeasurementApp.Api.DTOs;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppModelLayer.DTOs;
using QuantityMeasurementAppModelLayer.Models;
using QuantityMeasurementAppRepositoryLayer.Data;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityMeasurementApiFactory : WebApplicationFactory<Program>
    {
        private readonly string _dbName = "TestDb_" + Guid.NewGuid();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                
                var optDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (optDescriptor != null) services.Remove(optDescriptor);

                
                var genericOpt = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions));
                if (genericOpt != null) services.Remove(genericOpt);

                
                services.RemoveAll<AppDbContext>();

                
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(_dbName));
            });
        }

        
        protected override void ConfigureClient(HttpClient client)
        {
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        }
    }

    
    internal static class Json
    {
        private static readonly JsonSerializerOptions Opts =
            new() { PropertyNameCaseInsensitive = true };

        public static T? Deserialize<T>(string json) =>
            JsonSerializer.Deserialize<T>(json, Opts);

        public static StringContent Body(object obj) =>
            new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    }

    
    
    
    [TestClass]
    public class QuantityMeasurementIntegrationTests
    {
        private static QuantityMeasurementApiFactory _factory = null!;
        private static HttpClient _client = null!;

        [ClassInitialize]
        public static void ClassInit(TestContext _)
        {
            _factory = new QuantityMeasurementApiFactory();
            _client  = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        
        private static object ComparePayload(
            double v1, string u1, double v2, string u2, string type) => new
        {
            quantityOne = new { value = v1, unit = u1, measurementType = type },
            quantityTwo = new { value = v2, unit = u2, measurementType = type }
        };

        private static object AddPayload(
            double v1, string u1, double v2, string u2, string type, string target) => new
        {
            quantityOne = new { value = v1, unit = u1, measurementType = type },
            quantityTwo = new { value = v2, unit = u2, measurementType = type },
            targetUnit  = target
        };

        private static object ConvertPayload(
            double val, string unit, string type, string targetUnit) => new
        {
            source     = new { value = val, unit, measurementType = type },
            targetUnit
        };

        private static object SubtractPayload(
            double v1, string u1, double v2, string u2, string type, string target) => new
        {
            quantityOne = new { value = v1, unit = u1, measurementType = type },
            quantityTwo = new { value = v2, unit = u2, measurementType = type },
            targetUnit  = target
        };

        private static object DividePayload(
            double v1, string u1, double v2, string u2, string type) => new
        {
            quantityOne = new { value = v1, unit = u1, measurementType = type },
            quantityTwo = new { value = v2, unit = u2, measurementType = type }
        };

        
        [TestMethod]
        public async Task TestApplicationStarts_ContextLoadsAndServerResponds()
        {
            var response = await _client.GetAsync("/api/quantity/history");
            Assert.IsTrue((int)response.StatusCode < 500,
                "Server should start without 5xx errors.");
        }

        
        
        
        [TestMethod]
        public async Task TestRestEndpointCompareQuantities_Returns200()
        {
            var r = await _client.PostAsync("/api/quantity/compare",
                Json.Body(ComparePayload(1, "Feet", 12, "Inches", "Length")));
            Assert.AreEqual(HttpStatusCode.OK, r.StatusCode);
        }

        [TestMethod]
        public async Task TestRestEndpointCompareQuantities_ResponseContainsComparisonResult()
        {
            var r   = await _client.PostAsync("/api/quantity/compare",
                Json.Body(ComparePayload(1, "Feet", 12, "Inches", "Length")));
            var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());
            Assert.IsTrue(doc.RootElement.TryGetProperty("equal", out _),
                "Response should contain 'equal'.");
        }

        [TestMethod]
        public async Task TestRestEndpointCompareQuantities_EqualValues_EqualTrue()
        {
            var r   = await _client.PostAsync("/api/quantity/compare",
                Json.Body(ComparePayload(1, "Feet", 12, "Inches", "Length")));
            var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());
            Assert.IsTrue(doc.RootElement.GetProperty("equal").GetBoolean(),
                "1 Foot == 12 Inches.");
        }

        [TestMethod]
        public async Task TestRestEndpointCompareQuantities_UnequalValues_EqualFalse()
        {
            var r   = await _client.PostAsync("/api/quantity/compare",
                Json.Body(ComparePayload(1, "Feet", 1, "Inches", "Length")));
            var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());
            Assert.IsFalse(doc.RootElement.GetProperty("equal").GetBoolean(),
                "1 Foot != 1 Inch.");
        }

        
        
        
        [TestMethod]
        public async Task TestRestEndpointConvertQuantities_CelsiusToFahrenheit_CorrectValue()
        {
            var r   = await _client.PostAsync("/api/quantity/convert",
                Json.Body(ConvertPayload(100, "Celsius", "Temperature", "Fahrenheit")));
            Assert.AreEqual(HttpStatusCode.OK, r.StatusCode);
            var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());
            Assert.AreEqual(212.0, doc.RootElement.GetProperty("value").GetDouble(), 1e-4);
        }

        [TestMethod]
        public async Task TestRestEndpointConvertQuantities_FeetToInches_CorrectValue()
        {
            var r   = await _client.PostAsync("/api/quantity/convert",
                Json.Body(ConvertPayload(1, "Feet", "Length", "Inches")));
            var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());
            Assert.AreEqual(12.0,    doc.RootElement.GetProperty("value").GetDouble(), 1e-6);
            Assert.AreEqual("Inches", doc.RootElement.GetProperty("unit").GetString());
        }

        
        
        
        [TestMethod]
        public async Task TestRestEndpointAddQuantities_FeetAndInches_SumCorrect()
        {
            var r   = await _client.PostAsync("/api/quantity/add",
                Json.Body(AddPayload(1, "Feet", 6, "Inches", "Length", "Inches")));
            Assert.AreEqual(HttpStatusCode.OK, r.StatusCode);
            var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());
            Assert.AreEqual(18.0, doc.RootElement.GetProperty("value").GetDouble(), 1e-6,
                "1 Foot + 6 Inches = 18 Inches.");
        }

        [TestMethod]
        public async Task TestRestEndpointAddQuantities_ResponseContainsExpression()
        {
            var r    = await _client.PostAsync("/api/quantity/add",
                Json.Body(AddPayload(1, "Kilogram", 500, "Gram", "Weight", "Gram")));
            var body = await r.Content.ReadAsStringAsync();
            Assert.IsTrue(body.Contains("expression") && body.Contains("+"));
        }

        
        
        
        [TestMethod]
        public async Task TestRestEndpointInvalidInput_MalformedJson_Returns400()
        {
            var r = await _client.PostAsync("/api/quantity/compare",
                new StringContent("{ not json !!!", Encoding.UTF8, "application/json"));
            Assert.AreEqual(HttpStatusCode.BadRequest, r.StatusCode);
        }

        [TestMethod]
        public async Task TestRestEndpointInvalidInput_MixedTypes_Returns400()
        {
            var payload = new
            {
                quantityOne = new { value = 1, unit = "Feet",     measurementType = "Length" },
                quantityTwo = new { value = 1, unit = "Kilogram", measurementType = "Weight" }
            };
            var r = await _client.PostAsync("/api/quantity/compare", Json.Body(payload));
            Assert.AreEqual(HttpStatusCode.BadRequest, r.StatusCode);
        }

        [TestMethod]
        public async Task TestRestEndpointInvalidInput_TemperatureAdd_Returns400()
        {
            var r = await _client.PostAsync("/api/quantity/add",
                Json.Body(AddPayload(10, "Celsius", 20, "Celsius", "Temperature", "Celsius")));
            Assert.AreEqual(HttpStatusCode.BadRequest, r.StatusCode);
        }

        
        
        
        [TestMethod]
        public async Task TestRestEndpointMissingParameter_EmptyBody_Returns400()
        {
            var r = await _client.PostAsync("/api/quantity/add",
                new StringContent("{}", Encoding.UTF8, "application/json"));
            Assert.AreEqual(HttpStatusCode.BadRequest, r.StatusCode);
        }

        [TestMethod]
        public async Task TestRestEndpointMissingParameter_MissingTargetUnit_Returns400()
        {
            var payload = new
            {
                quantityOne = new { value = 1, unit = "Feet", measurementType = "Length" },
                quantityTwo = new { value = 1, unit = "Feet", measurementType = "Length" }
                
            };
            var r = await _client.PostAsync("/api/quantity/add", Json.Body(payload));
            Assert.AreEqual(HttpStatusCode.BadRequest, r.StatusCode);
        }

        [TestMethod]
        public async Task TestRestEndpointMissingParameter_MissingConvertSource_Returns400()
        {
            var r = await _client.PostAsync("/api/quantity/convert",
                Json.Body(new { targetUnit = "Fahrenheit" }));
            Assert.AreEqual(HttpStatusCode.BadRequest, r.StatusCode);
        }

        
        
        [TestMethod]
        public async Task TestDatabasePersistence_SaveViaApi_EntityInDb()
        {
            await _client.PostAsync("/api/quantity/convert",
                Json.Body(ConvertPayload(100, "Celsius", "Temperature", "Fahrenheit")));

            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            Assert.IsTrue(await db.QuantityMeasurements.AnyAsync(e => e.OperationType == "Convert"),
                "Convert entity must be persisted.");
        }

        [TestMethod]
        public async Task TestDatabasePersistence_MultipleOps_AllPersisted()
        {
            await _client.PostAsync("/api/quantity/compare",
                Json.Body(ComparePayload(1, "Kilogram", 1000, "Gram", "Weight")));
            await _client.PostAsync("/api/quantity/add",
                Json.Body(AddPayload(1, "Litre", 500, "Millilitre", "Volume", "Litre")));

            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            Assert.IsTrue(await db.QuantityMeasurements.AnyAsync(e => e.OperationType == "Compare"));
            Assert.IsTrue(await db.QuantityMeasurements.AnyAsync(e => e.OperationType == "Add"));
        }

        
        
        
        [TestMethod]
        public async Task TestRepositoryFindByOperation_OnlyMatchingEntitiesReturned()
        {
            await _client.PostAsync("/api/quantity/compare",
                Json.Body(ComparePayload(1, "Feet", 1, "Feet", "Length")));
            await _client.PostAsync("/api/quantity/add",
                Json.Body(AddPayload(1, "Feet", 1, "Feet", "Length", "Feet")));

            var r        = await _client.GetAsync("/api/quantity/history/operation/Compare");
            Assert.AreEqual(HttpStatusCode.OK, r.StatusCode);
            var entities = Json.Deserialize<List<QuantityMeasurementEntity>>(
                await r.Content.ReadAsStringAsync())!;
            Assert.IsTrue(entities.All(e => e.OperationType == "Compare"));
        }

        [TestMethod]
        public async Task TestRepositoryFindByOperation_AddOperation_CorrectEntities()
        {
            await _client.PostAsync("/api/quantity/add",
                Json.Body(AddPayload(2, "Kilogram", 500, "Gram", "Weight", "Gram")));
            var entities = Json.Deserialize<List<QuantityMeasurementEntity>>(
                await (await _client.GetAsync("/api/quantity/history/operation/Add"))
                .Content.ReadAsStringAsync())!;
            Assert.IsTrue(entities.Count >= 1);
            Assert.IsTrue(entities.All(e => e.OperationType == "Add"));
        }

        
        
        
        [TestMethod]
        public async Task TestRepositoryCustomQuery_FilterByTemperature_CorrectEntities()
        {
            await _client.PostAsync("/api/quantity/convert",
                Json.Body(ConvertPayload(0, "Celsius", "Temperature", "Kelvin")));
            var entities = Json.Deserialize<List<QuantityMeasurementEntity>>(
                await (await _client.GetAsync("/api/quantity/history/type/Temperature"))
                .Content.ReadAsStringAsync())!;
            Assert.IsTrue(entities.All(e => e.MeasurementType == "Temperature"));
        }

        [TestMethod]
        public async Task TestRepositoryCustomQuery_FilterByLength_CorrectEntities()
        {
            await _client.PostAsync("/api/quantity/compare",
                Json.Body(ComparePayload(1, "Yards", 3, "Feet", "Length")));
            var entities = Json.Deserialize<List<QuantityMeasurementEntity>>(
                await (await _client.GetAsync("/api/quantity/history/type/Length"))
                .Content.ReadAsStringAsync())!;
            Assert.IsTrue(entities.All(e => e.MeasurementType == "Length"));
        }

        
        
        
        [TestMethod]
        public async Task TestContentNegotiation_JSON_ResponseIsApplicationJson()
        {
            var req = new HttpRequestMessage(HttpMethod.Post, "/api/quantity/compare")
            {
                Content = Json.Body(ComparePayload(1, "Kilogram", 1000, "Gram", "Weight"))
            };
            req.Headers.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var r = await _client.SendAsync(req);
            Assert.AreEqual(HttpStatusCode.OK,  r.StatusCode);
            Assert.AreEqual("application/json", r.Content.Headers.ContentType?.MediaType);
        }

        
        
        
        [TestMethod]
        public async Task TestExceptionHandling_DivideByZero_Returns400WithMessage()
        {
            var r = await _client.PostAsync("/api/quantity/divide",
                Json.Body(DividePayload(1, "Feet", 0, "Feet", "Length")));
            Assert.AreEqual(HttpStatusCode.BadRequest, r.StatusCode);
            Assert.IsTrue((await r.Content.ReadAsStringAsync()).Contains("message"));
        }

        [TestMethod]
        public async Task TestExceptionHandling_ValidationFailure_StructuredErrorReturned()
        {
            var r    = await _client.PostAsync("/api/quantity/convert",
                new StringContent("{}", Encoding.UTF8, "application/json"));
            Assert.AreEqual(HttpStatusCode.BadRequest, r.StatusCode);
            var body = await r.Content.ReadAsStringAsync();
            Assert.IsTrue(body.Contains("message") || body.Contains("errors"));
        }

        
        
        
        [TestMethod]
        public async Task TestRequestPathVariable_OperationType_ExtractedCorrectly()
        {
            var r = await _client.GetAsync("/api/quantity/history/operation/Compare");
            Assert.AreEqual(HttpStatusCode.OK, r.StatusCode);
        }

        [TestMethod]
        public async Task TestRequestPathVariable_MeasurementType_ExtractedCorrectly()
        {
            var r = await _client.GetAsync("/api/quantity/history/type/Weight");
            Assert.AreEqual(HttpStatusCode.OK, r.StatusCode);
        }

        
        
        
        [TestMethod]
        public async Task TestResponseSerialization_Object_AutoSerializedToJSON()
        {
            var r   = await _client.PostAsync("/api/quantity/convert",
                Json.Body(ConvertPayload(1, "Kilogram", "Weight", "Gram")));
            Assert.AreEqual(HttpStatusCode.OK, r.StatusCode);
            var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());
            Assert.IsTrue(doc.RootElement.TryGetProperty("value",      out _));
            Assert.IsTrue(doc.RootElement.TryGetProperty("unit",       out _));
            Assert.IsTrue(doc.RootElement.TryGetProperty("expression", out _));
        }

        
        
        
        [TestMethod]
        public void TestMockMvc_ComparisonTest_MockedService_Returns200()
        {
            var mock = new Mock<IQuantityMeasurementService>();
            mock.Setup(s => s.Compare(It.IsAny<QuantityDTO>(), It.IsAny<QuantityDTO>()))
                .Returns(true);

            var ctrl  = new QuantityMeasurementAPIController(mock.Object);
            var input = new CompareRequestDTO
            {
                QuantityOne = new QuantityRequest { Value = 1,  Unit = "Feet",   MeasurementType = "Length" },
                QuantityTwo = new QuantityRequest { Value = 12, Unit = "Inches", MeasurementType = "Length" }
            };

            var result = ctrl.Compare(input) as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            mock.Verify(s => s.Compare(It.IsAny<QuantityDTO>(), It.IsAny<QuantityDTO>()), Times.Once);
        }

        [TestMethod]
        public void TestMockMvc_ConversionTest_MockedService_Returns200()
        {
            var mock = new Mock<IQuantityMeasurementService>();
            mock.Setup(s => s.Convert(It.IsAny<QuantityDTO>(), "Fahrenheit"))
                .Returns(new QuantityModel(212, "Fahrenheit"));

            var ctrl  = new QuantityMeasurementAPIController(mock.Object);
            var input = new ConvertRequestDTO
            {
                Source     = new QuantityRequest { Value = 100, Unit = "Celsius", MeasurementType = "Temperature" },
                TargetUnit = "Fahrenheit"
            };

            var result = ctrl.Convert(input) as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        
        
        
        [TestMethod]
        public async Task TestMockMvc_ResponseAssertion_Status_ContentType_JsonPath()
        {
            var r = await _client.PostAsync("/api/quantity/add",
                Json.Body(AddPayload(2, "Litre", 500, "Millilitre", "Volume", "Litre")));

            Assert.AreEqual(HttpStatusCode.OK,  r.StatusCode);
            Assert.AreEqual("application/json", r.Content.Headers.ContentType?.MediaType);

            var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());
            Assert.AreEqual(2.5,    doc.RootElement.GetProperty("value").GetDouble(), 1e-6);
            Assert.AreEqual("Litre", doc.RootElement.GetProperty("unit").GetString());
        }

        
        
        
        [TestMethod]
        public async Task TestIntegrationTest_MultipleOperations_AllPersistCorrectly()
        {
            await _client.PostAsync("/api/quantity/compare",
                Json.Body(ComparePayload(1, "Gallon", 3785.41, "Millilitre", "Volume")));
            await _client.PostAsync("/api/quantity/add",
                Json.Body(AddPayload(1, "Yard", 1, "Feet", "Length", "Feet")));
            await _client.PostAsync("/api/quantity/subtract",
                Json.Body(SubtractPayload(2, "Kilogram", 500, "Gram", "Weight", "Gram")));
            await _client.PostAsync("/api/quantity/divide",
                Json.Body(DividePayload(1, "Kilogram", 1, "Gram", "Weight")));
            await _client.PostAsync("/api/quantity/convert",
                Json.Body(ConvertPayload(273.15, "Kelvin", "Temperature", "Celsius")));

            var entities = Json.Deserialize<List<QuantityMeasurementEntity>>(
                await (await _client.GetAsync("/api/quantity/history"))
                .Content.ReadAsStringAsync())!;

            var ops = entities.Select(e => e.OperationType).ToHashSet();
            foreach (var op in new[] { "Compare", "Add", "Subtract", "Divide", "Convert" })
                Assert.IsTrue(ops.Contains(op), $"'{op}' must be persisted.");
        }

        
        
        
        [TestMethod]
        public void TestDatabaseInitialization_TableExists_ColumnsMatchEntity()
        {
            using var scope = _factory.Services.CreateScope();
            var db     = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var entity = db.Model.FindEntityType(typeof(QuantityMeasurementEntity))!;
            var cols   = entity.GetProperties().Select(p => p.Name).ToList();

            foreach (var col in new[]
            {
                nameof(QuantityMeasurementEntity.Id),
                nameof(QuantityMeasurementEntity.OperationType),
                nameof(QuantityMeasurementEntity.MeasurementType),
                nameof(QuantityMeasurementEntity.FirstValue),
                nameof(QuantityMeasurementEntity.FirstUnit),
                nameof(QuantityMeasurementEntity.SecondValue),
                nameof(QuantityMeasurementEntity.SecondUnit),
                nameof(QuantityMeasurementEntity.Result),
                nameof(QuantityMeasurementEntity.CreatedAt)
            })
                CollectionAssert.Contains(cols, col, $"Column '{col}' missing.");
        }

        [TestMethod]
        public void TestDatabaseInitialization_IndexesConfigured()
        {
            using var scope   = _factory.Services.CreateScope();
            var db     = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var entity = db.Model.FindEntityType(typeof(QuantityMeasurementEntity))!;
            var idxs   = entity.GetIndexes().Select(i => i.GetDatabaseName() ?? "").ToList();

            CollectionAssert.Contains(idxs, "IX_QuantityMeasurements_OperationType");
            CollectionAssert.Contains(idxs, "IX_QuantityMeasurements_MeasurementType");
        }

        
        
        
        [TestMethod]
        public async Task TestMessageConverter_JSONToObject_AllFieldsPopulatedCorrectly()
        {
            await _client.PostAsync("/api/quantity/convert",
                Json.Body(ConvertPayload(37.5, "Celsius", "Temperature", "Fahrenheit")));

            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var e  = await db.QuantityMeasurements
                .Where(x => x.OperationType == "Convert" && x.MeasurementType == "Temperature")
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            Assert.IsNotNull(e);
            Assert.AreEqual(37.5,          e.FirstValue,      1e-6);
            Assert.AreEqual("Celsius",     e.FirstUnit);
            Assert.AreEqual("Temperature", e.MeasurementType);
        }

        
        
        
        [TestMethod]
        public async Task TestMessageConverter_ObjectToJSON_MatchesExpectedFormat()
        {
            var r   = await _client.PostAsync("/api/quantity/convert",
                Json.Body(ConvertPayload(0, "Celsius", "Temperature", "Kelvin")));
            var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());

            Assert.IsTrue(doc.RootElement.TryGetProperty("value",      out _));
            Assert.IsTrue(doc.RootElement.TryGetProperty("unit",       out _));
            Assert.IsTrue(doc.RootElement.TryGetProperty("expression", out _));
            Assert.AreEqual(273.15, doc.RootElement.GetProperty("value").GetDouble(), 1e-4,
                "0°C = 273.15 K.");
        }

        
        
        
        [TestMethod]
        public async Task TestHttpStatusCodes_SuccessfulCompare_Returns200()
        {
            Assert.AreEqual(HttpStatusCode.OK,
                (await _client.PostAsync("/api/quantity/compare",
                    Json.Body(ComparePayload(1, "Litre", 1000, "Millilitre", "Volume")))).StatusCode);
        }

        [TestMethod]
        public async Task TestHttpStatusCodes_SuccessfulAdd_Returns200()
        {
            Assert.AreEqual(HttpStatusCode.OK,
                (await _client.PostAsync("/api/quantity/add",
                    Json.Body(AddPayload(1, "Pound", 1, "Pound", "Weight", "Kilogram")))).StatusCode);
        }

        [TestMethod]
        public async Task TestHttpStatusCodes_SuccessfulConvert_Returns200()
        {
            Assert.AreEqual(HttpStatusCode.OK,
                (await _client.PostAsync("/api/quantity/convert",
                    Json.Body(ConvertPayload(1, "Gallon", "Volume", "Litre")))).StatusCode);
        }

        [TestMethod]
        public async Task TestHttpStatusCodes_SuccessfulHistory_Returns200()
        {
            Assert.AreEqual(HttpStatusCode.OK,
                (await _client.GetAsync("/api/quantity/history")).StatusCode);
        }

        
        
        
        [TestMethod]
        public async Task TestHttpStatusCodes_BadRequest_Returns400()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest,
                (await _client.PostAsync("/api/quantity/subtract",
                    new StringContent("{}", Encoding.UTF8, "application/json"))).StatusCode);
        }

        [TestMethod]
        public async Task TestHttpStatusCodes_NotFound_UnknownRoute_Returns404()
        {
            Assert.AreEqual(HttpStatusCode.NotFound,
                (await _client.GetAsync("/api/quantity/doesNotExist")).StatusCode);
        }

        
        
        [TestMethod]
        public async Task TestRestEndpointSubtractQuantities_Returns200WithCorrectResult()
        {
            var r   = await _client.PostAsync("/api/quantity/subtract",
                Json.Body(SubtractPayload(2, "Kilogram", 500, "Gram", "Weight", "Gram")));
            Assert.AreEqual(HttpStatusCode.OK, r.StatusCode);
            var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());
            Assert.AreEqual(1500.0, doc.RootElement.GetProperty("value").GetDouble(), 1e-6);
        }

        [TestMethod]
        public async Task TestRestEndpointDivideQuantities_Returns200WithRatio()
        {
            var r   = await _client.PostAsync("/api/quantity/divide",
                Json.Body(DividePayload(1, "Feet", 1, "Inches", "Length")));
            Assert.AreEqual(HttpStatusCode.OK, r.StatusCode);
            var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());
            Assert.AreEqual(12.0,   doc.RootElement.GetProperty("value").GetDouble(), 1e-6);
            Assert.AreEqual("ratio", doc.RootElement.GetProperty("unit").GetString());
        }

        [TestMethod]
        public async Task TestStatsEndpoint_Returns200WithTotalRecordsAndPoolInfo()
        {
            var r   = await _client.GetAsync("/api/quantity/stats");
            Assert.AreEqual(HttpStatusCode.OK, r.StatusCode);
            var doc = JsonDocument.Parse(await r.Content.ReadAsStringAsync());
            Assert.IsTrue(doc.RootElement.TryGetProperty("totalRecords",out _));
            Assert.IsTrue(doc.RootElement.TryGetProperty("poolInfo",out _));
        }

        [TestMethod]
        public async Task TestDeleteHistory_Returns200WithConfirmationMessage()
        {
            var r = await _client.DeleteAsync("/api/quantity/history");
            Assert.AreEqual(HttpStatusCode.OK, r.StatusCode);
            Assert.IsTrue((await r.Content.ReadAsStringAsync()).Contains("message"));
        }
    }
}