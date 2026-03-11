namespace QuantityMeasurementApp;

public interface IMeasurable
{
    double GetConversionFactor();

    double ConvertToBaseUnit(double value);

    double ConvertFromBaseUnit(double baseValue);

    string GetUnitName();
}