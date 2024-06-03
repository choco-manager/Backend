namespace Choco.Backend.Api.Configuration.Swagger;

public class SwaggerTags
{
    [TagDescription("Operations, related to authentication & authorization")]
    public const string Auth = "Auth";

    [TagDescription("Operations, required for internal purposes, like health checking")]
    public const string Internal = "Internal";

    [TagDescription("Operations, related to products manipulation")]
    public const string Products = "Products";

    [TagDescription("Operations, related to product tags")]
    public const string ProductTags = "Product tags";

    [TagDescription("Operations, related to updating stock balance of products")]
    public const string Stocktaking = "Stocktaking";

    [TagDescription("Operations, related to orders manipulation")]
    public const string Orders = "Orders";

    [TagDescription("Operations, related to accessing notifications")]
    public const string Notifications = "Notifications";

    [TagDescription("Operations, related to customers manipulation")]
    public const string Customers = "Customers";

    [TagDescription("Operations, related to procurement manipulation")]
    public const string Procurements = "Procurements";
}