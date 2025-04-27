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

public class GenreBusinessTests : DatabaseTestBase
{
    // Helper to create a valid genre
    private Genre CreateValidGenre(int id, string namePrefix = "TestGenre")
    {
        return new Genre
        {
            Id = id,
            Name = $"{namePrefix}{id}"
        };
    }

    // Helper to clean the Genres table before certain tests
    private async Task CleanGenreTable()
    {
        await using var cleanupContext = new VibelyDbContext(CreateNewContextOptions());
        // Simple approach: Remove all genres. Assumes no critical FK constraints from Songs yet.
        var allGenres = await cleanupContext.Genres.ToListAsync();
        if (allGenres.Any())
        {
            cleanupContext.Genres.RemoveRange(allGenres);
            await cleanupContext.SaveChangesAsync();
        }
    }

    [Test]
    public async Task Add_ShouldPersistGenre()
    {
        // Arrange
        await CleanGenreTable(); // Ensure clean state for this assertion
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var genreBusiness = new GenreBusiness(context);
        var newGenre = CreateValidGenre(1);

        // Act
        genreBusiness.Add(newGenre);
        // Add calls SaveChanges internally

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var addedGenre = await assertContext.Genres.FindAsync(newGenre.Id);
        addedGenre.Should().NotBeNull();
        // Manual assertion for relevant properties
        addedGenre.Id.Should().Be(newGenre.Id);
        addedGenre.Name.Should().Be(newGenre.Name);
    }

    [Test]
    public async Task Add_WhenGenreExists_ShouldNotAddDuplicate()
    {
        // Arrange
        await CleanGenreTable();
        var existingGenre = CreateValidGenre(2);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Genres.Add(existingGenre);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var genreBusiness = new GenreBusiness(context);
        var duplicateGenre = CreateValidGenre(2); // Same ID

        // Act
        genreBusiness.Add(duplicateGenre);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var genres = await assertContext.Genres.Where(g => g.Id == existingGenre.Id).ToListAsync();
        genres.Should().HaveCount(1);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllAddedGenres()
    {
        // Arrange
        await CleanGenreTable();
        var genre1 = CreateValidGenre(3);
        var genre2 = CreateValidGenre(4);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Genres.AddRange(genre1, genre2);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var genreBusiness = new GenreBusiness(context);

        // Act
        var genres = genreBusiness.GetAll();

        // Assert
        genres.Should().NotBeNull();
        genres.Should().HaveCount(2);
        // Manual assertion for contained items
        genres.Should().ContainSingle(g => g.Id == genre1.Id && g.Name == genre1.Name);
        genres.Should().ContainSingle(g => g.Id == genre2.Id && g.Name == genre2.Name);
    }

    [Test]
    public async Task Find_WithExistingId_ShouldReturnGenre()
    {
        // Arrange
        await CleanGenreTable();
        var genre = CreateValidGenre(5);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Genres.Add(genre);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var genreBusiness = new GenreBusiness(context);

        // Act
        var foundGenre = genreBusiness.Find(genre.Id);

        // Assert
        foundGenre.Should().NotBeNull();
        // Manual assertion for relevant properties
        foundGenre.Id.Should().Be(genre.Id);
        foundGenre.Name.Should().Be(genre.Name);
    }

    [Test]
    public async Task Find_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        await CleanGenreTable();
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var genreBusiness = new GenreBusiness(context);

        // Act
        var foundGenre = genreBusiness.Find(999); // ID that shouldn't exist

        // Assert
        foundGenre.Should().BeNull();
    }

    [Test]
    public async Task FindByName_WithExistingName_ShouldReturnGenre()
    {
        // Arrange
        await CleanGenreTable();
        var genre = CreateValidGenre(6, "UniqueGenre");
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Genres.Add(genre);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var genreBusiness = new GenreBusiness(context);

        // Act
        var foundGenre = genreBusiness.FindByName(genre.Name);

        // Assert
        foundGenre.Should().NotBeNull();
        // Manual assertion for relevant properties
        foundGenre.Id.Should().Be(genre.Id);
        foundGenre.Name.Should().Be(genre.Name);
    }

    [Test]
    public async Task FindByName_WithNonExistingName_ShouldReturnNull()
    {
        // Arrange
        await CleanGenreTable();
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var genreBusiness = new GenreBusiness(context);

        // Act
        var foundGenre = genreBusiness.FindByName("NonExistentGenreName");

        // Assert
        foundGenre.Should().BeNull();
    }

    [Test]
    public async Task Remove_WithExistingId_ShouldDeleteGenre()
    {
        // Arrange
        await CleanGenreTable();
        var genre = CreateValidGenre(7);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Genres.Add(genre);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var genreBusiness = new GenreBusiness(context);

        // Act
        genreBusiness.Remove(genre.Id);
        // Remove calls SaveChanges internally

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var deletedGenre = await assertContext.Genres.FindAsync(genre.Id);
        deletedGenre.Should().BeNull();
    }

    [Test]
    public async Task Remove_WithNonExistingId_ShouldDoNothing()
    {
        // Arrange
        await CleanGenreTable();
        var genre = CreateValidGenre(8);
        int initialCount = 0; // Declare outside
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Genres.Add(genre);
            await setupContext.SaveChangesAsync();
            initialCount = await setupContext.Genres.CountAsync(); // Calculate count *inside* the using block
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var genreBusiness = new GenreBusiness(context);

        // Act
        genreBusiness.Remove(998); // Non-existent ID

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        assertContext.Genres.Count().Should().Be(initialCount);
        (await assertContext.Genres.AnyAsync(g => g.Id == genre.Id)).Should().BeTrue();
    }

    [Test]
    public async Task Update_ShouldModifyGenre()
    {
        // Arrange
        await CleanGenreTable();
        var originalGenre = CreateValidGenre(9);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Genres.Add(originalGenre);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var genreBusiness = new GenreBusiness(context);

        var updatedGenreData = CreateValidGenre(9); // Same ID
        updatedGenreData.Name = "UpdatedGenreName";

        // Act
        genreBusiness.Update(originalGenre.Id, updatedGenreData);
        // Update calls SaveChanges internally

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var updatedGenre = await assertContext.Genres.FindAsync(originalGenre.Id);
        updatedGenre.Should().NotBeNull();
        updatedGenre.Name.Should().Be("UpdatedGenreName");
    }
} 