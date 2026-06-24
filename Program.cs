using Scalar.AspNetCore;
using TestAPI.Extensions;

if (await SeedAdminCliExtensions.TryRunSeedAdminAsync(args))
{
    return;
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddSessionSnapshotStore(builder.Configuration, builder.Environment);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseCors("ReactPolicy");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("/", () => Results.Redirect("/scalar"));
}

app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
