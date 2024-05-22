using Choco.Backend.Api.Data.Models;

namespace Choco.Backend.Api.Domain.ProductTags.Data;

public class ProductTagsResponse
{
    public required List<ProductTag> Tags { get; set; }
}