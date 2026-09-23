using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DesktopApplication.Core;

namespace DesktopApplication;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private readonly NoteStore _noteStore;
    private string _notes;
    private string _status;

    public MainViewModel()
        : this(new NoteStore(NoteStore.DefaultPath), SystemInformationProvider.GetCurrent())
    {
    }

    public MainViewModel(NoteStore noteStore, SystemInformation systemInformation)
    {
        _noteStore = noteStore;
        SystemInformation = systemInformation;
        _notes = _noteStore.Load();
        _status = "Ready";
        SaveCommand = new RelayCommand(SaveNotes);
        CheckBulletinCommand = new RelayCommand(CheckBulletin);
        ExportBackupCommand = new RelayCommand(ExportBackup);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public SystemInformation SystemInformation { get; }

    public string Title => "Desktop Application";

    public string Subtitle => "Windows desktop client";

    public string Notes
    {
        get => _notes;
        set
        {
            if (_notes == value)
            {
                return;
            }

            _notes = value;
            OnPropertyChanged();
        }
    }

    public string Status
    {
        get => _status;
        private set
        {
            if (_status == value)
            {
                return;
            }

            _status = value;
            OnPropertyChanged();
        }
    }

    public ICommand SaveCommand { get; }

    public ICommand CheckBulletinCommand { get; }

    public ICommand ExportBackupCommand { get; }

    private void SaveNotes()
    {
        _noteStore.Save(Notes);
        Status = $"Saved at {DateTime.Now:HH:mm:ss}";
    }

    private void CheckBulletin()
    {
        Status = "Fetching vendor bulletin…";
        try
        {
            var body = new InsecureVendorBulletinClient().FetchAsync().GetAwaiter().GetResult();
            Status = $"Bulletin bytes {body.Length} (TLS path {InsecureVendorBulletinClient.Marker})";
        }
        catch (Exception ex)
        {
            Status = $"Bulletin fetch failed: {ex.GetType().Name}";
        }
    }

    private void ExportBackup()
    {
        try
        {
            var path = _noteStore.ExportCopy(Path.Combine("..", "desktop-application-notes.bak"));
            Status = $"Backup written to {path} ({NoteStore.ExportMarker})";
        }
        catch (Exception ex)
        {
            Status = $"Backup failed: {ex.GetType().Name}";
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
