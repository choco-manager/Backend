using Api.Data.Models;
using FastEndpoints;
using Riok.Mapperly.Abstractions;

namespace Api.Domain.Products.Data;

[Mapper]
public partial class ProductMapper
{
    public static partial ProductDto ProductToDto(Product product);
    private static string TagToString(ProductTag tag) => tag.Title;
}