using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Vibely_App.Data;
using Vibely_App.Business;
using Vibely_App.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Vibely_App_Test;

public class PlaylistBusinessTests : DatabaseTestBase
{
    // Helper to create a valid playlist
    private Playlist CreateValidPlaylist(int id, string titlePrefix = "TestPlaylist")
    {
        return new Playlist
        {
            Id = id,
            Title = $"{titlePrefix}{id}",
            Duration = 120 + id // Example duration
        };
    }

    // Helper to clean the Playlists table
    private async Task CleanPlaylistTable()
    {
        await using var cleanupContext = new VibelyDbContext(CreateNewContextOptions());
        // Might need more complex cleanup if FKs from UserPlaylist/PlaylistSong exist
        var allPlaylists = await cleanupContext.Playlists.ToListAsync();
        if (allPlaylists.Any())
        {
            cleanupContext.Playlists.RemoveRange(allPlaylists);
            await cleanupContext.SaveChangesAsync();
        }
    }

    [Test]
    public async Task Add_ShouldPersistPlaylist()
    {
        // Arrange
        await CleanPlaylistTable();
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var playlistBusiness = new PlaylistBusiness(context);
        var newPlaylist = CreateValidPlaylist(1);

        // Act
        playlistBusiness.Add(newPlaylist);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var addedPlaylist = await assertContext.Playlists.FindAsync(newPlaylist.Id);
        addedPlaylist.Should().NotBeNull();
        // Manual assertions
        addedPlaylist.Id.Should().Be(newPlaylist.Id);
        addedPlaylist.Title.Should().Be(newPlaylist.Title);
        addedPlaylist.Duration.Should().Be(newPlaylist.Duration);
    }

    [Test]
    public async Task Add_WhenPlaylistExists_ShouldNotAddDuplicate()
    {
        // Arrange
        await CleanPlaylistTable();
        var existingPlaylist = CreateValidPlaylist(2);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Playlists.Add(existingPlaylist);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var playlistBusiness = new PlaylistBusiness(context);
        var duplicatePlaylist = CreateValidPlaylist(2); // Same ID

        // Act
        playlistBusiness.Add(duplicatePlaylist);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var playlists = await assertContext.Playlists.Where(p => p.Id == existingPlaylist.Id).ToListAsync();
        playlists.Should().HaveCount(1);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllAddedPlaylists()
    {
        // Arrange
        await CleanPlaylistTable();
        var playlist1 = CreateValidPlaylist(3);
        var playlist2 = CreateValidPlaylist(4);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Playlists.AddRange(playlist1, playlist2);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var playlistBusiness = new PlaylistBusiness(context);

        // Act
        var playlists = playlistBusiness.GetAll();

        // Assert
        playlists.Should().NotBeNull();
        playlists.Should().HaveCount(2);
        // Manual assertions for contained items
        playlists.Should().ContainSingle(p => p.Id == playlist1.Id && p.Title == playlist1.Title && p.Duration == playlist1.Duration);
        playlists.Should().ContainSingle(p => p.Id == playlist2.Id && p.Title == playlist2.Title && p.Duration == playlist2.Duration);
    }

    [Test]
    public async Task Find_WithExistingId_ShouldReturnPlaylist()
    {
        // Arrange
        await CleanPlaylistTable();
        var playlist = CreateValidPlaylist(5);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Playlists.Add(playlist);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var playlistBusiness = new PlaylistBusiness(context);

        // Act
        var foundPlaylist = playlistBusiness.Find(playlist.Id);

        // Assert
        foundPlaylist.Should().NotBeNull();
        foundPlaylist.Id.Should().Be(playlist.Id);
        foundPlaylist.Title.Should().Be(playlist.Title);
        foundPlaylist.Duration.Should().Be(playlist.Duration);
    }

    [Test]
    public async Task Find_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        await CleanPlaylistTable();
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var playlistBusiness = new PlaylistBusiness(context);

        // Act
        var foundPlaylist = playlistBusiness.Find(999); // ID that shouldn't exist

        // Assert
        foundPlaylist.Should().BeNull();
    }

    [Test]
    public async Task FindByName_WithExistingName_ShouldReturnPlaylist()
    {
        // Arrange
        await CleanPlaylistTable();
        var playlist = CreateValidPlaylist(6, "UniquePlaylist");
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Playlists.Add(playlist);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var playlistBusiness = new PlaylistBusiness(context);

        // Act
        var foundPlaylist = playlistBusiness.FindByName(playlist.Title);

        // Assert
        foundPlaylist.Should().NotBeNull();
        foundPlaylist.Id.Should().Be(playlist.Id);
        foundPlaylist.Title.Should().Be(playlist.Title);
        foundPlaylist.Duration.Should().Be(playlist.Duration);
    }

    [Test]
    public async Task FindByName_WithNonExistingName_ShouldReturnNull()
    {
        // Arrange
        await CleanPlaylistTable();
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var playlistBusiness = new PlaylistBusiness(context);

        // Act
        var foundPlaylist = playlistBusiness.FindByName("NonExistentPlaylistName");

        // Assert
        foundPlaylist.Should().BeNull();
    }

    [Test]
    public async Task Remove_WithExistingId_ShouldDeletePlaylist()
    {
        // Arrange
        await CleanPlaylistTable();
        var playlist = CreateValidPlaylist(7);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Playlists.Add(playlist);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var playlistBusiness = new PlaylistBusiness(context);

        // Act
        playlistBusiness.Remove(playlist.Id);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var deletedPlaylist = await assertContext.Playlists.FindAsync(playlist.Id);
        deletedPlaylist.Should().BeNull();
    }

    [Test]
    public async Task Remove_WithNonExistingId_ShouldDoNothing()
    {
        // Arrange
        await CleanPlaylistTable();
        var playlist = CreateValidPlaylist(8);
        int initialCount = 0;
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Playlists.Add(playlist);
            await setupContext.SaveChangesAsync();
            initialCount = await setupContext.Playlists.CountAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var playlistBusiness = new PlaylistBusiness(context);

        // Act
        playlistBusiness.Remove(998); // Non-existent ID

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        assertContext.Playlists.Count().Should().Be(initialCount);
        (await assertContext.Playlists.AnyAsync(p => p.Id == playlist.Id)).Should().BeTrue();
    }

    [Test]
    public async Task Update_ShouldModifyPlaylist()
    {
        // Arrange
        await CleanPlaylistTable();
        var originalPlaylist = CreateValidPlaylist(9);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Playlists.Add(originalPlaylist);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var playlistBusiness = new PlaylistBusiness(context);

        var updatedPlaylistData = CreateValidPlaylist(9); // Same ID
        updatedPlaylistData.Title = "UpdatedPlaylistTitle";
        updatedPlaylistData.Duration = 999;

        // Act
        playlistBusiness.Update(originalPlaylist.Id, updatedPlaylistData);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var updatedPlaylist = await assertContext.Playlists.FindAsync(originalPlaylist.Id);
        updatedPlaylist.Should().NotBeNull();
        updatedPlaylist.Title.Should().Be("UpdatedPlaylistTitle");
        updatedPlaylist.Duration.Should().Be(999);
    }
} 