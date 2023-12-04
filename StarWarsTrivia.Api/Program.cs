using MongoDB.Driver;
using Serilog;
using StarWars.Api.GraphQL;
using StarWars.Application.DTOs;
using StarWars.Application.Interfaces;
using StarWars.Application.Services;
using StarWars.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog();

var ConnectionString = builder.Configuration.GetSection("MongoDbSettings:ConnectionString")!.Value;
var databaseName = builder.Configuration.GetSection("MongoDbSettings:DatabaseName")!.Value;
var collectionName = builder.Configuration.GetSection("MongoDbSettings:CollectionName")!.Value;


try
{
    var mongoClient = new MongoClient(ConnectionString);

    var database = mongoClient.GetDatabase(databaseName);
    var collection = database.GetCollection<CharacterDto>(collectionName);
    builder.Services.AddSingleton(database);

}
catch (MongoAuthenticationException authException)
{
    Console.WriteLine($"Authentication with MongoDB failed. Check credentials and configuration.,  {authException}");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while connecting to MongoDB. : {ex}");
}



// Add Serilog using the configuration method that suits your needs
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/StarWars-api.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Ensure to add Serilog in the 'ConfigureServices' method
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

// Adding D.I
builder.Services.AddScoped<IMongoDbService, MongoDbService>();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<IFilmService, FilmService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IStarWarsExternalService, StarWarsExternalService>();
builder.Services.AddHttpClient<IStarWarsExternalService, StarWarsExternalService>();
builder.Services.AddGraphQLServer()
        .AddType<CharacterType>()
        .AddQueryType<QueryType>();
builder.Services.AddScoped<CharacterType>();
builder.Services.AddScoped<QueryType>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGraphQL(path: "/graphql");
  

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
