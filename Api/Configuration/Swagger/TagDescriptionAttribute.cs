namespace Api.Configuration.Swagger;

[AttributeUsage(AttributeTargets.Field)]
public class TagDescriptionAttribute(string descrption): Attribute
{
    public string Description { get; set; } = descrption;
}