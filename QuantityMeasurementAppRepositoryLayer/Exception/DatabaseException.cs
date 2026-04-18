namespace QuantityMeasurementAppRepositoryLayer.Exception
{
    public class DatabaseException : ApplicationException
    {
        public string Operation { get; }

        public DatabaseException(string message) : base(message)
        {
            Operation = "Unknown";
        }

        public DatabaseException(string message, string operation) : base(message)
        {
            Operation = operation;
        }

        public DatabaseException(string message, string operation, System.Exception innerException)
            : base(message, innerException)
        {
            Operation = operation;
        }

        public override string ToString()
            => $"DatabaseException [{Operation}]: {Message}" +
               (InnerException != null ? $"\nCaused by: {InnerException.Message}" : string.Empty);
    }
}
