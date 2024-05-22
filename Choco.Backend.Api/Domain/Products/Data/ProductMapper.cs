using Choco.Backend.Api.Data.Enums;
using Choco.Backend.Api.Data.Models;
using Riok.Mapperly.Abstractions;

namespace Choco.Backend.Api.Domain.Products.Data;

[Mapper]
public partial class ProductMapper
{
    public static ProductDto ProductToDto(Product product, ICollection<PriceHistory> prices)
    {
        var dto = ProductToDto(product);

        dto.CostPrice = GetCostPrice(prices)!.Value;
        dto.RetailPrice = GetRetailPrice(prices)!.Value;

        return dto;
    }

    private static partial ProductDto ProductToDto(Product product);

    private static string TagToString(ProductTag tag) => tag.Title;

    private static decimal? GetCostPrice(ICollection<PriceHistory> prices) =>
        prices.FirstOrDefault(e => e.PriceType == PriceType.Cost)?.Price;

    private static decimal? GetRetailPrice(ICollection<PriceHistory> prices) =>
        prices.FirstOrDefault(e => e.PriceType == PriceType.Retail)?.Price;
}