using System.Data;
using Microsoft.Data.SqlClient;
using QuantityMeasurementAppModelLayer.Models;
using QuantityMeasurementAppRepositoryLayer.Interface;
using QuantityMeasurementAppRepositoryLayer.Exception;
using Microsoft.Extensions.Logging;

namespace QuantityMeasurementAppRepositoryLayer.Database
{
    
    
    
    
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<QuantityMeasurementDatabaseRepository> _logger;
        private int _activeConnections = 0;

        public QuantityMeasurementDatabaseRepository(
            string connectionString,
            ILogger<QuantityMeasurementDatabaseRepository> logger)
        {
            _connectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
            _logger = logger;
            _logger.LogInformation("DatabaseRepository initialized. Verifying schema...");
            EnsureSchemaExists();
        }

        

        private void EnsureSchemaExists()
        {
            const string createTableSql = @"
                IF NOT EXISTS (
                    SELECT * FROM sysobjects
                    WHERE name='QuantityMeasurementEntities' AND xtype='U'
                )
                BEGIN
                    CREATE TABLE QuantityMeasurementEntities (
                        Id              INT IDENTITY(1,1) PRIMARY KEY,
                        OperationType   NVARCHAR(50)  NOT NULL,
                        MeasurementType NVARCHAR(50)  NOT NULL,
                        FirstValue      FLOAT         NOT NULL,
                        FirstUnit       NVARCHAR(50)  NOT NULL,
                        SecondValue     FLOAT         NOT NULL,
                        SecondUnit      NVARCHAR(50)  NOT NULL,
                        Result          NVARCHAR(200) NOT NULL,
                        CreatedAt       DATETIME2     NOT NULL DEFAULT GETUTCDATE()
                    );
                    CREATE INDEX IX_QME_OperationType   ON QuantityMeasurementEntities(OperationType);
                    CREATE INDEX IX_QME_MeasurementType ON QuantityMeasurementEntities(MeasurementType);
                END";

            const string createHistorySql = @"
                IF NOT EXISTS (
                    SELECT * FROM sysobjects
                    WHERE name='QuantityMeasurementHistory' AND xtype='U'
                )
                BEGIN
                    CREATE TABLE QuantityMeasurementHistory (
                        HistoryId   INT IDENTITY(1,1) PRIMARY KEY,
                        EntityId    INT           NOT NULL,
                        ChangedAt   DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
                        ChangeType  NVARCHAR(20)  NOT NULL,
                        Description NVARCHAR(500)
                    );
                END";

            try
            {
                ExecuteNonQuery(createTableSql);
                ExecuteNonQuery(createHistorySql);
                _logger.LogInformation("Schema verified / created successfully.");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to create schema.");
                throw new DatabaseException("Schema creation failed.", "EnsureSchema", ex);
            }
        }

        

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            const string insertSql = @"
                INSERT INTO QuantityMeasurementEntities
                    (OperationType, MeasurementType, FirstValue, FirstUnit,
                     SecondValue, SecondUnit, Result, CreatedAt)
                OUTPUT INSERTED.Id
                VALUES
                    (@OperationType, @MeasurementType, @FirstValue, @FirstUnit,
                     @SecondValue, @SecondUnit, @Result, @CreatedAt)";

