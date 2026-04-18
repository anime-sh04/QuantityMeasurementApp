using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using QuantityMeasurementApp.Api.DTOs;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppModelLayer.DTOs;

namespace QuantityMeasurementApp.Api.Controller
{
    // [Authorize]
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
                var userId = GetUserId();
                bool equal = _service.Compare(Map(input.QuantityOne), Map(input.QuantityTwo), userId);
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
                var userId = GetUserId();
                var result = _service.Add(Map(input.QuantityOne), Map(input.QuantityTwo), input.TargetUnit, userId);
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
                var userId = GetUserId();
                var result = _service.Subtract(Map(input.QuantityOne), Map(input.QuantityTwo), input.TargetUnit, userId);
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
                var userId = GetUserId();
                var result = _service.Divide(Map(input.QuantityOne), Map(input.QuantityTwo), "ratio", userId);
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
                var userId = GetUserId();
                var result = _service.Convert(Map(input.Source), input.TargetUnit, userId);
                return Ok(new
                {
                    value      = result.Value,
                    unit       = result.Unit,
                    expression = $"{input.Source.Value} {input.Source.Unit} → {result.Value:G} {result.Unit}"
                });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        private int? GetUserId()
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                     ?? User.FindFirst("sub");
            return claim != null && int.TryParse(claim.Value, out var id) ? id : null;
        }

        [Authorize]
        [HttpGet("history")]
        public IActionResult GetHistory()
        {
            var userId = GetUserId();
            return userId.HasValue
                ? Ok(_service.GetHistory(userId.Value))
                : Ok(_service.GetHistory());
        }

        [Authorize]
        [HttpGet("history/operation/{operationType}")]
        public IActionResult GetHistoryByOperation(string operationType)
        {
            var userId = GetUserId();
            return userId.HasValue
                ? Ok(_service.GetHistoryByOperation(operationType, userId.Value))
                : Ok(_service.GetHistoryByOperation(operationType));
        }

        [Authorize]
        [HttpGet("history/type/{measurementType}")]
        public IActionResult GetHistoryByType(string measurementType)
        {
            var userId = GetUserId();
            return userId.HasValue
                ? Ok(_service.GetHistoryByType(measurementType, userId.Value))
                : Ok(_service.GetHistoryByType(measurementType));
        }

        [Authorize]
        [HttpDelete("history")]
        public IActionResult DeleteHistory()
        {
            var userId = GetUserId();
            if (userId.HasValue)
                _service.DeleteAllHistory(userId.Value);
            else
                _service.DeleteAllHistory();
            return Ok(new { message = "History deleted." });
        }

        [Authorize]
        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            var userId = GetUserId();
            return Ok(new
            {
                totalRecords = userId.HasValue ? _service.GetTotalCount(userId.Value) : _service.GetTotalCount(),
                poolInfo     = _service.GetPoolStatistics()
            });
        }
    }
}