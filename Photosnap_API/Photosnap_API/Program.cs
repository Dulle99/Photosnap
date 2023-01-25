using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region ConnectingToDatabase

var client = new MongoClient(builder.Configuration.GetSection("MongoDBConnectionSettings:Server").Value);
var database = client.GetDatabase(builder.Configuration.GetSection("MongoDBConnectionSettings:Database").Value);
builder.Services.AddSingleton(database);

#endregion ConnectingToDatabase

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