            try
            {
                using var conn = OpenConnection();
                using var cmd  = new SqlCommand(insertSql, conn);
                AddEntityParameters(cmd, entity);
                entity.Id = Convert.ToInt32(cmd.ExecuteScalar());
                _logger.LogInformation("Saved entity Id={Id} ({Op}/{Type}).", entity.Id, entity.OperationType, entity.MeasurementType);
                LogHistory(conn, entity.Id, "INSERT", $"Saved {entity.OperationType} for {entity.MeasurementType}");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error during Save.");
                throw new DatabaseException("Failed to save entity.", "Save", ex);
            }
        }

        

        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            const string sql = @"
                SELECT Id, OperationType, MeasurementType,
                       FirstValue, FirstUnit, SecondValue, SecondUnit, Result, CreatedAt
                FROM QuantityMeasurementEntities
                ORDER BY CreatedAt DESC";
            try { return ExecuteQuery(sql); }
            catch (SqlException ex) { throw new DatabaseException("Failed to retrieve all.", "GetAll", ex); }
        }

        

        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType)
        {
            const string sql = @"
                SELECT Id, OperationType, MeasurementType,
                       FirstValue, FirstUnit, SecondValue, SecondUnit, Result, CreatedAt
                FROM QuantityMeasurementEntities
                WHERE OperationType = @OperationType
                ORDER BY CreatedAt DESC";
            try
            {
                using var conn = OpenConnection();
                using var cmd  = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@OperationType", operationType);
                return ReadEntities(cmd);
            }
            catch (SqlException ex) { throw new DatabaseException($"Failed to retrieve by operation '{operationType}'.", "GetByOperation", ex); }
        }

        

        public List<QuantityMeasurementEntity> GetMeasurementsByType(string measurementType)
        {
            const string sql = @"
                SELECT Id, OperationType, MeasurementType,
                       FirstValue, FirstUnit, SecondValue, SecondUnit, Result, CreatedAt
                FROM QuantityMeasurementEntities
                WHERE MeasurementType = @MeasurementType
                ORDER BY CreatedAt DESC";
            try
            {
                using var conn = OpenConnection();
                using var cmd  = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MeasurementType", measurementType);
                return ReadEntities(cmd);
            }
            catch (SqlException ex) { throw new DatabaseException($"Failed to retrieve by type '{measurementType}'.", "GetByType", ex); }
        }

        

        public int GetTotalCount()
        {
            const string sql = "SELECT COUNT(*) FROM QuantityMeasurementEntities";
            try
            {
                using var conn = OpenConnection();
                using var cmd  = new SqlCommand(sql, conn);
                return (int)cmd.ExecuteScalar()!;
            }
            catch (SqlException ex) { throw new DatabaseException("Failed to get count.", "GetTotalCount", ex); }
        }

        

        public void DeleteAll()
        {
            try
            {
                ExecuteNonQuery("DELETE FROM QuantityMeasurementHistory");
                ExecuteNonQuery("DELETE FROM QuantityMeasurementEntities");
                _logger.LogInformation("All measurements deleted.");
            }
            catch (SqlException ex) { throw new DatabaseException("Failed to delete all.", "DeleteAll", ex); }
        }

        

        public string GetPoolStatistics()
            => $"Active connections (approx): {_activeConnections} | " +
               $"ADO.NET pooling active for: {MaskConnectionString(_connectionString)}";

        public void ReleaseResources()
        {
            SqlConnection.ClearAllPools();
            _logger.LogInformation("All SQL connection pools cleared.");
        }

        

        private SqlConnection OpenConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            Interlocked.Increment(ref _activeConnections);
            conn.Disposed += (_, _) => Interlocked.Decrement(ref _activeConnections);
            return conn;
        }

        private void ExecuteNonQuery(string sql)
        {
            using var conn = OpenConnection();
            using var cmd  = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        private List<QuantityMeasurementEntity> ExecuteQuery(string sql)
        {
            using var conn = OpenConnection();
            using var cmd  = new SqlCommand(sql, conn);
            return ReadEntities(cmd);
        }

        private static List<QuantityMeasurementEntity> ReadEntities(SqlCommand cmd)
        {
            var results = new List<QuantityMeasurementEntity>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) results.Add(MapEntity(reader));
            return results;
        }

        private static QuantityMeasurementEntity MapEntity(IDataRecord r) => new()
        {
            Id              = r.GetInt32(r.GetOrdinal("Id")),
            OperationType   = r.GetString(r.GetOrdinal("OperationType")),
            MeasurementType = r.GetString(r.GetOrdinal("MeasurementType")),
            FirstValue      = r.GetDouble(r.GetOrdinal("FirstValue")),
            FirstUnit       = r.GetString(r.GetOrdinal("FirstUnit")),
            SecondValue     = r.GetDouble(r.GetOrdinal("SecondValue")),
            SecondUnit      = r.GetString(r.GetOrdinal("SecondUnit")),
            Result          = r.GetString(r.GetOrdinal("Result")),
            CreatedAt       = r.GetDateTime(r.GetOrdinal("CreatedAt"))
        };

        private static void AddEntityParameters(SqlCommand cmd, QuantityMeasurementEntity e)
        {
            cmd.Parameters.AddWithValue("@OperationType",   e.OperationType);
            cmd.Parameters.AddWithValue("@MeasurementType", e.MeasurementType);
            cmd.Parameters.AddWithValue("@FirstValue",      e.FirstValue);
            cmd.Parameters.AddWithValue("@FirstUnit",       e.FirstUnit);
            cmd.Parameters.AddWithValue("@SecondValue",     e.SecondValue);
            cmd.Parameters.AddWithValue("@SecondUnit",      e.SecondUnit);
            cmd.Parameters.AddWithValue("@Result",          e.Result);
            cmd.Parameters.AddWithValue("@CreatedAt",       e.CreatedAt);
        }

        private void LogHistory(SqlConnection conn, int entityId, string changeType, string description)
        {
            const string sql = @"
                INSERT INTO QuantityMeasurementHistory (EntityId, ChangeType, Description, ChangedAt)
                VALUES (@EntityId, @ChangeType, @Description, @ChangedAt)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@EntityId",    entityId);
            cmd.Parameters.AddWithValue("@ChangeType",  changeType);
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@ChangedAt",   DateTime.UtcNow);
            cmd.ExecuteNonQuery();
        }

        private static string MaskConnectionString(string cs)
        {
            var builder = new SqlConnectionStringBuilder(cs);
            if (!string.IsNullOrEmpty(builder.Password)) builder.Password = "***";
            return builder.ConnectionString;
        }
    }
}
