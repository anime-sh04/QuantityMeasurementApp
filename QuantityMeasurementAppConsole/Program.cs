using QuantityMeasurementApp.Console.Controller;
using QuantityMeasurementApp.Console.Menu;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementServices.Services;
using QuantityMeasurementAppRepositoryLayer.Cache;
using QuantityMeasurementAppRepositoryLayer.Interface;

IQuantityMeasurementRepository repository = new QuantityMeasurementCacheRepository();
IQuantityMeasurementService service = new QuantityMeasurementService(repository);
Menu menu = new Menu(service);

menu.Show();