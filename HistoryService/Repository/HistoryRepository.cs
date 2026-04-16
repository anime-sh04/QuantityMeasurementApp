using HistoryService.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace HistoryService.Repository
{
    public interface IHistoryRepository
    {
        // Global (admin)
        List<QuantityMeasurementEntity> GetAll();
        List<QuantityMeasurementEntity> GetByOperation(string operationType);
        List<QuantityMeasurementEntity> GetByType(string measurementType);
        int  GetTotalCount();
        void DeleteAll();

        // Per-user
        List<QuantityMeasurementEntity> GetAll(int userId);
        List<QuantityMeasurementEntity> GetByOperation(string operationType, int userId);
        List<QuantityMeasurementEntity> GetByType(string measurementType, int userId);
        int  GetTotalCount(int userId);
        void DeleteAll(int userId);
    }

    public class HistoryRepository : IHistoryRepository
    {
        private readonly HistoryDbContext _context;

        public HistoryRepository(HistoryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<QuantityMeasurementEntity> GetAll()
            => _context.QuantityMeasurements.OrderByDescending(e => e.CreatedAt).ToList();

        public List<QuantityMeasurementEntity> GetByOperation(string operationType)
            => _context.QuantityMeasurements
                       .Where(e => e.OperationType == operationType)
                       .OrderByDescending(e => e.CreatedAt).ToList();

        public List<QuantityMeasurementEntity> GetByType(string measurementType)
            => _context.QuantityMeasurements
                       .Where(e => e.MeasurementType == measurementType)
                       .OrderByDescending(e => e.CreatedAt).ToList();

        public int GetTotalCount()
            => _context.QuantityMeasurements.Count();

        public void DeleteAll()
        {
            _context.QuantityMeasurements.RemoveRange(_context.QuantityMeasurements);
            _context.SaveChanges();
        }

        // ── Per-user ──────────────────────────────────────────────────────────

        public List<QuantityMeasurementEntity> GetAll(int userId)
            => _context.QuantityMeasurements
                       .Where(e => e.UserId == userId)
                       .OrderByDescending(e => e.CreatedAt).ToList();

        public List<QuantityMeasurementEntity> GetByOperation(string operationType, int userId)
            => _context.QuantityMeasurements
                       .Where(e => e.OperationType == operationType && e.UserId == userId)
                       .OrderByDescending(e => e.CreatedAt).ToList();

        public List<QuantityMeasurementEntity> GetByType(string measurementType, int userId)
            => _context.QuantityMeasurements
                       .Where(e => e.MeasurementType == measurementType && e.UserId == userId)
                       .OrderByDescending(e => e.CreatedAt).ToList();

        public int GetTotalCount(int userId)
            => _context.QuantityMeasurements.Count(e => e.UserId == userId);

        public void DeleteAll(int userId)
        {
            var rows = _context.QuantityMeasurements.Where(e => e.UserId == userId);
            _context.QuantityMeasurements.RemoveRange(rows);
            _context.SaveChanges();
        }
    }
}
