using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Vibely_App.Data;
using Vibely_App.Business;
using Vibely_App.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Vibely_App_Test;

// Inherit from the base class, remove [TestFixture] attribute here
public class UserBusinessTests : DatabaseTestBase 
{
    // --- Test Helper Methods --- (Keep helpers specific to User tests)
    private User CreateValidUser(int id, string usernameSuffix = "")
    {
        // Creates a user instance with valid default data based on constraints
        return new User
        {
            Id = id,
            Username = $"testuser{usernameSuffix}{id}",
            Password = "hashed_password", // Assuming password hashing is done elsewhere or test with known hash
            Email = $"test{usernameSuffix}{id}@example.com",
            FirstName = "Test",
            LastName = $"User{id}",
            PhoneNumber = "1234567890",
            ProfilePicture = new byte[] { 0x01, 0x02 }, // Placeholder image data
            IsPremium = false,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
            SubscriptionPrice = 0
        };
    }

    // --- Tests --- 

    [Test]
    public async Task Add_ShouldPersistUser()
    {
        // Arrange
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var userBusiness = new UserBusiness(context);
        var newUser = CreateValidUser(1);

        // Act
        userBusiness.Add(newUser);
        // Add calls SaveChanges internally

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var addedUser = await assertContext.Users.FindAsync(newUser.Id);
        addedUser.Should().NotBeNull();
        addedUser.Should().BeEquivalentTo(newUser, options => 
            options.Excluding(m => m.Name == "UserPlaylist")
                   .Excluding(m => m.Name == "Songs")
        );
    }

    [Test]
    public async Task Add_WhenUserExists_ShouldNotAddDuplicate()
    {
        // Arrange
        var existingUser = CreateValidUser(2);
        // Add initial user
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
             setupContext.Users.Add(existingUser);
             await setupContext.SaveChangesAsync();
        }
        
        // Create context for the test action
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var userBusiness = new UserBusiness(context);
        var duplicateUser = CreateValidUser(2); // Same ID

