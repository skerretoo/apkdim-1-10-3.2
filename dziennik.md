# Dziennik weryfikacji

Wpisy od najnowszego. Zapisuję wynik sprawdzenia, nie wrażenie.

## 2026-10-22 — wpis wsteczny: notatnik

- Przyjęte bez sprawdzenia (2026-10-01, commit `187ac3f`): cały notatnik. W `changes/01`–`04` jako weryfikację wpisałem `dotnet build`, a round-trip sprawdziłem na kopii logiki, nie w działającej aplikacji. Nie sprawdziłem notatki, której treść zawiera linię zaczynającą się od `## `.
- Sprawdzone dzisiaj: `dotnet build` → **1 błąd**, `NETSDK1045` (SDK 9.0.311, projekt `net10.0`). W tym środowisku nie uruchomię aplikacji, więc działanie sprawdzam na kopii funkcji zapisu i odczytu.
- Sprawdzone dzisiaj: test na kopii `ToMarkdown` / `ParseMarkdown` z `MainWindow.axaml.cs` → notatka z linią `## 2025-01-01 — ...` w treści wraca jako **dwie** notatki, a pierwsza ma treść uciętą przed tą linią. To cicha utrata danych przy zapisie i ponownym odczycie.
- Zauważone w kodzie, nie sprawdzone w działaniu: `SaveNotes()` ustawia „Zapisano” i nadpisuje szczegółowe „Zapisano notatkę” / „Zaktualizowano notatkę”, więc po komunikacie nie widać, czy zapis był edycją, czy nowym wpisem.
- Nie wiadomo: co się stanie, gdy plik notatek jest otwarty w innym programie i nie można do niego pisać.
- Następnym razem: zapisać i odczytać notatkę z `## ` w treści, polskimi znakami i wieloma liniami — w działającej aplikacji, nie tylko na kopii funkcji.
