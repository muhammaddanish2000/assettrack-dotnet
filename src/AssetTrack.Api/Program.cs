using System.Text.Json.Serialization;
using AssetTrack.Api.Data;
using AssetTrack.Api.Models;
using AssetTrack.Api.Services;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder =
    WebApplication.CreateBuilder(args);


builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions
            .Converters
            .Add(
                new JsonStringEnumConverter()
            );
    });


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AssetDbContext>(
    options =>
    {
        string connectionString =
            builder.Configuration
                .GetConnectionString(
                    "DefaultConnection"
                )
            ??
            "Data Source=assettrack.db";

        options.UseSqlite(
            connectionString
        );
    }
);


builder.Services.AddScoped<AssetService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "Frontend",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});


WebApplication app =
    builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("Frontend");

app.UseHttpsRedirection();

app.MapControllers();


app.MapGet(
    "/api/health",
    () => Results.Ok(
        new
        {
            application =
                "AssetTrack",

            status =
                "running",

            version =
                "1.0.0",

            timestamp =
                DateTime.UtcNow
        }
    )
);


using (
    IServiceScope scope =
        app.Services.CreateScope()
)
{
    AssetDbContext db =
        scope.ServiceProvider
            .GetRequiredService<
                AssetDbContext
            >();

    db.Database.EnsureCreated();

    if (!db.Assets.Any())
    {
        DateTime now =
            DateTime.UtcNow;

        db.Assets.AddRange(
            new Asset
            {
                Name =
                    "MacBook Pro",

                AssetTag =
                    "IT-0001",

                Category =
                    "Laptop",

                AssignedTo =
                    "Web Development",

                Location =
                    "Main Office",

                PurchaseDate =
                    new DateOnly(
                        2025,
                        1,
                        15
                    ),

                PurchasePrice =
                    6999,

                Status =
                    AssetStatus.Assigned,

                Notes =
                    "Primary development laptop.",

                CreatedAt =
                    now,

                UpdatedAt =
                    now
            },

            new Asset
            {
                Name =
                    "Dell 24-inch Monitor",

                AssetTag =
                    "IT-0002",

                Category =
                    "Monitor",

                AssignedTo =
                    "Web Development",

                Location =
                    "Main Office",

                PurchaseDate =
                    new DateOnly(
                        2025,
                        3,
                        10
                    ),

                PurchasePrice =
                    850,

                Status =
                    AssetStatus.Assigned,

                Notes =
                    "External development monitor.",

                CreatedAt =
                    now,

                UpdatedAt =
                    now
            },

            new Asset
            {
                Name =
                    "Lenovo ThinkPad",

                AssetTag =
                    "IT-0003",

                Category =
                    "Laptop",

                AssignedTo =
                    "",

                Location =
                    "IT Store",

                PurchaseDate =
                    new DateOnly(
                        2024,
                        8,
                        20
                    ),

                PurchasePrice =
                    4200,

                Status =
                    AssetStatus.Available,

                Notes =
                    "Available replacement laptop.",

                CreatedAt =
                    now,

                UpdatedAt =
                    now
            },

            new Asset
            {
                Name =
                    "Epson Projector",

                AssetTag =
                    "IT-0004",

                Category =
                    "Projector",

                AssignedTo =
                    "Meeting Room",

                Location =
                    "Meeting Room A",

                PurchasePrice =
                    2800,

                Status =
                    AssetStatus.Maintenance,

                Notes =
                    "Lamp inspection required.",

                CreatedAt =
                    now,

                UpdatedAt =
                    now
            }
        );

        db.SaveChanges();
    }
}


app.Run();


public partial class Program
{
}
