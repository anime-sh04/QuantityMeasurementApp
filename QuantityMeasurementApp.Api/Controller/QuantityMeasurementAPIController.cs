using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.Api.DTOs;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppModelLayer.DTOs;

namespace QuantityMeasurementApp.Api.Controller
{
    [Route("api/quantity")]
    [ApiController]
    public class QuantityMeasurementAPIController : ControllerBase
    {
        private readonly IQuantityMeasurementService _service;

        public QuantityMeasurementAPIController(IQuantityMeasurementService service)
        {
            _service = service;
        }

        // ── Map API request → service DTO ─────────────────────────────────────
        private static QuantityDTO Map(QuantityRequest r) =>
            new(r.Value, r.Unit, r.MeasurementType);

        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Compare two quantities of the same type.
        /// No TargetUnit needed — result is Equal / Not Equal.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/quantity/compare
        ///     {
        ///       "quantityOne": { "value": 1, "unit": "Feet", "measurementType": "Length" },
        ///       "quantityTwo": { "value": 12, "unit": "Inches", "measurementType": "Length" }
        ///     }
        /// </remarks>
        [HttpPost("compare")]
        public IActionResult Compare([FromBody] CompareRequestDTO input)
        {
            try
            {
                bool equal = _service.Compare(Map(input.QuantityOne), Map(input.QuantityTwo));
                return Ok(new
                {
                    equal,
                    message    = equal ? "Quantities are EQUAL." : "Quantities are NOT equal.",
                    first      = $"{input.QuantityOne.Value} {input.QuantityOne.Unit}",
                    second     = $"{input.QuantityTwo.Value} {input.QuantityTwo.Unit}"
                });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        /// <summary>
        /// Add two quantities and return the result in the specified TargetUnit.
        /// Temperature is not supported.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/quantity/add
        ///     {
        ///       "quantityOne":  { "value": 1,   "unit": "Feet",   "measurementType": "Length" },
        ///       "quantityTwo":  { "value": 6,   "unit": "Inches", "measurementType": "Length" },
        ///       "targetUnit":   "Inches"
        ///     }
        /// </remarks>
        [HttpPost("add")]
        public IActionResult Add([FromBody] AddRequestDTO input)
        {
            try
            {
                var result = _service.Add(Map(input.QuantityOne), Map(input.QuantityTwo), input.TargetUnit);
                return Ok(new
                {
                    value      = result.Value,
                    unit       = result.Unit,
                    expression = $"{input.QuantityOne.Value} {input.QuantityOne.Unit} + {input.QuantityTwo.Value} {input.QuantityTwo.Unit} = {result.Value:G} {result.Unit}"
                });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        /// <summary>
        /// Subtract QuantityTwo from QuantityOne and return the result in TargetUnit.
        /// Temperature is not supported.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/quantity/subtract
        ///     {
        ///       "quantityOne":  { "value": 2,   "unit": "Kilogram", "measurementType": "Weight" },
        ///       "quantityTwo":  { "value": 500, "unit": "Gram",     "measurementType": "Weight" },
        ///       "targetUnit":   "Gram"
        ///     }
        /// </remarks>
        [HttpPost("subtract")]
        public IActionResult Subtract([FromBody] SubtractRequestDTO input)
        {
            try
            {
                var result = _service.Subtract(Map(input.QuantityOne), Map(input.QuantityTwo), input.TargetUnit);
                return Ok(new
                {
                    value      = result.Value,
                    unit       = result.Unit,
                    expression = $"{input.QuantityOne.Value} {input.QuantityOne.Unit} - {input.QuantityTwo.Value} {input.QuantityTwo.Unit} = {result.Value:G} {result.Unit}"
                });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        /// <summary>
        /// Divide QuantityOne by QuantityTwo. Returns a dimensionless ratio.
        /// No TargetUnit needed. Temperature is not supported.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/quantity/divide
        ///     {
        ///       "quantityOne": { "value": 1, "unit": "Gallon", "measurementType": "Volume" },
        ///       "quantityTwo": { "value": 1, "unit": "Litre",  "measurementType": "Volume" }
        ///     }
        /// </remarks>
        [HttpPost("divide")]
        public IActionResult Divide([FromBody] DivideRequestDTO input)
        {
            try
            {
                var result = _service.Divide(Map(input.QuantityOne), Map(input.QuantityTwo), "ratio");
                return Ok(new
                {
                    value      = result.Value,
                    unit       = result.Unit,
                    expression = $"{input.QuantityOne.Value} {input.QuantityOne.Unit} ÷ {input.QuantityTwo.Value} {input.QuantityTwo.Unit} = {result.Value:G} (ratio)"
                });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        /// <summary>
        /// Convert a quantity to a different unit within the same MeasurementType.
        /// Works for all types including Temperature.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/quantity/convert
        ///     {
        ///       "source":     { "value": 100, "unit": "Celsius", "measurementType": "Temperature" },
        ///       "targetUnit": "Fahrenheit"
        ///     }
        /// </remarks>
        [HttpPost("convert")]
        public IActionResult Convert([FromBody] ConvertRequestDTO input)
        {
            try
            {
                var result = _service.Convert(Map(input.Source), input.TargetUnit);
                return Ok(new
                {
                    value      = result.Value,
                    unit       = result.Unit,
                    expression = $"{input.Source.Value} {input.Source.Unit} → {result.Value:G} {result.Unit}"
                });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        // ── History endpoints ─────────────────────────────────────────────────

        /// <summary>Get full measurement history (all operations, all types).</summary>
        [HttpGet("history")]
        public IActionResult GetHistory()
            => Ok(_service.GetHistory());

        /// <summary>
        /// Filter history by operation type.
        /// Valid values: Compare | Add | Subtract | Divide | Convert
        /// </summary>
        [HttpGet("history/operation/{operationType}")]
        public IActionResult GetHistoryByOperation(string operationType)
            => Ok(_service.GetHistoryByOperation(operationType));

        /// <summary>
        /// Filter history by measurement type.
        /// Valid values: Length | Weight | Volume | Temperature
        /// </summary>
        [HttpGet("history/type/{measurementType}")]
        public IActionResult GetHistoryByType(string measurementType)
            => Ok(_service.GetHistoryByType(measurementType));

        /// <summary>Delete all history records.</summary>
        [HttpDelete("history")]
        public IActionResult DeleteHistory()
        {
            _service.DeleteAllHistory();
            return Ok(new { message = "All history deleted." });
        }

        /// <summary>Get total record count and repository info.</summary>
        [HttpGet("stats")]
        public IActionResult GetStats()
            => Ok(new
            {
                totalRecords = _service.GetTotalCount(),
                poolInfo     = _service.GetPoolStatistics()
            });
    }
}
