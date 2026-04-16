using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ConversionService.DTOs;
using ConversionService.Services;

namespace ConversionService.Controllers
{
    [Route("api/quantity")]
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private readonly IConversionService _service;

        public ConversionController(IConversionService service) => _service = service;

        private static QuantityInput Map(QuantityRequest r) =>
            new(r.Value, r.Unit, r.MeasurementType);

        private int? GetUserId()
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                     ?? User.FindFirst("sub");
            return claim != null && int.TryParse(claim.Value, out var id) ? id : null;
        }

        /// <summary>Compare two quantities of the same type.</summary>
        [HttpPost("compare")]
        public IActionResult Compare([FromBody] CompareRequestDTO input)
        {
            bool equal = _service.Compare(Map(input.QuantityOne), Map(input.QuantityTwo), GetUserId());
            return Ok(new
            {
                equal,
                message = equal ? "Quantities are EQUAL." : "Quantities are NOT equal.",
                first   = $"{input.QuantityOne.Value} {input.QuantityOne.Unit}",
                second  = $"{input.QuantityTwo.Value} {input.QuantityTwo.Unit}"
            });
        }

        /// <summary>Add two quantities and return result in target unit.</summary>
        [HttpPost("add")]
        public IActionResult Add([FromBody] AddRequestDTO input)
        {
            var result = _service.Add(Map(input.QuantityOne), Map(input.QuantityTwo), input.TargetUnit, GetUserId());
            return Ok(new
            {
                value      = result.Value,
                unit       = result.Unit,
                expression = $"{input.QuantityOne.Value} {input.QuantityOne.Unit} + {input.QuantityTwo.Value} {input.QuantityTwo.Unit} = {result.Value:G} {result.Unit}"
            });
        }

        /// <summary>Subtract two quantities and return result in target unit.</summary>
        [HttpPost("subtract")]
        public IActionResult Subtract([FromBody] SubtractRequestDTO input)
        {
            var result = _service.Subtract(Map(input.QuantityOne), Map(input.QuantityTwo), input.TargetUnit, GetUserId());
            return Ok(new
            {
                value      = result.Value,
                unit       = result.Unit,
                expression = $"{input.QuantityOne.Value} {input.QuantityOne.Unit} - {input.QuantityTwo.Value} {input.QuantityTwo.Unit} = {result.Value:G} {result.Unit}"
            });
        }

        /// <summary>Divide two quantities and return a dimensionless ratio.</summary>
        [HttpPost("divide")]
        public IActionResult Divide([FromBody] DivideRequestDTO input)
        {
            var result = _service.Divide(Map(input.QuantityOne), Map(input.QuantityTwo), GetUserId());
            return Ok(new
            {
                value      = result.Value,
                unit       = result.Unit,
                expression = $"{input.QuantityOne.Value} {input.QuantityOne.Unit} ÷ {input.QuantityTwo.Value} {input.QuantityTwo.Unit} = {result.Value:G} (ratio)"
            });
        }

        /// <summary>Convert a quantity to a different unit of the same type.</summary>
        [HttpPost("convert")]
        public IActionResult Convert([FromBody] ConvertRequestDTO input)
        {
            var result = _service.Convert(Map(input.Source), input.TargetUnit, GetUserId());
            return Ok(new
            {
                value      = result.Value,
                unit       = result.Unit,
                expression = $"{input.Source.Value} {input.Source.Unit} → {result.Value:G} {result.Unit}"
            });
        }
    }
}
