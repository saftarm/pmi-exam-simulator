using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using TestAPI.Data;
using TestAPI.Persistence.Implementation;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Implementation;
using TestAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using TestAPI.Entities;





var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(
    o => o.JsonSerializerOptions
    .ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));


builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    policy.WithOrigins("http://localhost:5173") // React dev server URL
    .AllowAnyHeader()
    .AllowAnyMethod());
});




builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IQuestionService, QuestionService>();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IDomainRepository, DomainRepository>();

builder.Services.AddScoped<IExamAttemptRepository, ExamAttemptRepository>();
builder.Services.AddScoped<IExamAttemptService, ExamAttemptService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDomainService, DomainService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));




builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));

builder.Services.AddAuthentication(options=> 

{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

} ).AddJwtBearer(options =>

{
    options.TokenValidationParameters = new TokenValidationParameters (){
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["AuthSettings:Issuer"],
        ValidAudience = builder.Configuration["AuthSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthSettings:SecretKey"]!))

    };
}


);





var app = builder.Build();

app.UseCors("ReactPolicy");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("/", () => Results.Redirect("/scalar"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
