namespace Api.Configuration.Swagger;

public class SwaggerTags
{
    [TagDescription("Operations, related to authentication & authorization")]
    public const string Auth = "Auth";
    
    [TagDescription("Operations, required for internal purposes, like health checking")]
    public const string Internal = "Internal";
    
    [TagDescription("Operations, related to products manipulation")]
    public const string Products = "Products";
}