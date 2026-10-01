# 01 — Notatnik: UI i zapis do pliku

Polecenie: pole na tytuł, pole na treść, przycisk zapisu i lista
zapisanych notatek; notatki zapisywane do pliku.

## Zmiany

- `MainWindow.axaml` — layout `Grid`: `TitleBox` (tytuł),
  `ContentBox` (treść, wielolinijkowa), przyciski `SaveButton` /
  `NewButton` / `DeleteButton`, lista `NotesList`, status `StatusText`.
- `MainWindow.axaml.cs` — model `Note` (`Title`, `Content`, `Created`;
  `ToString()` w formacie `yyyy-MM-dd HH:mm — Tytuł` decyduje
  o wyglądzie pozycji na liście).
- Zapis: cała kolekcja `ObservableCollection<Note>` serializowana
  do pliku `notatki.json` obok pliku wykonywalnego
  (`AppContext.BaseDirectory`), odczyt przy starcie (`LoadNotes`).
- `Zapisz` dodaje nową notatkę albo aktualizuje zaznaczoną;
  pusty tytuł blokowany komunikatem.

## Weryfikacja

- `dotnet build` — 0 błędów, 0 ostrzeżeń.
