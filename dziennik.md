# Dziennik weryfikacji

Wpisy od najnowszego. Zapisuję wynik sprawdzenia, nie wrażenie.

## 2026-10-29 — build po instalacji SDK 10 i test formatu pliku

- Sprawdzone: `dotnet build` → **0 błędów, 0 ostrzeżeń** (SDK 10.0.401). Wpis z 2026-10-22 podaje `NETSDK1045`, bo wtedy był tylko SDK 9.0.311; dziś projekt się buduje.
- Sprawdzone: `bin/Debug/net10.0/notatnik.exe` uruchamia się, okno zostaje otwarte (4 s, proces żyje, `stderr` pusty).
- Sprawdzone na kopii `ToMarkdown` / `ParseMarkdown`: treść z linią `## 2025-01-01 — spotkanie` wraca jako **2 notatki** — pierwsza ucięta przed tą linią, druga z dzisiejszą datą i tytułem „2025-01-01 — spotkanie”. Potwierdzona cicha utrata danych przy zapisie i ponownym odczycie.
- Sprawdzone na kopii, 4 pozostałe przypadki: `# nagłówek` (jeden krzyżyk), pusta treść, polskie znaki z pustą linią w środku i tytuł z `—` wracają bez zmian.
- Nie wiadomo: co się stanie, gdy plik notatek jest otwarty w innym programie i nie można do niego pisać.
- Następnym razem: zapisać i odczytać notatkę z `## ` w treści w działającej aplikacji, nie tylko na kopii funkcji.

## 2026-10-22 — wpis wsteczny: notatnik

- Przyjęte bez sprawdzenia (2026-10-01, commit `187ac3f`): cały notatnik. W `changes/01`–`04` jako weryfikację wpisałem `dotnet build`, a round-trip sprawdziłem na kopii logiki, nie w działającej aplikacji. Nie sprawdziłem notatki, której treść zawiera linię zaczynającą się od `## `.
- Sprawdzone dzisiaj: `dotnet build` → **1 błąd**, `NETSDK1045` (SDK 9.0.311, projekt `net10.0`). W tym środowisku nie uruchomię aplikacji, więc działanie sprawdzam na kopii funkcji zapisu i odczytu.
- Sprawdzone dzisiaj: test na kopii `ToMarkdown` / `ParseMarkdown` z `MainWindow.axaml.cs` → notatka z linią `## 2025-01-01 — ...` w treści wraca jako **dwie** notatki, a pierwsza ma treść uciętą przed tą linią. To cicha utrata danych przy zapisie i ponownym odczycie.
- Zauważone w kodzie, nie sprawdzone w działaniu: `SaveNotes()` ustawia „Zapisano” i nadpisuje szczegółowe „Zapisano notatkę” / „Zaktualizowano notatkę”, więc po komunikacie nie widać, czy zapis był edycją, czy nowym wpisem.
- Nie wiadomo: co się stanie, gdy plik notatek jest otwarty w innym programie i nie można do niego pisać.
- Następnym razem: zapisać i odczytać notatkę z `## ` w treści, polskimi znakami i wieloma liniami — w działającej aplikacji, nie tylko na kopii funkcji.
