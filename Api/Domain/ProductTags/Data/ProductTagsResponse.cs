using Api.Data.Models;

namespace Api.Domain.ProductTags.Data;

public class ProductTagsResponse
{
    public required List<ProductTag> Tags { get; set; }
}