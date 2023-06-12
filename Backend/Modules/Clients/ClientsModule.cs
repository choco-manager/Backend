// ------------------------------------------------------------------------
// Copyright (C) 2023 dadyarri
// This file is part of ChocoManager <https://github.com/choco-manager/Backend>.
// 
// ChocoManager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// ChocoManager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with ChocoManager.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------
// 


#region

using Backend.Data;
using Backend.Data.FakeDataGeneration;
using Backend.Exceptions;
using Backend.Modules.Addresses.Contract;
using Backend.Modules.Clients.Contract;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Serilog;

using SerilogTimings;

using Swashbuckle.AspNetCore.Annotations;

#endregion


namespace Backend.Modules.Clients;

public class ClientsModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    builder.AddSingleton<AbstractValidator<UpdateClientRequestBody>, UpdateClientRequestBodyValidator>();
    
    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    endpoints.MapGet("/api/clients", GetClients).WithTags("Clients API");
    endpoints.MapGet("/api/clients/{id:guid}", GetClientDetails).WithTags("Clients API");
    endpoints.MapPut("/api/clients/{id:guid}", UpdateClient).WithTags("Clients API");
    endpoints.MapPut("/api/clients/fake", GenerateFakeClients).WithTags("Clients API");
    endpoints.MapPost("/api/clients", CreateClient).WithTags("Clients API");

    return endpoints;
  }

  [SwaggerOperation(Summary = "Gets all available clients (with pagination)")]
  [SwaggerResponse(200, "Clients was returned successfully", typeof(List<ClientDto>))]
  private async Task<IResult> GetClients(
    [FromServices] ApplicationDbContext db,
    [FromServices] Mappers mappers,
    [FromQuery] int offset = 0,
    [FromQuery] int count = 5
  ) {
    using var op = Operation.Begin("Requesting clients");
    var addresses = await db.Clients
      .OrderBy(c => c.LastName)
      .Skip(offset)
      .Take(count)
      .Select(c => mappers.Cut(c))
      .ToListAsync();
    op.Complete();
    Log.Information("Fetched {Count} clients", addresses.Count);
    return TypedResults.Ok(addresses);
  }

  [SwaggerOperation(Summary = "Gets detail of client")]
  [SwaggerResponse(200, "Client was returned successfully", typeof(Client))]
  [SwaggerResponse(404, "Client was not found", typeof(ProblemDetails))]
  private async Task<IResult> GetClientDetails(
    [FromRoute] Guid id,
    [FromServices] ApplicationDbContext db
  ) {
    using var clientOp = Operation.Begin("Requesting client with Id = {Id}", id);
    var client = await db.Clients
      .Where(c => c.Id == id)
      .Include(c => c.Addresses)
      .ThenInclude(a => a.City)
      .FirstOrDefaultAsync();
    clientOp.Complete();

    if (client is null)
    {
      throw new EntityWasNotFoundException(nameof(Client), id);
    }

    Log.Information("Fetched client '{FirstName} {LastName}'", client.FirstName, client.LastName);
    return TypedResults.Ok(client);
  }

  [SwaggerOperation(Summary = "Creates client")]
  private async Task<IResult> CreateClient(
    [FromBody] UpdateClientRequestBody body,
    [FromServices] ApplicationDbContext db,
    [FromServices] AbstractValidator<UpdateClientRequestBody> validator
  ) {
    await validator.ValidateAndThrowAsync(body);

    var addresses = new List<Address>();

    foreach (var addressId in body.Addresses)
    {
      var address = await db.Addresses.FindAsync(addressId);

      if (address is not null)
      {
        addresses.Add(address);
      }
    }

    var client = new Client {
      FirstName = body.FirstName,
      LastName = body.LastName,
      ChatLink = body.ChatLink,
      PhoneNumber = body.PhoneNumber,
      Addresses = addresses,
    };

    await db.Clients.AddAsync(client);
    await db.SaveChangesAsync();

    return TypedResults.Created("/api/clients", client);
  }

  [SwaggerOperation(Summary = "Updates client")]
  [SwaggerResponse(200, "Client was updated successfully", typeof(Client))]
  [SwaggerResponse(404, "Client was not found", typeof(ProblemDetails))]
  private async Task<IResult> UpdateClient(
    [FromRoute] Guid id,
    [FromBody] UpdateClientRequestBody body,
    [FromServices] ApplicationDbContext db,
    [FromServices] Mappers mappers,
    [FromServices] AbstractValidator<UpdateClientRequestBody> validator
  ) {
    await validator.ValidateAndThrowAsync(body);
    
    using var clientOp = Operation.Begin("Requesting client with Id = {Id}", id);
    var client = await db.Clients
      .Where(c => c.Id == id)
      .Include(c => c.Addresses)
      .ThenInclude(a => a.City)
      .FirstOrDefaultAsync();
    clientOp.Complete();

    if (client is null)
    {
      throw new EntityWasNotFoundException(nameof(Client), id);
    }

    var shortenClient = mappers.CutToRb(client);

    var newAddressesList = CalculateDeltaOfAddresses(shortenClient.Addresses, body.Addresses);

    var addresses = new List<Address>();

    foreach (var newAddressId in newAddressesList)
    {
      var address = await db.Addresses.FindAsync(newAddressId);

      if (address is not null)
      {
        addresses.Add(address);
      }
    }

    client.FirstName = body.FirstName;
    client.LastName = body.LastName;
    client.ChatLink = body.ChatLink;
    client.PhoneNumber = body.PhoneNumber;
    client.Addresses = addresses;

    await db.SaveChangesAsync();

    return TypedResults.Ok(client);
  }

  [SwaggerOperation(Summary = "Generates fake clients")]
  [SwaggerResponse(201, "Clients was generated successfully", typeof(List<Client>))]
  private async Task<IResult> GenerateFakeClients([FromServices] ApplicationDbContext db) {
    var generator = new GenerateClients();
    var clients = await generator.Generate(db);
    return TypedResults.Created("/api/clients", clients);
  }

  public static List<Guid> CalculateDeltaOfAddresses(List<Guid> oldAddresses, List<Guid> newAddresses) {
    var secondHashSet = new HashSet<Guid>(newAddresses);
    var deltaList = oldAddresses.Where(item => secondHashSet.Contains(item)).ToList();
    deltaList.AddRange(newAddresses.Where(item => !oldAddresses.Contains(item)));

    return deltaList;
  }
}