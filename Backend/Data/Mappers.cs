using Backend.Modules.Cities.Contract;

using Riok.Mapperly.Abstractions;


namespace Backend.Data;

[Mapper]
public partial class Mappers {
  public partial City Map(CreateCityRequestBody city);
}