using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppBusinessLayer.Service;
using QuantityMeasurementAppBusinessLayer.Util;
using QuantityMeasurementAppModelLayer.DTOs;
using QuantityMeasurementAppRepositoryLayer.Cache;
using QuantityMeasurementAppRepositoryLayer.Database;
using QuantityMeasurementAppRepositoryLayer.Interface;

// ── Config ────────────────────────────────────────────────────────────────
string repoType = "database";
string connectionString  = "Server=localhost\\SQLEXPRESS;Database=QuantityMeasurementDB;Trusted_Connection=True;TrustServerCertificate=True;";

// ── DI ────────────────────────────────────────────────────────────────────
var services = new ServiceCollection();
services.AddLogging(b => { b.AddConsole(); b.SetMinimumLevel(LogLevel.Warning); });

if (repoType.Equals("database", StringComparison.OrdinalIgnoreCase))
{
    services.AddSingleton<IQuantityMeasurementRepository>(sp =>
        new QuantityMeasurementDatabaseRepository(connectionString,
            sp.GetRequiredService<ILogger<QuantityMeasurementDatabaseRepository>>()));
}
else
{
    services.AddSingleton<IQuantityMeasurementRepository, QuantityMeasurementCacheRepository>();
}

services.AddSingleton<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();
var provider = services.BuildServiceProvider();
var svc      = provider.GetRequiredService<IQuantityMeasurementService>();

RunMenu(svc);
provider.GetRequiredService<IQuantityMeasurementRepository>().ReleaseResources();

// ══════════════════════════════════════════════════════════════════════════
static void RunMenu(IQuantityMeasurementService svc)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine($"""

  ╔═══════════════════════════════════════════════╗
  ║       Quantity Measurement App  (UC16)        ║
  ╠═══════════════════════════════════════════════╣
  ║  1.  Compare two quantities                   ║
  ║  2.  Add two quantities                       ║
  ║  3.  Subtract two quantities                  ║
  ║  4.  Divide two quantities                    ║
  ║  5.  Convert a quantity                       ║
  ║──────────────────────────────────────────────║
  ║  6.  View all history                         ║
  ║  7.  Filter history by operation              ║
  ║  8.  Filter history by measurement type       ║
  ║  9.  Stats & pool info                        ║
  ║  10. Delete all history                       ║
  ║──────────────────────────────────────────────║
  ║  0.  Exit                                     ║
  ╚═══════════════════════════════════════════════╝
""");

        Console.Write("  Enter choice: ");
        switch (Console.ReadLine()?.Trim())
        {
            case "1":  DoCompare(svc);    break;
            case "2":  DoAdd(svc);        break;
            case "3":  DoSubtract(svc);   break;
            case "4":  DoDivide(svc);     break;
            case "5":  DoConvert(svc);    break;
            case "6":  ShowAll(svc);      break;
            case "7":  FilterByOp(svc);   break;
            case "8":  FilterByType(svc); break;
            case "9":  ShowStats(svc);    break;
            case "10": DeleteAll(svc);    break;
            case "0":
                Console.WriteLine("\n  Goodbye!\n");
                return;
            default:
                Warn("Invalid choice. Press any key to try again.");
                Console.ReadKey(true);
                break;
        }
    }
}

// ── 1. Compare ────────────────────────────────────────────────────────────
static void DoCompare(IQuantityMeasurementService svc)
{
    Header("Compare Two Quantities");
    var (type, _) = PickType(allowTemp: true);

    var first  = PromptValue("  First",  type);
    var second = PromptValue("  Second", type);

    // For Compare we just compare, no target unit needed (result is Equal/Not Equal)
    try
    {
        bool equal = svc.Compare(first, second);

        Console.WriteLine();
        if (equal)
            Ok($"{first.Value} {first.Unit}  ==  {second.Value} {second.Unit}   →  ✔ EQUAL");
        else
            Warn2($"{first.Value} {first.Unit}  ≠   {second.Value} {second.Unit}   →  ✘ NOT EQUAL");
    }
    catch (Exception ex) { Warn(ex.Message); }

    Pause();
}

