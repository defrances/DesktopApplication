using DesktopApplication.Core;

namespace DesktopApplication.Tests;

public sealed class NoteStoreTests
{
    [Fact]
    [Trait("Category", "Unit")]
    [Trait("TestId", "TC-UNIT-NOTES-EMPTY")]
    public void Load_ReturnsEmptyString_WhenFileDoesNotExist()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "notes.txt");
        var store = new NoteStore(path);

        Assert.Equal(string.Empty, store.Load());
    }

    [Fact]
    [Trait("Category", "Smoke")]
    [Trait("TestId", "TC-SMOKE-NOTES")]
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
    [Trait("Category", "Unit")]
    [Trait("TestId", "TC-UNIT-NOTES-PATH")]
    public void Constructor_Throws_WhenPathIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => new NoteStore(" "));
    }

    [Fact]
    [Trait("Category", "Unit")]
    [Trait("TestId", "TC-UNIT-NOTES-EXPORT")]
    public void ExportCopy_WritesOutsideTheNotesDirectory_WhenNameContainsParentSegments()
    {
        var folder = Path.Combine(Path.GetTempPath(), "DesktopApplicationTests", Guid.NewGuid().ToString("N"));
        var path = Path.Combine(folder, "notes.txt");
        var store = new NoteStore(path);
        store.Save("secret-note");

        try
        {
            var written = store.ExportCopy(Path.Combine("..", "escaped.bak"));
            var notesRoot = Path.GetFullPath(folder);
            var writtenFull = Path.GetFullPath(written);

            Assert.Equal(NoteStore.ExportMarker, "INTENTIONAL_SKILL_TEST_VULNERABILITY");
            Assert.False(writtenFull.StartsWith(notesRoot, StringComparison.OrdinalIgnoreCase));
            Assert.True(File.Exists(writtenFull));
            Assert.Equal("secret-note", File.ReadAllText(writtenFull));
        }
        finally
        {
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, recursive: true);
            }

            var leaked = Path.Combine(Path.GetDirectoryName(folder)!, "escaped.bak");
            if (File.Exists(leaked))
            {
                File.Delete(leaked);
            }
        }
    }
}
