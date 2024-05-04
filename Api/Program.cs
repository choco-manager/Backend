using FastEndpoints;
using FastEndpoints.Security;

var bld = WebApplication.CreateBuilder();
bld.Services
    .AddAuthenticationJwtBearer(s => { s.SigningKey = ""; })
    .AddAuthorization()
    .AddFastEndpoints();

var app = bld.Build();
app
    .UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints();
app.Run();