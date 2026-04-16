using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HistoryService.Repository;

namespace HistoryService.Controllers
{
    [Route("api/quantity")]
    [ApiController]
    [Authorize]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryRepository _repo;

        public HistoryController(IHistoryRepository repo) => _repo = repo;

        private int? GetUserId()
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                     ?? User.FindFirst("sub");
            return claim != null && int.TryParse(claim.Value, out var id) ? id : null;
        }

        /// <summary>Get all measurement history for the authenticated user.</summary>
        [HttpGet("history")]
        public IActionResult GetHistory()
        {
            var userId = GetUserId();
            return Ok(userId.HasValue ? _repo.GetAll(userId.Value) : _repo.GetAll());
        }

        /// <summary>Get history filtered by operation type (Compare, Add, Subtract, Divide, Convert).</summary>
        [HttpGet("history/operation/{operationType}")]
        public IActionResult GetHistoryByOperation(string operationType)
        {
            var userId = GetUserId();
            return Ok(userId.HasValue
                ? _repo.GetByOperation(operationType, userId.Value)
                : _repo.GetByOperation(operationType));
        }

        /// <summary>Get history filtered by measurement type (Length, Weight, Volume, Temperature).</summary>
        [HttpGet("history/type/{measurementType}")]
        public IActionResult GetHistoryByType(string measurementType)
        {
            var userId = GetUserId();
            return Ok(userId.HasValue
                ? _repo.GetByType(measurementType, userId.Value)
                : _repo.GetByType(measurementType));
        }

        /// <summary>Delete all history for the authenticated user.</summary>
        [HttpDelete("history")]
        public IActionResult DeleteHistory()
        {
            var userId = GetUserId();
            if (userId.HasValue)
                _repo.DeleteAll(userId.Value);
            else
                _repo.DeleteAll();
            return Ok(new { message = "History deleted." });
        }

        /// <summary>Get total record count and pool statistics.</summary>
        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            var userId    = GetUserId();
            int total     = userId.HasValue ? _repo.GetTotalCount(userId.Value) : _repo.GetTotalCount();
            int userTotal = userId.HasValue ? _repo.GetTotalCount(userId.Value) : 0;

            return Ok(new
            {
                totalRecords     = total,
                userRecords      = userTotal,
                poolInfo         = $"EF Core repository. Total measurements in DB: {_repo.GetTotalCount()}"
            });
        }
    }
}
