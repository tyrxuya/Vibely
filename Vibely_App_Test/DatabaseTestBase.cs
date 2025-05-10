using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Vibely_App.Data; // Adjust if your DbContext is elsewhere
using System;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace Vibely_App_Test;

[TestFixture] // Base class needs this for OneTimeSetUp/TearDown to be discovered
public abstract class DatabaseTestBase // Abstract to prevent instantiation
{
    protected PostgreSqlContainer _dbContainer;
    protected string _connectionString;

    // Helper method to create DbContextOptions for tests
    protected DbContextOptions<VibelyDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<VibelyDbContext>()
            .UseNpgsql(_connectionString)
            .Options;
    }

    [OneTimeSetUp] // Runs once before all tests in any derived fixture
    public async Task OneTimeSetUp()
    {
        Console.WriteLine("[DatabaseTestBase] Starting PostgreSQL container...");
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            // Using a single DB for all tests inheriting this base for simplicity.
            // Can be overridden or parameterized if separate DBs per fixture are strictly needed.
            .WithDatabase("vibely_integration_test_db") 
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .WithCleanUp(true)
            .Build();

        await _dbContainer.StartAsync();
        _connectionString = _dbContainer.GetConnectionString();
        Console.WriteLine($"[DatabaseTestBase] PostgreSQL container started. ConnectionString: {_connectionString}");

        // Apply migrations once for the fixture
        Console.WriteLine("[DatabaseTestBase] Applying migrations...");
        try
        {
            await using var migrationContext = new VibelyDbContext(CreateNewContextOptions());
            await migrationContext.Database.MigrateAsync();
            Console.WriteLine("[DatabaseTestBase] Migrations applied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DatabaseTestBase] Error applying migrations: {ex}");
            throw;
        }
    }

    [OneTimeTearDown] // Runs once after all tests in all derived fixtures
    public async Task OneTimeTearDown()
    {
        Console.WriteLine("[DatabaseTestBase] Stopping PostgreSQL container...");
        if (_dbContainer != null)
        {
            await _dbContainer.StopAsync();
            await _dbContainer.DisposeAsync();
        }
        Console.WriteLine("[DatabaseTestBase] PostgreSQL container stopped.");
    }
} 