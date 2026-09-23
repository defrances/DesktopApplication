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

    /// <summary>
    /// INTENTIONAL_SKILL_TEST_VULNERABILITY
    /// Writes a copy to a caller-supplied relative path. The name is combined
    /// with the notes directory and is not checked for <c>..</c> or rooted paths,
    /// so the copy can land outside the application data folder.
    /// </summary>
    public const string ExportMarker = "INTENTIONAL_SKILL_TEST_VULNERABILITY";

    public string ExportCopy(string destinationName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationName);
        var directory = Path.GetDirectoryName(FilePath) ?? string.Empty;
        var destination = Path.GetFullPath(Path.Combine(directory, destinationName));
        var destDir = Path.GetDirectoryName(destination);
        if (!string.IsNullOrEmpty(destDir))
        {
            Directory.CreateDirectory(destDir);
        }

        File.WriteAllText(destination, Load());
        return destination;
    }
}
