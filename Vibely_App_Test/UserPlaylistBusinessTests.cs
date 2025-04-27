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

public class UserPlaylistBusinessTests : DatabaseTestBase
{
    // Store IDs of prerequisite records
    private int _testUserId;
    private int _testPlaylistId;

    [OneTimeSetUp]
    public async Task SetupPrerequisites()
    {
        await using var setupContext = new VibelyDbContext(CreateNewContextOptions());

        // Ensure a test User exists
        var testUser = await setupContext.Users.FirstOrDefaultAsync(u => u.Username == "_UserPlaylistTestUser");
        if (testUser == null)
        {
            testUser = new User
            {
                Id = 99998, // High ID
                Username = "_UserPlaylistTestUser",
                Password = "hashed_password",
                Email = "up_test@example.com",
                FirstName = "UserPlaylist",
                LastName = "Tester",
                PhoneNumber = "1111111111",
                ProfilePicture = new byte[] { 0x02 },
                IsPremium = false,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
                SubscriptionPrice = 0
            };
            setupContext.Users.Add(testUser);
            await setupContext.SaveChangesAsync();
        }
        _testUserId = testUser.Id;

        // Ensure a test Playlist exists
        var testPlaylist = await setupContext.Playlists.FirstOrDefaultAsync(p => p.Title == "_UserPlaylistTestPlaylist");
        if (testPlaylist == null)
        {
            testPlaylist = new Playlist { Id = 99998, Title = "_UserPlaylistTestPlaylist", Duration = 10 }; // High ID
            setupContext.Playlists.Add(testPlaylist);
            await setupContext.SaveChangesAsync();
        }
        _testPlaylistId = testPlaylist.Id;

        Console.WriteLine($"[UserPlaylistBusinessTests] Prerequisites ensured: UserId={_testUserId}, PlaylistId={_testPlaylistId}");
    }

    // Helper to create a valid UserPlaylist
    private UserPlaylist CreateValidUserPlaylist(int id)
    {
        return new UserPlaylist
        {
            Id = id,
            UserId = _testUserId,
            PlaylistId = _testPlaylistId
        };
    }

    // Helper to clean the UserPlaylists table
    private async Task CleanUserPlaylistTable()
    {
        await using var cleanupContext = new VibelyDbContext(CreateNewContextOptions());
        var allItems = await cleanupContext.UsersPlaylists.ToListAsync();
        if (allItems.Any())
        {
            cleanupContext.UsersPlaylists.RemoveRange(allItems);
            await cleanupContext.SaveChangesAsync();
        }
    }

    [Test]
    public async Task Add_ShouldPersistUserPlaylist()
    {
        // Arrange
        await CleanUserPlaylistTable();
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new UserPlaylistBusiness(context);
        var newUserPlaylist = CreateValidUserPlaylist(1);

        // Act
        business.Add(newUserPlaylist);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var addedItem = await assertContext.UsersPlaylists.FindAsync(newUserPlaylist.Id);
        addedItem.Should().NotBeNull();
        addedItem.UserId.Should().Be(_testUserId);
        addedItem.PlaylistId.Should().Be(_testPlaylistId);
    }
    
