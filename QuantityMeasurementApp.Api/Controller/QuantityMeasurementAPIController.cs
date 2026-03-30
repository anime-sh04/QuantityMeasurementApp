using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using QuantityMeasurementApp.Api.DTOs;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppModelLayer.DTOs;

namespace QuantityMeasurementApp.Api.Controller
{
    [Authorize]
    [Route("api/quantity")]
    [ApiController]
    public class QuantityMeasurementAPIController : ControllerBase
    {
        private readonly IQuantityMeasurementService _service;

        public QuantityMeasurementAPIController(IQuantityMeasurementService service)
        {
            _service = service;
        }

        
        private static QuantityDTO Map(QuantityRequest r) =>
            new(r.Value, r.Unit, r.MeasurementType);

        
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

        [HttpGet("history")]
        public IActionResult GetHistory()
            => Ok(_service.GetHistory());


        [HttpGet("history/operation/{operationType}")]
        public IActionResult GetHistoryByOperation(string operationType)
            => Ok(_service.GetHistoryByOperation(operationType));

        
        [HttpGet("history/type/{measurementType}")]
        public IActionResult GetHistoryByType(string measurementType)
            => Ok(_service.GetHistoryByType(measurementType));

        
        [HttpDelete("history")]
        public IActionResult DeleteHistory()
        {
            _service.DeleteAllHistory();
            return Ok(new { message = "All history deleted." });
        }

        
        [HttpGet("stats")]
        public IActionResult GetStats()
            => Ok(new
            {
                totalRecords = _service.GetTotalCount(),
                poolInfo     = _service.GetPoolStatistics()
            });
    }
}
