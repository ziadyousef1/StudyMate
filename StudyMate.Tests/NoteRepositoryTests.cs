using Microsoft.EntityFrameworkCore;
using StudyMate.Data;
using StudyMate.Models;
using StudyMate.Repositories.Implementaions;

namespace StudyMate.Tests;

public class NoteRepositoryTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "NoteDb")
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task CreateAndGetNote_Works()
    {
        var context = GetDbContext();
        var repo = new NoteRepository(context);

        var note = new Note { Id = "1", Title = "Test", Content = "Content", UserId = "user1" };
        await repo.CreateAsync(note);

        var fetched = await repo.GetByIdAsync("1");
        Assert.NotNull(fetched);
        Assert.Equal("Test", fetched.Title);
    }

    [Fact]
    public async Task UpdateNote_ChangesTitle()
    {
        var context = GetDbContext();
        var repo = new NoteRepository(context);

        var note = new Note { Id = "2", Title = "Old", Content = "Old", UserId = "user1" };
        await repo.CreateAsync(note);

        note.Title = "New";
        var updated = await repo.UpdateAsync(note);

        Assert.True(updated);
        var fetched = await repo.GetByIdAsync("2");
        Assert.Equal("New", fetched.Title);
    }

    [Fact]
    public async Task DeleteNote_RemovesNote()
    {
        var context = GetDbContext();
        var repo = new NoteRepository(context);

        var note = new Note { Id = "3", Title = "ToDelete", Content = "Content", UserId = "user1" };
        await repo.CreateAsync(note);

        var deleted = await repo.DeleteAsync("3");
        
        Assert.True(deleted);
        var fetched = await repo.GetByIdAsync("3");
        Assert.Null(fetched);
    }
}