// ── 2. Add ────────────────────────────────────────────────────────────────
static void DoAdd(IQuantityMeasurementService svc)
{
    Header("Add Two Quantities");
    var (type, _) = PickType(allowTemp: false);

    var first  = PromptValue("  First",  type);
    var second = PromptValue("  Second", type);
    string target = PickTargetUnit(type, "  Result unit");

    try
    {
        var r = svc.Add(first, second, target);
        Ok($"{first.Value} {first.Unit}  +  {second.Value} {second.Unit}  =  {r.Value:G} {r.Unit}");
    }
    catch (Exception ex) { Warn(ex.Message); }

    Pause();
}

// ── 3. Subtract ───────────────────────────────────────────────────────────
static void DoSubtract(IQuantityMeasurementService svc)
{
    Header("Subtract Two Quantities");
    var (type, _) = PickType(allowTemp: false);

    var first  = PromptValue("  First",  type);
    var second = PromptValue("  Second", type);
    string target = PickTargetUnit(type, "  Result unit");

    try
    {
        var r = svc.Subtract(first, second, target);
        Ok($"{first.Value} {first.Unit}  −  {second.Value} {second.Unit}  =  {r.Value:G} {r.Unit}");
    }
    catch (Exception ex) { Warn(ex.Message); }

    Pause();
}

// ── 4. Divide ─────────────────────────────────────────────────────────────
static void DoDivide(IQuantityMeasurementService svc)
{
    Header("Divide Two Quantities");
    Console.WriteLine("  (Returns how many times First is bigger than Second)\n");
    var (type, _) = PickType(allowTemp: false);

    var first  = PromptValue("  First",  type);
    var second = PromptValue("  Second", type);

    try
    {
        var r = svc.Divide(first, second, "ratio");
        Ok($"{first.Value} {first.Unit}  ÷  {second.Value} {second.Unit}  =  {r.Value:G}× (ratio)");
    }
    catch (Exception ex) { Warn(ex.Message); }

    Pause();
}

// ── 5. Convert ────────────────────────────────────────────────────────────
static void DoConvert(IQuantityMeasurementService svc)
{
    Header("Convert a Quantity");
    var (type, _) = PickType(allowTemp: true);

    var source = PromptValue("  Value to convert", type);
    string target = PickTargetUnit(type, "  Convert to", exclude: source.Unit);

    try
    {
        var r = svc.Convert(source, target);
        Ok($"{source.Value} {source.Unit}  →  {r.Value:G} {r.Unit}");
    }
    catch (Exception ex) { Warn(ex.Message); }

    Pause();
}

// ── 6. Show All ───────────────────────────────────────────────────────────
static void ShowAll(IQuantityMeasurementService svc)
{
    Header("All Saved Measurements");
    var list = svc.GetHistory();
    if (list.Count == 0) { Console.WriteLine("  (no records yet)"); }
    else foreach (var e in list) Console.WriteLine($"  {e}");
    Pause();
}

// ── 7. Filter by operation ────────────────────────────────────────────────
static void FilterByOp(IQuantityMeasurementService svc)
{
    Header("Filter by Operation");
    string[] ops = ["Compare", "Add", "Subtract", "Divide", "Convert"];
    for (int i = 0; i < ops.Length; i++) Console.WriteLine($"  {i + 1}. {ops[i]}");
    Console.Write("\n  Choice: ");
    int idx = int.TryParse(Console.ReadLine(), out int v) ? v - 1 : 0;
    if (idx < 0 || idx >= ops.Length) idx = 0;
    string op = ops[idx];

    var list = svc.GetHistoryByOperation(op);
    Console.WriteLine($"\n  '{op}', {list.Count} record(s):");
    foreach (var e in list) Console.WriteLine($"  {e}");
    Pause();
}

