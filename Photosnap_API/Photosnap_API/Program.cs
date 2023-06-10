using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Photosnap_API.MongoIndexing;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region ConnectingToDatabase

var client = new MongoClient(builder.Configuration.GetSection("MongoDBConnectionSettings:Server").Value);
var database = client.GetDatabase(builder.Configuration.GetSection("MongoDBConnectionSettings:Database").Value);
MongoDbIndexing.InitializeIndexes(database);
builder.Services.AddSingleton(database);

#endregion ConnectingToDatabase

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Jwt";
    options.DefaultChallengeScheme = "Jwt";
}).AddJwtBearer("Jwt", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Lorem ipsum dolor sit amet, consectetur adipiscing elit")),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5) 
    };

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(configurePolicy =>
                configurePolicy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
