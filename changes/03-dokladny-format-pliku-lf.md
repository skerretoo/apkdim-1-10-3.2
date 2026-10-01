# 03 — Dokładny wygląd pliku (LF, bez BOM)

Polecenie: plik ma wyglądać dokładnie tak:

```text
## 2026-09-08 14:32 — Zakupy
mleko
chleb

## 2026-09-07 09:15 — Pomysł na projekt
minutnik z historią sesji
```

## Zmiany

- `MainWindow.axaml.cs` — `ToMarkdown()` przepisane na jawne `\n`:
  blok to nagłówek + opcjonalnie `\n` + treść,
  bloki łączone `\n\n`, plik kończy pojedynczy `\n`.
- Treść i tytuł normalizowane przed zapisem
  (`ReplaceLineEndings`), więc brak mieszanych końcówek linii.
- Zapis bez BOM, bez znaków `\r` — bajtowo zgodnie ze wzorem.

## Weryfikacja

- `dotnet build` — 0 błędów.
- Test na kopii logiki ze wzorem `Zakupy` / `Pomysł na projekt`:
  wyjście bajtowo identyczne, LF, bez BOM, round-trip OK.