// ── 8. Filter by type ─────────────────────────────────────────────────────
static void FilterByType(IQuantityMeasurementService svc)
{
    Header("Filter by Measurement Type");
    string[] types = ["Length", "Weight", "Volume", "Temperature"];
    for (int i = 0; i < types.Length; i++) Console.WriteLine($"  {i + 1}. {types[i]}");
    Console.Write("\n  Choice: ");
    int idx = int.TryParse(Console.ReadLine(), out int v) ? v - 1 : 0;
    if (idx < 0 || idx >= types.Length) idx = 0;
    string type = types[idx];

    var list = svc.GetHistoryByType(type);
    Console.WriteLine($"\n  '{type}', {list.Count} record(s):");
    foreach (var e in list) Console.WriteLine($"  {e}");
    Pause();
}

// ── 9. Stats ──────────────────────────────────────────────────────────────
static void ShowStats(IQuantityMeasurementService svc)
{
    Header("Stats & Pool Info");
    Console.WriteLine($"  Total records : {svc.GetTotalCount()}");
    Console.WriteLine($"  Pool info     : {svc.GetPoolStatistics()}");
    Pause();
}

// ── 10. Delete all ────────────────────────────────────────────────────────
static void DeleteAll(IQuantityMeasurementService svc)
{
    Header("Delete All History");
    Console.Write("  ⚠  Type YES to confirm deletion of ALL records: ");
    if (Console.ReadLine()?.Trim() == "YES")
    { svc.DeleteAllHistory(); Ok("All records deleted."); }
    else
    { Console.WriteLine("  Cancelled."); }
    Pause();
}

// ══════════════════════════════════════════════════════════════════════════
// ── Shared helpers ────────────────────────────────────────────────────────

static (string type, string[] units) PickType(bool allowTemp)
{
    string[] types = allowTemp
        ? ["Length", "Weight", "Volume", "Temperature"]
        : ["Length", "Weight", "Volume"];

    Console.WriteLine("  Measurement type:");
    for (int i = 0; i < types.Length; i++)
        Console.WriteLine($"    {i + 1}. {types[i]}");
    Console.Write("  Choice: ");

    int idx = int.TryParse(Console.ReadLine(), out int v) ? v - 1 : 0;
    if (idx < 0 || idx >= types.Length) idx = 0;
    string type  = types[idx];
    string[] units = UnitConverter.UnitsFor(type);
    return (type, units);
}

static QuantityDTO PromptValue(string label, string measurementType)
{
    string[] units = UnitConverter.UnitsFor(measurementType);
    Console.WriteLine($"\n{label}, {measurementType} unit:");
    for (int i = 0; i < units.Length; i++)
        Console.WriteLine($"    {i + 1}. {units[i]}");
    Console.Write("  Choice: ");
    int idx = int.TryParse(Console.ReadLine(), out int v) ? v - 1 : 0;
    if (idx < 0 || idx >= units.Length) idx = 0;

    Console.Write($"  Value ({units[idx]}): ");
    double val = double.TryParse(Console.ReadLine(), out double d) ? d : 0;
    return new QuantityDTO(val, units[idx], measurementType);
}

static string PickTargetUnit(string measurementType, string prompt, string? exclude = null)
{
    string[] units = UnitConverter.UnitsFor(measurementType)
                                  .Where(u => !u.Equals(exclude, StringComparison.OrdinalIgnoreCase))
                                  .ToArray();
    Console.WriteLine($"\n{prompt}, choose {measurementType} unit:");
    for (int i = 0; i < units.Length; i++)
        Console.WriteLine($"    {i + 1}. {units[i]}");
    Console.Write("  Choice: ");
    int idx = int.TryParse(Console.ReadLine(), out int v) ? v - 1 : 0;
    if (idx < 0 || idx >= units.Length) idx = 0;
    return units[idx];
}

static void Header(string title)
{
    Console.WriteLine();
    Console.WriteLine($"  ── {title} " + new string('─', Math.Max(0, 44 - title.Length)));
    Console.WriteLine();
}
static void Ok(string msg)    => Console.WriteLine($"\n  ✔  {msg}");
static void Warn(string msg)  => Console.WriteLine($"\n  ⚠  {msg}");
static void Warn2(string msg) => Console.WriteLine($"\n  ✘  {msg}");
static void Pause()
{
    Console.WriteLine("\n  Press any key to return to menu...");
    Console.ReadKey(true);
}
