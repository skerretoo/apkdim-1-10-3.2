# 04 — `try`/`catch` wokół zapisu do pliku

Polecenie: znaleźć w projekcie (Ctrl+Shift+F, fraza `File.`)
linijkę piszącą do pliku i objąć ją klamrami `try` z `catch`.

## Zmiany

- Wyszukiwanie `File\.` w projekcie: jedyna linijka zapisująca
  to `File.WriteAllText` w `SaveNotes()` (`MainWindow.axaml.cs`).
- `SaveNotes()` ma kształt ze wzoru:

```csharp
try
{
    File.WriteAllText(_filePath, ToMarkdown(_notes), new UTF8Encoding(false));
    StatusText.Text = "Zapisano";
}
catch (Exception ex)
{
    StatusText.Text = $"Nie udało się zapisać: {ex.Message}";
}
```

- Skutek uboczny: ogólne `Zapisano` nadpisuje szczegółowe
  `Zapisano notatkę` / `Zaktualizowano notatkę` z `SaveButton_Click`.

## Weryfikacja

- `dotnet build` — 0 błędów.
