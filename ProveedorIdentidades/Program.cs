using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddRazorPages();

//Configuración del servidor de identidades con IdentityServer4
//Acá se instancia la api, sus scopes y usuarios de prueba simulando la base de datos de credenciales.
builder.Services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryClients(
                [
                    new Client()
                    {
                        ClientId = "kA9qW2mL7xRpT4cH8jVzN1oY",
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        ClientSecrets = { new Secret("N3pGv1Kj9xRzQ2lT6yFbM8oW4sH7u".Sha256()) },
                        AllowedScopes = { "api1" }
                    }
                ])
                .AddInMemoryApiResources(
                [
                    new ApiResource("api1", "Api de autenticación c#")
                    {
                        Scopes = { "api1" },
                        UserClaims = { "name", "email", "role" }
                    }
                ])
                .AddInMemoryApiScopes(
                [
                    new ApiScope("api1", "MyApi")
                ])
                .AddTestUsers(
                [
                    new TestUser()
                    {
                        SubjectId = "1",
                        Username = "Diego",
                        Password = "diego123",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Diego Carrillo"),
                            new Claim(JwtClaimTypes.Email, "diego.carrillo@empresa.com"),
                            new Claim(JwtClaimTypes.Role, "administrador")
                        }
                    },
                    new TestUser()
                    {
                        SubjectId = "2",
                        Username = "Alexander",
                        Password = "alex987",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Alexander Oliver"),
                            new Claim(JwtClaimTypes.Email, "alexander.oliver@empresa.com"),
                            new Claim(JwtClaimTypes.Role, "soporte")
                        }
                    }
                ]);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseIdentityServer();

app.MapRazorPages();

app.UseEndpoints(ObjEndpoint =>
{
    _ = ObjEndpoint.MapControllers();
});

app.Run();
