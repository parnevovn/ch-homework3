using Microsoft.Extensions.Options;
using Moq;
using Route256.PriceCalculator.Domain;
using Route256.PriceCalculator.Domain.Entities;
using Route256.PriceCalculator.Domain.Exceptions;
using Route256.PriceCalculator.Domain.Separated;
using Route256.PriceCalculator.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Workshop.UnitTests;

public class GoodPriceCalculatorServiceTests
{
    private static readonly List<GoodEntity> _entity = new()
    {
        new("Парик для питомца", 1, 100, 200, 300, 400,  new Random().Next(0, 10), 100),
        new("Накидка на телевизор", 2, 20, 10, 40, 40,  new Random().Next(0, 10), 120),
        new("Ковёр настенный", 3, 10, 10, 20, 3,  new Random().Next(0, 10), 140)
    };

    private static Mock<IStorageRepository> CreateRepositoryMock()
    {
        var repositoryMock = new Mock<IStorageRepository>(MockBehavior.Strict);
        repositoryMock.Setup(x => x.Save(It.IsAny<StorageEntity>()));
        return repositoryMock;
    }

    private static Mock<IGoodsRepository> CreateGoodsRepositoryMock(GoodEntity entity)
    {
        var repositoryMock = new Mock<IGoodsRepository>();
        repositoryMock
            .Setup(x => x.Get(entity.Id)).Returns(entity);

        return repositoryMock;
    }

    private static IOptionsSnapshot<PriceCalculatorOptions> CreateOptionsSnapshot(
        PriceCalculatorOptions options)
    {
        var repositoryMock = new Mock<IOptionsSnapshot<PriceCalculatorOptions>>(MockBehavior.Strict);
        
        repositoryMock
            .Setup(x => x.Value)
            .Returns(() => options);

        return repositoryMock.Object;
    }

    public static IEnumerable<object[]> GetGoodsMemberData => GetGoods();
    private static IEnumerable<object[]> GetGoods()
    {
        yield return new object[]
        {
            GoodPriceCalculatorServiceTests._entity.Where(x => x.Id == 1).First(), 1000, 6000000
        };

        yield return new object[]
        {
            GoodPriceCalculatorServiceTests._entity.Where(x => x.Id == 2).First(), 1, 8
        };

        yield return new object[]
        {
            GoodPriceCalculatorServiceTests._entity.Where(x => x.Id == 3).First(), 100, 200
        };
    }

    [Theory]
    [MemberData(nameof(GetGoodsMemberData))]
    public void GoodPriceCalculatorService_WhenCalcMany_ShouldSuccess(
        GoodEntity entity,
        int distance,
        decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            VolumeToPriceRatio = 1,
            WeightToPriceRatio = 1
        };
        var repositoryMock = CreateRepositoryMock();
        var goodsRepositoryMock = CreateGoodsRepositoryMock(entity);
        var pcs = new PriceCalculatorService(CreateOptionsSnapshot(options).Value, repositoryMock.Object);
        var goodPcs = new GoodPriceCalculatorService(goodsRepositoryMock.Object, pcs);

        // Act
        var result = goodPcs.CalculatePrice(entity.Id, distance);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GoodPriceCalculatorService_WhenIdIsEmpty_ShouldThrow()
    {
        var options = new PriceCalculatorOptions
        {
            VolumeToPriceRatio = 1,
            WeightToPriceRatio = 1
        };
        var repositoryMock = CreateRepositoryMock();
        var goodsRepositoryMock = CreateGoodsRepositoryMock(GoodPriceCalculatorServiceTests._entity.Where(x => x.Id == 1).First());
        var pcs = new PriceCalculatorService(CreateOptionsSnapshot(options).Value, repositoryMock.Object);
        var goodPcs = new GoodPriceCalculatorService(goodsRepositoryMock.Object, pcs);


        // Act, Assert
        Assert.Throws<DomainException>(() => goodPcs.CalculatePrice(0, 1000));
    }

    [Fact]
    public void GoodPriceCalculatorService_WhenDistanceIsEmpty_ShouldThrow()
    {
        var options = new PriceCalculatorOptions
        {
            VolumeToPriceRatio = 1,
            WeightToPriceRatio = 1
        };
        var repositoryMock = CreateRepositoryMock();
        var goodsRepositoryMock = CreateGoodsRepositoryMock(GoodPriceCalculatorServiceTests._entity.Where(x => x.Id == 1).First());
        var pcs = new PriceCalculatorService(CreateOptionsSnapshot(options).Value, repositoryMock.Object);
        var goodPcs = new GoodPriceCalculatorService(goodsRepositoryMock.Object, pcs);


        // Act, Assert
        Assert.Throws<DomainException>(() => goodPcs.CalculatePrice(1, 0));
    }
}