    [Test]
    public async Task Add_WhenUserPlaylistExists_ShouldNotAddDuplicate()
    {
        // Arrange
        await CleanUserPlaylistTable();
        var existingItem = CreateValidUserPlaylist(2);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.UsersPlaylists.Add(existingItem);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new UserPlaylistBusiness(context);
        var duplicateItem = CreateValidUserPlaylist(2); // Same ID

        // Act
        business.Add(duplicateItem);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var items = await assertContext.UsersPlaylists.Where(up => up.Id == existingItem.Id).ToListAsync();
        items.Should().HaveCount(1);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllAddedUserPlaylistsWithIncludes()
    {
        // Arrange
        await CleanUserPlaylistTable();
        var item1 = CreateValidUserPlaylist(3);
        var item2 = CreateValidUserPlaylist(4);
        // Need a different playlist for item2 if needed for distinction
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.UsersPlaylists.AddRange(item1, item2);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new UserPlaylistBusiness(context);

        // Act
        var items = business.GetAll();

        // Assert
        items.Should().NotBeNull();
        items.Should().HaveCount(2);
        // Check one item in detail
        var retrievedItem1 = items.FirstOrDefault(up => up.Id == item1.Id);
        retrievedItem1.Should().NotBeNull();
        retrievedItem1.User.Should().NotBeNull();
        retrievedItem1.User.Id.Should().Be(_testUserId);
        retrievedItem1.Playlist.Should().NotBeNull();
        retrievedItem1.Playlist.Id.Should().Be(_testPlaylistId);
        // Check presence of other item
        items.Should().ContainSingle(up => up.Id == item2.Id);
    }

    [Test]
    public async Task Find_WithExistingId_ShouldReturnUserPlaylist()
    {
        // Arrange
        await CleanUserPlaylistTable();
        var item = CreateValidUserPlaylist(5);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.UsersPlaylists.Add(item);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new UserPlaylistBusiness(context);

        // Act
        var foundItem = business.Find(item.Id);

        // Assert
        foundItem.Should().NotBeNull();
        foundItem.UserId.Should().Be(_testUserId);
        foundItem.PlaylistId.Should().Be(_testPlaylistId);
    }

    [Test]
    public async Task Find_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        await CleanUserPlaylistTable();
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new UserPlaylistBusiness(context);

        // Act
        var foundItem = business.Find(999);

        // Assert
        foundItem.Should().BeNull();
    }

    [Test]
    public async Task Remove_WithExistingId_ShouldDeleteUserPlaylist()
    {
        // Arrange
        await CleanUserPlaylistTable();
        var item = CreateValidUserPlaylist(6);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.UsersPlaylists.Add(item);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new UserPlaylistBusiness(context);

        // Act
        business.Remove(item.Id);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var deletedItem = await assertContext.UsersPlaylists.FindAsync(item.Id);
        deletedItem.Should().BeNull();
    }

    [Test]
    public async Task Remove_WithNonExistingId_ShouldDoNothing()
    {
        // Arrange
        await CleanUserPlaylistTable();
        var item = CreateValidUserPlaylist(7);
        int initialCount = 0;
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.UsersPlaylists.Add(item);
            await setupContext.SaveChangesAsync();
            initialCount = await setupContext.UsersPlaylists.CountAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new UserPlaylistBusiness(context);

        // Act
        business.Remove(998);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        assertContext.UsersPlaylists.Count().Should().Be(initialCount);
        (await assertContext.UsersPlaylists.AnyAsync(up => up.Id == item.Id)).Should().BeTrue();
    }

    [Test]
    public async Task Update_ShouldModifyUserPlaylist()
    {
        // Arrange
        await CleanUserPlaylistTable();
        var originalItem = CreateValidUserPlaylist(8);
        // Need a second user/playlist for the update
        User otherUser = null;
        Playlist otherPlaylist = null;
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.UsersPlaylists.Add(originalItem);
            otherUser = new User { Id = 99997, Username = "_OtherUserForUP", Password="p", Email="e@e.com", FirstName="f", LastName="l", PhoneNumber="p", ProfilePicture = new byte[] { 0x09 } };
            otherPlaylist = new Playlist { Id = 99997, Title = "_OtherPlaylistForUP", Duration = 1 };
            if (!await setupContext.Users.AnyAsync(u => u.Id == otherUser.Id)) setupContext.Users.Add(otherUser);
            if (!await setupContext.Playlists.AnyAsync(p => p.Id == otherPlaylist.Id)) setupContext.Playlists.Add(otherPlaylist);
            await setupContext.SaveChangesAsync();
            // Ensure they have IDs if generated
            otherUser = await setupContext.Users.FindAsync(otherUser.Id);
            otherPlaylist = await setupContext.Playlists.FindAsync(otherPlaylist.Id);
        }
        otherUser.Should().NotBeNull();
        otherPlaylist.Should().NotBeNull();

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new UserPlaylistBusiness(context);

        var updatedData = new UserPlaylist
        {
            Id = originalItem.Id, // Keep the same ID
            UserId = otherUser.Id,      // Change UserId
            PlaylistId = otherPlaylist.Id // Change PlaylistId
        };

        // Act
        business.Update(originalItem.Id, updatedData);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var updatedItem = await assertContext.UsersPlaylists.FindAsync(originalItem.Id);
        updatedItem.Should().NotBeNull();
        updatedItem.UserId.Should().Be(otherUser.Id);
        updatedItem.PlaylistId.Should().Be(otherPlaylist.Id);
    }
} 