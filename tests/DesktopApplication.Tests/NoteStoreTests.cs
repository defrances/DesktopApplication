using DesktopApplication.Core;

namespace DesktopApplication.Tests;

public sealed class NoteStoreTests
{
    [Fact]
    public void Load_ReturnsEmptyString_WhenFileDoesNotExist()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "notes.txt");
        var store = new NoteStore(path);

        Assert.Equal(string.Empty, store.Load());
    }

    [Fact]
    public void Save_ThenLoad_ReturnsSavedContent()
    {
        var path = Path.Combine(Path.GetTempPath(), "DesktopApplicationTests", Guid.NewGuid().ToString("N"), "notes.txt");
        var store = new NoteStore(path);

        try
        {
            store.Save("hello from tests");
            Assert.Equal("hello from tests", store.Load());
            Assert.True(File.Exists(path));
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    [Fact]
    public void Constructor_Throws_WhenPathIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => new NoteStore(" "));
    }
}
