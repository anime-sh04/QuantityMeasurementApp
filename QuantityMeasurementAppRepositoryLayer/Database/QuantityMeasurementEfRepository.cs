using QuantityMeasurementAppModelLayer.Models;
using QuantityMeasurementAppRepositoryLayer.Data;
using QuantityMeasurementAppRepositoryLayer.Interface;

namespace QuantityMeasurementAppRepositoryLayer.Database
{
    /// <summary>
    /// EF Core implementation of IQuantityMeasurementRepository.
    /// </summary>
    public class QuantityMeasurementEfRepository : IQuantityMeasurementRepository
    {
        private readonly AppDbContext _context;

        public QuantityMeasurementEfRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            _context.QuantityMeasurements.Add(entity);
            _context.SaveChanges();
        }

        public List<QuantityMeasurementEntity> GetAllMeasurements()
            => _context.QuantityMeasurements.OrderByDescending(e => e.CreatedAt).ToList();

        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType)
            => _context.QuantityMeasurements
                       .Where(e => e.OperationType == operationType)
                       .OrderByDescending(e => e.CreatedAt)
                       .ToList();

        public List<QuantityMeasurementEntity> GetMeasurementsByType(string measurementType)
            => _context.QuantityMeasurements
                       .Where(e => e.MeasurementType == measurementType)
                       .OrderByDescending(e => e.CreatedAt)
                       .ToList();

        public int GetTotalCount()
            => _context.QuantityMeasurements.Count();

        public void DeleteAll()
        {
            _context.QuantityMeasurements.RemoveRange(_context.QuantityMeasurements);
            _context.SaveChanges();
        }

        public string GetPoolStatistics()
            => $"EF Core repository. Total measurements: {_context.QuantityMeasurements.Count()}";
    }
}
