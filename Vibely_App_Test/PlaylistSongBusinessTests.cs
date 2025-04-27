using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Vibely_App.Data;
using Vibely_App.Business;
using Vibely_App.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Vibely_App_Test;

public class PlaylistSongBusinessTests : DatabaseTestBase
{
    // Store IDs of prerequisite records
    private int _testUserId;
    private int _testGenreId;
    private int _testPlaylistId;
    private int _testSongId;
    private int _testSongDuration;

    [OneTimeSetUp]
    public async Task SetupPrerequisites()
    {
        await using var setupContext = new VibelyDbContext(CreateNewContextOptions());

        // --- Ensure User --- 
        var testUser = await setupContext.Users.FirstOrDefaultAsync(u => u.Username == "_PlaylistSongTestUser");
        if (testUser == null)
        {
            testUser = new User
            {
                Id = 99996,
                Username = "_PlaylistSongTestUser",
                Password = "hashed_password", Email = "ps_test@example.com",
                FirstName = "PlaylistSong", LastName = "Tester", PhoneNumber = "2222222222",
                ProfilePicture = new byte[] { 0x03 }, IsPremium = false,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow), EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
                SubscriptionPrice = 0
            };
            setupContext.Users.Add(testUser);
            await setupContext.SaveChangesAsync();
        }
        _testUserId = testUser.Id;

        // --- Ensure Genre --- 
        var testGenre = await setupContext.Genres.FirstOrDefaultAsync(g => g.Name == "_PlaylistSongTestGenre");
        if (testGenre == null)
        {
            testGenre = new Genre { Id = 99996, Name = "_PlaylistSongTestGenre" };
            setupContext.Genres.Add(testGenre);
            await setupContext.SaveChangesAsync();
        }
        _testGenreId = testGenre.Id;

        // --- Ensure Playlist --- 
        var testPlaylist = await setupContext.Playlists.FirstOrDefaultAsync(p => p.Title == "_PlaylistSongTestPlaylist");
        if (testPlaylist == null)
        {
            testPlaylist = new Playlist { Id = 99996, Title = "_PlaylistSongTestPlaylist", Duration = 0 }; // Start duration 0
            setupContext.Playlists.Add(testPlaylist);
            await setupContext.SaveChangesAsync();
        }
        _testPlaylistId = testPlaylist.Id;

        // --- Ensure Song --- 
        _testSongDuration = 210;
        var testSong = await setupContext.Songs.FirstOrDefaultAsync(s => s.Title == "_PlaylistSongTestSong");
        if (testSong == null)
        {
            testSong = new Song 
            { 
                Id = 99996, Title = "_PlaylistSongTestSong", Artist = "TestPSArtist", 
                Duration = _testSongDuration, GenreId = _testGenreId, UserId = _testUserId, 
                Data = new byte[] { 0xFF } 
            };
            setupContext.Songs.Add(testSong);
            await setupContext.SaveChangesAsync();
        }
        _testSongId = testSong.Id;

