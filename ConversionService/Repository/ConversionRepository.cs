using ConversionService.Data;
using Shared.Models;

namespace ConversionService.Repository
{
    public interface IConversionRepository
    {
        void Save(QuantityMeasurementEntity entity);
    }

    public class ConversionRepository : IConversionRepository
    {
        private readonly ConversionDbContext _context;

        public ConversionRepository(ConversionDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            _context.QuantityMeasurements.Add(entity);
            _context.SaveChanges();
        }
    }
}
