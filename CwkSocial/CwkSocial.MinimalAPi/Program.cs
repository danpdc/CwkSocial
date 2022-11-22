using AutoMapper;
using CwkSocial.MinimalAPi.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterServices();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/weatherforecasts", (IMapper mapper) =>
{
    return TypedResults.Ok();
});

app.Run();
