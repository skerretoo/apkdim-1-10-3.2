using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace notatnik;

public class Note
{
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime Created { get; set; } = DateTime.Now;

    // ToString decyduje o wyglądzie na liście
    public override string ToString() => $"{Created:yyyy-MM-dd HH:mm} — {Title}";
}

public partial class MainWindow : Window
{
    private readonly ObservableCollection<Note> _notes = new();
    private readonly string _filePath;

    public MainWindow()
    {
        InitializeComponent();

        _filePath = ResolveNotesPath();

        NotesList.ItemsSource = _notes;
        SaveButton.Click += SaveButton_Click;
        NewButton.Click += NewButton_Click;
        DeleteButton.Click += DeleteButton_Click;
        NotesList.SelectionChanged += NotesList_SelectionChanged;

        LoadNotes();
    }

    private static string ResolveNotesPath()
    {
        // "Folder projektu" = katalog zawierający .csproj.
        // Podczas uruchamiania z bin/Debug/... idziemy w górę aż znajdziemy projekt.
        try
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            for (int i = 0; i < 6 && dir != null; i++)
            {
                if (dir.GetFiles("*.csproj").Length > 0)
                    return Path.Combine(dir.FullName, "notatki.md");
                dir = dir.Parent;
            }
        }
        catch
        {
            // ignoruj, użyj fallbacku poniżej
        }
        return Path.Combine(AppContext.BaseDirectory, "notatki.md");
    }

    private void NotesList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (NotesList.SelectedItem is Note note)
        {
            TitleBox.Text = note.Title;
            ContentBox.Text = note.Content;
            StatusText.Text = "";
        }
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        var title = (TitleBox.Text ?? "").Trim().ReplaceLineEndings(" ");
        var content = (ContentBox.Text ?? "").ReplaceLineEndings("\n").Trim();

        if (string.IsNullOrWhiteSpace(title))
        {
            StatusText.Text = "Podaj tytuł notatki.";
            return;
        }

        if (NotesList.SelectedItem is Note selected)
        {
            // Aktualizuj zaznaczoną notatkę (data bez zmian)
            selected.Title = title;
            selected.Content = content;
            var idx = NotesList.SelectedIndex;
            _notes[idx] = selected;
            NotesList.SelectedIndex = idx;
            StatusText.Text = "Zaktualizowano notatkę.";
        }
        else
        {
            var note = new Note { Title = title, Content = content, Created = DateTime.Now };
            // Najnowsza na górze pliku i listy
            _notes.Insert(0, note);
            NotesList.SelectedItem = note;
            StatusText.Text = "Zapisano notatkę.";
        }

        SaveNotes();
    }

    private void NewButton_Click(object? sender, RoutedEventArgs e)
    {
        NotesList.SelectedItem = null;
        TitleBox.Text = "";
        ContentBox.Text = "";
        StatusText.Text = "";
        TitleBox.Focus();
    }

    private void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {
        if (NotesList.SelectedItem is Note note)
        {
            _notes.Remove(note);
            SaveNotes();
            TitleBox.Text = "";
            ContentBox.Text = "";
            StatusText.Text = "Usunięto notatkę.";
        }
        else
        {
            StatusText.Text = "Wybierz notatkę do usunięcia.";
        }
    }

    private void LoadNotes()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                foreach (var n in ParseMarkdown(File.ReadAllText(_filePath, Encoding.UTF8)))
                    _notes.Add(n);
                StatusText.Text = $"Wczytano {_notes.Count} notatek.";
            }
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Błąd odczytu pliku: {ex.Message}";
        }
    }

    private void SaveNotes()
    {
        try
        {
            File.WriteAllText(_filePath, ToMarkdown(_notes), new UTF8Encoding(false));
            StatusText.Text = "Zapisano";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Nie udało się zapisać: {ex.Message}";
        }
    }

    private static string ToMarkdown(IEnumerable<Note> notes)
    {
        // Dokładny format pliku (LF):
        // ## yyyy-MM-dd HH:mm — Tytuł
        // linia1 treści
        // linia2 treści
        //
        // ## ... (pusta linia tylko między wpisami)
        var blocks = new List<string>();
        foreach (var n in notes)
        {
            var title = (n.Title ?? "").ReplaceLineEndings(" ").Trim();
            var content = (n.Content ?? "").ReplaceLineEndings("\n").Trim();
            var block = $"## {n.Created:yyyy-MM-dd HH:mm} — {title}";
            if (!string.IsNullOrWhiteSpace(content))
                block += "\n" + content;
            blocks.Add(block);
        }
        return blocks.Count == 0 ? "" : string.Join("\n\n", blocks) + "\n";
    }

    private static List<Note> ParseMarkdown(string text)
    {
        var result = new List<Note>();
        Note? current = null;
        var contentLines = new List<string>();

        void Flush()
        {
            if (current != null)
            {
                // Usuń puste linie z początku/końca treści, środek zostaw
                int start = 0;
                while (start < contentLines.Count && string.IsNullOrWhiteSpace(contentLines[start]))
                    start++;
                int end = contentLines.Count - 1;
                while (end >= start && string.IsNullOrWhiteSpace(contentLines[end]))
                    end--;
                var kept = start <= end
                    ? contentLines.GetRange(start, end - start + 1)
                    : new List<string>();
                current.Content = string.Join("\n", kept);
                result.Add(current);
            }
        }

        foreach (var rawLine in text.ReplaceLineEndings("\n").Split('\n'))
        {
            var line = rawLine.TrimEnd('\r');
            if (line.StartsWith("## "))
            {
                Flush();
                current = ParseHeader(line[3..].Trim());
                contentLines = new List<string>();
            }
            else if (current != null)
            {
                contentLines.Add(line);
            }
        }
        Flush();
        return result;
    }

    private static Note ParseHeader(string header)
    {
        // Oczekiwane: "2026-09-08 14:32 — Tytuł"
        string title = header;
        DateTime created = DateTime.Now;

        if (header.Length >= 16 &&
            DateTime.TryParseExact(header[..16], "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
        {
            created = dt;
            var rest = header[16..].Trim();
            // Separator: em dash / en dash / myślnik
            if (rest.StartsWith("—") || rest.StartsWith("–") || rest.StartsWith("-"))
                rest = rest[1..].Trim();
            title = rest;
        }

        return new Note { Title = title, Created = created, Content = "" };
    }
}
