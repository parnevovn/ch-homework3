using Route256.PriceCalculator.Domain.Entities;
using Route256.PriceCalculator.Domain.Exceptions;
using Route256.PriceCalculator.Domain.Models.PriceCalculator;
using Route256.PriceCalculator.Domain.Separated;
using Route256.PriceCalculator.Domain.Services.Interfaces;
using Route256.PriceCalculator.Domain.Validators;

namespace Route256.PriceCalculator.Domain.Services;

internal sealed class GoodPriceCalculatorService : IGoodPriceCalculatorService
{
    private readonly IGoodsRepository _repository;
    private readonly IPriceCalculatorService _service;

    public GoodPriceCalculatorService(
        IGoodsRepository repository,
        IPriceCalculatorService service)
    {
        _repository = repository;
        _service = service;
    }

    public decimal CalculatePrice(
        int goodId,
        decimal distance)
    {
        try
        {
            return CalculatePriceUnsave(goodId, distance);
        }
        catch (ArgumentException ex)
        {
            throw new DomainException(ex.ToString());
        }
        catch (DivideByZeroException ex)
        {
            throw new DomainException("You can not to divide by zero", ex);
        }
    }

    public decimal CalculatePriceUnsave(
        int goodId, 
        decimal distance)
    {
        CheckAttributes(goodId, distance);

        var good = _repository.Get(goodId);
        var model = EntityToModel(good);
        var priceTotal = _service.CalculatePrice(new[] { model }) * distance;

        return priceTotal;
    }

    private static GoodModel EntityToModel(GoodEntity good)
    {
        return new(good.Height, good.Length, good.Width, good.Weight);
    }

    private static void CheckAttributes(
        int goodId,
        decimal distance)
    {
        if (goodId == default)
            throw new ArgumentException($"{nameof(goodId)} is default");

        if (distance == default)
            throw new ArgumentException($"{nameof(distance)} is default");
    }
}