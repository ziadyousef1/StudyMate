using System.Text.Json.Serialization;
using Serilog;
using StudyMate.DependancyInjection;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();



builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }
);
builder.Services.AddControllers();
builder.Services.AddOpenApi();


builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.WithTitle("StudyMate API")
        .WithTheme(ScalarTheme.Mars)
        .WithDefaultHttpClient(ScalarTarget.Dart, ScalarClient.HttpClient);


});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseApplicationServices();
app.Run();