        // Act
        userBusiness.Add(duplicateUser); // Should do nothing as Find(id) will return existing

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var users = await assertContext.Users.Where(u => u.Id == existingUser.Id).ToListAsync();
        users.Should().HaveCount(1); // Only the original user should exist
    }

    [Test]
    public async Task GetAll_ShouldReturnAllAddedUsers()
    {
        // --- Clean slate before test --- 
        await using (var cleanupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            // Simple approach: Remove all users. Might need adjustment for foreign keys later.
            var allUsers = await cleanupContext.Users.ToListAsync();
            cleanupContext.Users.RemoveRange(allUsers);
            await cleanupContext.SaveChangesAsync();
        }
        // ----------------------------

        // Arrange
        var user1 = CreateValidUser(3);
        var user2 = CreateValidUser(4);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
             setupContext.Users.AddRange(user1, user2);
             await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var userBusiness = new UserBusiness(context);

        // Act
        var users = userBusiness.GetAll();

        // Assert
        users.Should().NotBeNull();
        users.Should().HaveCount(2);
        users.Should().ContainEquivalentOf(user1, options => 
            options.Excluding(m => m.Name == "UserPlaylist")
                   .Excluding(m => m.Name == "Songs")
        );
        users.Should().ContainEquivalentOf(user2, options => 
            options.Excluding(m => m.Name == "UserPlaylist")
                   .Excluding(m => m.Name == "Songs")
        );
    }

    [Test]
    public async Task Find_WithExistingId_ShouldReturnUser()
    {
        // --- Clean slate before test --- 
        await using (var cleanupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            var allUsers = await cleanupContext.Users.ToListAsync();
            cleanupContext.Users.RemoveRange(allUsers);
            await cleanupContext.SaveChangesAsync();
        }
        // ----------------------------
        
        // Arrange
        var user = CreateValidUser(5);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
             setupContext.Users.Add(user);
             await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var userBusiness = new UserBusiness(context);

        // Act
        var foundUser = userBusiness.Find(user.Id);

        // Assert
        foundUser.Should().NotBeNull();
        foundUser.Should().BeEquivalentTo(user, options => 
            options.Excluding(m => m.Name == "UserPlaylist")
                   .Excluding(m => m.Name == "Songs")
        );
    }

    [Test]
    public async Task Find_WithNonExistingId_ShouldReturnNull()
    {
        // --- Clean slate before test --- 
        await using (var cleanupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            var allUsers = await cleanupContext.Users.ToListAsync();
            cleanupContext.Users.RemoveRange(allUsers);
            await cleanupContext.SaveChangesAsync();
        }
        // ----------------------------

        // Arrange
        // Using statement ensures context is disposed even without TearDown
        await using var context = new VibelyDbContext(CreateNewContextOptions()); 
        var userBusiness = new UserBusiness(context);

        // Act
        var foundUser = userBusiness.Find(999); // ID that shouldn't exist

        // Assert
        foundUser.Should().BeNull();
    }

    [Test]
    public async Task Remove_WithExistingId_ShouldDeleteUser()
    {
        // Arrange
        var user = CreateValidUser(6);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
             setupContext.Users.Add(user);
             await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var userBusiness = new UserBusiness(context);

        // Act
        userBusiness.Remove(user.Id);
        // Remove calls SaveChanges internally

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var deletedUser = await assertContext.Users.FindAsync(user.Id);
        deletedUser.Should().BeNull();
    }

    [Test]
    public async Task Remove_WithNonExistingId_ShouldDoNothing()
    {
        // Arrange
        var user = CreateValidUser(7);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
             setupContext.Users.Add(user);
             await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var userBusiness = new UserBusiness(context);
        int initialCount = context.Users.Count();

        // Act
        userBusiness.Remove(998); // Non-existent ID

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        assertContext.Users.Count().Should().Be(initialCount);
        assertContext.Users.Any(u => u.Id == user.Id).Should().BeTrue(); // Ensure existing user wasn't deleted
    }

    [Test]
    public async Task Update_ShouldModifyUser()
    {
        // Arrange
        var originalUser = CreateValidUser(8);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
             setupContext.Users.Add(originalUser);
             await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var userBusiness = new UserBusiness(context);
        
        // Create updated user data (note: Update method takes the *new* user data)
        var updatedUserData = CreateValidUser(8); // Start with same ID
        updatedUserData.FirstName = "UpdatedFirstName";
        updatedUserData.LastName = "UpdatedLastName";
        updatedUserData.IsPremium = true;
        // IMPORTANT: The original Update method copies PhoneNumber from Username, which seems wrong.
        // We'll test the behaviour as written. Ideally, this should be fixed in UserBusiness.
        // updatedUserData.PhoneNumber = "9876543210"; // This won't be applied due to bug
        updatedUserData.Username = "updatedUsername8"; // This *will* be copied to PhoneNumber

        // Act
        userBusiness.Update(originalUser.Id, updatedUserData);
        // Update calls SaveChanges internally

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var updatedUser = await assertContext.Users.FindAsync(originalUser.Id);
        updatedUser.Should().NotBeNull();
        updatedUser.FirstName.Should().Be("UpdatedFirstName");
        updatedUser.LastName.Should().Be("UpdatedLastName");
        updatedUser.IsPremium.Should().BeTrue();
        updatedUser.Username.Should().Be(updatedUserData.Username);
        // Verify the potential bug: PhoneNumber should match the updated Username
        updatedUser.PhoneNumber.Should().Be(updatedUserData.Username);
    }

    [Test]
    public async Task IsUsernameTaken_WithExistingUsername_ShouldReturnTrue()
    {
         // Arrange
        var user = CreateValidUser(9, "taken");
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
             setupContext.Users.Add(user);
             await setupContext.SaveChangesAsync();
        }
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var userBusiness = new UserBusiness(context);

        // Act
        bool isTaken = userBusiness.IsUsernameTaken(user.Username);

        // Assert
        isTaken.Should().BeTrue();
    }

     [Test]
    public async Task IsUsernameTaken_WithNonExistingUsername_ShouldReturnFalse()
    {
        // --- Clean slate before test --- 
        await using (var cleanupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            var allUsers = await cleanupContext.Users.ToListAsync();
            cleanupContext.Users.RemoveRange(allUsers);
            await cleanupContext.SaveChangesAsync();
        }
        // ----------------------------

        // Arrange
        // Using statement ensures context is disposed even without TearDown
        await using var context = new VibelyDbContext(CreateNewContextOptions()); 
        var userBusiness = new UserBusiness(context);

        // Act
        bool isTaken = userBusiness.IsUsernameTaken("thisusernamedoesnotexist");

        // Assert
        isTaken.Should().BeFalse();
    }
    
    [Test]
    public async Task GetAll_WhenNoUsers_ShouldReturnEmptyList()
    {
        // Arrange
        await CleanUserTable(); // Ensure table is empty
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var userBusiness = new UserBusiness(context);

        // Act
        var users = userBusiness.GetAll();

        // Assert
        users.Should().NotBeNull();
        users.Should().BeEmpty();
    }

    [Test]
    public void Add_WithUsernameTooLong_ShouldThrowDbUpdateException()
    {
        // Arrange
        using var context = new VibelyDbContext(CreateNewContextOptions());
        var userBusiness = new UserBusiness(context);
        var invalidUser = CreateValidUser(99, "toolong");
        invalidUser.Username = new string('a', 40); // Exceeds StringLength(32)

        // Act & Assert
        // Add calls SaveChanges, which should throw due to constraint violation
        var exception = Assert.Throws<DbUpdateException>(() => userBusiness.Add(invalidUser));
        
        // Optional: Check inner exception for more specific database error if needed
        // exception.InnerException.Should().BeOfType<Npgsql.PostgresException>();
        // ((Npgsql.PostgresException)exception.InnerException).SqlState.Should().Be("22001"); // value too long for type character varying(32)
        Console.WriteLine($"Caught expected exception: {exception.Message}"); 
    }

    [Test]
    public async Task Update_NonExistentUser_ShouldDoNothing()
    {
        // Arrange
        await CleanUserTable();
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var userBusiness = new UserBusiness(context);
        var nonExistentUserId = 997;
        var someUserData = CreateValidUser(nonExistentUserId);
        var initialCount = await context.Users.CountAsync(); // Should be 0

        // Act
        userBusiness.Update(nonExistentUserId, someUserData);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        assertContext.Users.Count().Should().Be(initialCount);
        (await assertContext.Users.FindAsync(nonExistentUserId)).Should().BeNull();
    }

    // Helper to clean the Users table before certain tests
    private async Task CleanUserTable()
    {
        await using var cleanupContext = new VibelyDbContext(CreateNewContextOptions());
        // Simple approach: Remove all users. Might need adjustment for foreign keys later.
        var allUsers = await cleanupContext.Users.ToListAsync();
        if (allUsers.Any())
        {
            cleanupContext.Users.RemoveRange(allUsers);
            await cleanupContext.SaveChangesAsync();
        }
    }

    // Note: Testing IUserBusiness.FindByCredentials would require setting up the IPasswordHasher
    // interface and potentially mocking it or providing a test implementation if direct hashing is complex.
    // This might be better suited for unit tests or requires more setup here.
} 