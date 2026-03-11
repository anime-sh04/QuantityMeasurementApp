namespace QuantityMeasurementApp;

public interface IMeasurable
{
    double GetConversionFactor();

    double ConvertToBaseUnit(double value);

    double ConvertFromBaseUnit(double baseValue);

    string GetUnitName();

    // =============================
    // UC14: Arithmetic capability
    // =============================

    // Default: all units support arithmetic
    bool SupportsArithmetic()
    {
        return true;
    }

    // Default: allow operation
    void ValidateOperationSupport(string operation)
    {
        // Do nothing by default
    }
}