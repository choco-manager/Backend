using Swashbuckle.AspNetCore.Annotations;


namespace Backend.Modules.Cities.Contract;

[SwaggerSchema(Required = new[] { "Name" })]
public class CityRequestBody {
  [SwaggerSchema("Name of the city")]
  public string Name { get; set; }
}