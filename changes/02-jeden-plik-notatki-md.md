# 02 — Jeden plik `notatki.md` w folderze projektu

Polecenie: notatki w jednym pliku `notatki.md`; wpis w formacie

```text
## 2026-09-08 14:32 — Tytuł notatki
treść notatki, może mieć kilka linii
```

pusta linia między wpisami, najnowsza notatka na górze,
polskie znaki bez escapowania.

## Zmiany

- `MainWindow.axaml.cs` — porzucono JSON na rzecz Markdown.
- `ResolveNotesPath()` — „folder projektu” wykrywany przez marsz
  w górę od `AppContext.BaseDirectory` aż do katalogu z `*.csproj`;
  fallback to katalog pliku wykonywalnego.
- `ToMarkdown()` — nagłówek `## yyyy-MM-dd HH:mm — Tytuł`,
  pod nim treść; zapis UTF-8 bez BOM (`UTF8Encoding(false)`),
  więc `ą`, `ę`, `ż` itp. trafiają do pliku dosłownie.
- `ParseMarkdown()` / `ParseHeader()` — odczyt toleruje separatory
  `—` / `–` / `-`; treść to linie do następnego `## `,
  z obciętymi pustymi liniami na brzegach.
- Nowe notatki wstawiane na początek (`Insert(0, …)`),
  więc plik i lista mają najnowsze wpisy na górze.

## Weryfikacja

- `dotnet build` — 0 błędów.
- Round-trip zapis → odczyt z polskimi znakami i wieloliniową treścią.
