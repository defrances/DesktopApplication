namespace DesktopApplication.Core;

public sealed class NoteStore
{
    public NoteStore(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        FilePath = filePath;
    }

    public string FilePath { get; }

    public static string DefaultPath =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DesktopApplication",
            "notes.txt");

    public string Load()
    {
        return File.Exists(FilePath) ? File.ReadAllText(FilePath) : string.Empty;
    }

    public void Save(string? content)
    {
        var directory = Path.GetDirectoryName(FilePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(FilePath, content ?? string.Empty);
    }
}