        Console.WriteLine($"[PlaylistSongBusinessTests] Prerequisites ensured: UserId={_testUserId}, GenreId={_testGenreId}, PlaylistId={_testPlaylistId}, SongId={_testSongId}");
    }

    // Helper to create a valid PlaylistSong
    private PlaylistSong CreateValidPlaylistSong(int id, int? playlistId = null, int? songId = null)
    {
        return new PlaylistSong
        {
            Id = id,
            PlaylistId = playlistId ?? _testPlaylistId,
            SongId = songId ?? _testSongId
        };
    }

    // Helper to clean the table
    private async Task CleanPlaylistSongTable()
    {
        await using var cleanupContext = new VibelyDbContext(CreateNewContextOptions());
        var allItems = await cleanupContext.PlaylistsSongs.ToListAsync();
        if (allItems.Any())
        {
            cleanupContext.PlaylistsSongs.RemoveRange(allItems);
            // Reset playlist duration potentially affected by Add
            var playlist = await cleanupContext.Playlists.FindAsync(_testPlaylistId);
            if(playlist != null) playlist.Duration = 0; 
            await cleanupContext.SaveChangesAsync();
        }
    }

    [Test]
    public async Task Add_ShouldPersistAndIncreasePlaylistDuration()
    {
        // Arrange
        await CleanPlaylistSongTable();
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new PlaylistSongBusiness(context);
        var newItem = CreateValidPlaylistSong(1);
        var initialPlaylistDuration = (await context.Playlists.FindAsync(_testPlaylistId))?.Duration ?? 0;

        // Act
        business.Add(newItem);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var addedItem = await assertContext.PlaylistsSongs.FindAsync(newItem.Id);
        addedItem.Should().NotBeNull();
        addedItem.PlaylistId.Should().Be(_testPlaylistId);
        addedItem.SongId.Should().Be(_testSongId);

        // Check playlist duration increased
        var updatedPlaylist = await assertContext.Playlists.FindAsync(_testPlaylistId);
        updatedPlaylist.Should().NotBeNull();
        updatedPlaylist.Duration.Should().Be(initialPlaylistDuration + _testSongDuration);
    }

    [Test]
    public async Task Add_WhenItemExists_ShouldNotAddDuplicate()
    {
        // Arrange
        await CleanPlaylistSongTable();
        var existingItem = CreateValidPlaylistSong(2);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            // Manually add without triggering duration update for test setup clarity
            setupContext.PlaylistsSongs.Add(existingItem);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new PlaylistSongBusiness(context);
        var duplicateItem = CreateValidPlaylistSong(2); // Same ID

        // Act
        business.Add(duplicateItem);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var items = await assertContext.PlaylistsSongs.Where(ps => ps.Id == existingItem.Id).ToListAsync();
        items.Should().HaveCount(1);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllItemsWithIncludes()
    {
        // Arrange
        await CleanPlaylistSongTable();
        var item1 = CreateValidPlaylistSong(3);
        // Need a second song for distinction
        Song song2 = null;
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            song2 = new Song { Id=99995, Title="_PSSong2", Artist="Art2", Duration=10, GenreId=_testGenreId, UserId=_testUserId, Data=new byte[]{0x01}};
            setupContext.Songs.Add(song2);
            await setupContext.SaveChangesAsync();
            var item2 = CreateValidPlaylistSong(4, songId: song2.Id);
            setupContext.PlaylistsSongs.AddRange(item1, item2);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new PlaylistSongBusiness(context);

        // Act
        var items = business.GetAll();

        // Assert
        items.Should().NotBeNull();
        items.Should().HaveCount(2);
        // Check includes on one item
        var retrievedItem1 = items.FirstOrDefault(ps => ps.Id == item1.Id);
        retrievedItem1.Should().NotBeNull();
        retrievedItem1.Playlist.Should().NotBeNull();
        retrievedItem1.Playlist.Id.Should().Be(_testPlaylistId);
        retrievedItem1.Song.Should().NotBeNull();
        retrievedItem1.Song.Id.Should().Be(_testSongId);
        // Check presence of other item
        items.Should().ContainSingle(ps => ps.Id == 4 && ps.SongId == song2.Id);
    }

    [Test]
    public async Task Find_WithExistingId_ShouldReturnItem()
    {
        // Arrange
        await CleanPlaylistSongTable();
        var item = CreateValidPlaylistSong(5);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.PlaylistsSongs.Add(item);
            await setupContext.SaveChangesAsync();
        }
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new PlaylistSongBusiness(context);

        // Act
        var foundItem = business.Find(item.Id);

        // Assert
        foundItem.Should().NotBeNull();
        foundItem.PlaylistId.Should().Be(_testPlaylistId);
        foundItem.SongId.Should().Be(_testSongId);
    }
    
    [Test]
    public async Task FindByPlaylistAndSong_WithExistingPair_ShouldReturnItem()
    {
        // Arrange
        await CleanPlaylistSongTable();
        var item = CreateValidPlaylistSong(6);
        Playlist playlist = null;
        Song song = null;
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.PlaylistsSongs.Add(item);
            await setupContext.SaveChangesAsync();
            playlist = await setupContext.Playlists.FindAsync(_testPlaylistId);
            song = await setupContext.Songs.FindAsync(_testSongId);
        }
        playlist.Should().NotBeNull();
        song.Should().NotBeNull();
        
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new PlaylistSongBusiness(context);

        // Act
        var foundItem = business.FindByPlaylistAndSong(playlist, song);

        // Assert
        foundItem.Should().NotBeNull();
        foundItem.Id.Should().Be(item.Id);
    }
    
    [Test]
    public async Task GetAllSongsInPlaylist_ShouldReturnCorrectSongs()
    {
        // Arrange
        await CleanPlaylistSongTable();
        var item1 = CreateValidPlaylistSong(7);
        Song song2 = null;
        Playlist playlist = null;
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            song2 = new Song { Id=99994, Title="_PSSong3", Artist="Art3", Duration=15, GenreId=_testGenreId, UserId=_testUserId, Data=new byte[]{0x02}};
            setupContext.Songs.Add(song2);
            await setupContext.SaveChangesAsync();
            var item2 = CreateValidPlaylistSong(8, songId: song2.Id);
            setupContext.PlaylistsSongs.AddRange(item1, item2);
            await setupContext.SaveChangesAsync();
            playlist = await setupContext.Playlists.FindAsync(_testPlaylistId);
        }
        playlist.Should().NotBeNull();
        
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new PlaylistSongBusiness(context);

        // Act
        var songsInPlaylist = business.GetAllSongsInPlaylist(playlist);

        // Assert
        songsInPlaylist.Should().NotBeNull();
        songsInPlaylist.Should().HaveCount(2);
        songsInPlaylist.Should().ContainSingle(s => s.Id == _testSongId);
        songsInPlaylist.Should().ContainSingle(s => s.Id == song2.Id);
        // Check includes
        var firstSong = songsInPlaylist.First(s => s.Id == _testSongId);
        firstSong.Genre.Should().NotBeNull();
        firstSong.User.Should().NotBeNull();
    }

    [Test]
    public async Task Remove_WithExistingId_ShouldDeleteItem()
    {
        // Arrange
        await CleanPlaylistSongTable();
        var item = CreateValidPlaylistSong(9);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            // Add without triggering duration update
            setupContext.PlaylistsSongs.Add(item);
            await setupContext.SaveChangesAsync();
        }
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new PlaylistSongBusiness(context);

        // Act
        business.Remove(item.Id);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var deletedItem = await assertContext.PlaylistsSongs.FindAsync(item.Id);
        deletedItem.Should().BeNull();
        // Note: Duration is NOT reset by Remove method, only by Add
    }

    // Update test requires ensuring other potential FK targets exist
    [Test]
    public async Task Update_ShouldModifyPlaylistSong()
    {
        // Arrange
        await CleanPlaylistSongTable();
        var originalItem = CreateValidPlaylistSong(10);
        Playlist otherPlaylist = null;
        Song otherSong = null;
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
             setupContext.PlaylistsSongs.Add(originalItem);
             otherPlaylist = new Playlist { Id=99995, Title="_OtherPSPlaylist", Duration=5 };
             otherSong = new Song { Id=99993, Title="_OtherPSSong", Artist="Art4", Duration=20, GenreId=_testGenreId, UserId=_testUserId, Data=new byte[]{0x03}};
             if (!await setupContext.Playlists.AnyAsync(p=>p.Id == otherPlaylist.Id)) setupContext.Playlists.Add(otherPlaylist);
             if (!await setupContext.Songs.AnyAsync(s=>s.Id == otherSong.Id)) setupContext.Songs.Add(otherSong);
             await setupContext.SaveChangesAsync();
             otherPlaylist = await setupContext.Playlists.FindAsync(otherPlaylist.Id);
             otherSong = await setupContext.Songs.FindAsync(otherSong.Id);
        }
        otherPlaylist.Should().NotBeNull();
        otherSong.Should().NotBeNull();

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var business = new PlaylistSongBusiness(context);
        var updatedData = new PlaylistSong { Id = originalItem.Id, PlaylistId = otherPlaylist.Id, SongId = otherSong.Id };

        // Act
        business.Update(originalItem.Id, updatedData);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var updatedItem = await assertContext.PlaylistsSongs.FindAsync(originalItem.Id);
        updatedItem.Should().NotBeNull();
        updatedItem.PlaylistId.Should().Be(otherPlaylist.Id);
        updatedItem.SongId.Should().Be(otherSong.Id);
    }

} 