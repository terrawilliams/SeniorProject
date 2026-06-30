# Chaotic Creation

A Windows desktop app for tabletop RPG game masters that randomly generates *Dungeons & Dragons 5e* content — NPCs, encounters, locations, loot, and random-table rolls — and lets you save your favorite creations for later use.

Built as a senior project.

## Demo

[![Chaotic Creation demo](https://img.youtube.com/vi/aKgUqxVMvw4/maxresdefault.jpg)](https://www.youtube.com/watch?v=aKgUqxVMvw4)

## Features

- **Random Tables** — Roll on built-in tables with a full set of animated dice (d4, d6, d8, d10, d12, d20, and percentile/d100).
- **Encounter Generator** — Generate combat encounters, complete with a loot generator.
- **NPC Generator** — Create non-player characters with randomized names, races, and other traits.
- **Location Generator** — Generate locations for your world.
- **Saved Creations** — Save any generation, then view, edit, or delete it later. A "recent creations" list keeps your last 10 generations close at hand.

## Tech Stack

- **Language / Runtime:** C# on .NET Framework 4.7.2
- **UI:** WPF (XAML), organized with an MVVM-style pattern (View + `*Model` classes)
- **Data:** SQLite via `System.Data.SQLite` and Entity Framework 6
- **Source data:** D&D 5e content derived from the [5e-database](https://github.com/adrpadua/5e-database)

## Project Structure

```
ChaoticCreation/
├── ChaoticCreation.sln          # Visual Studio solution
├── ChaoticCreation.csproj
├── MainWindow.xaml              # Tabbed shell hosting each generator
├── App.xaml                     # App resources / theme
├── Creation.cs                  # Model for a single generated creation
├── RecentCreations.cs           # In-memory list of recent generations (singleton)
├── GeneralGenerator.cs          # Shared SQLite query helper for all generators
├── Save.cs                      # Persistence for saved creations
├── HomeTab/                     # Home tab
├── RandomTables/                # Random tables + dice rollers (d4–d100)
├── EncounterGenerator/          # Encounter + loot generation
├── NpcGenerator/                # NPC generation
├── LocationGenerator/           # Location generation
├── SavedCreations/              # View/edit/delete saved creations
├── Images/                      # Logo, dice, and UI art
└── sqlDatabase/                 # SQLite databases (MasterDB.db, NPC.db) and queries
```

Each generator follows the same shape: a `*.xaml` view, its code-behind, a `*Model.cs` for logic, and a `*Query_Gen.cs` that builds the SQL used to pull random rows from the SQLite databases.

## Getting Started

### Prerequisites

- Windows
- Visual Studio 2019 (or newer) with the **.NET desktop development** workload
- .NET Framework 4.7.2 developer pack

### Build & Run

1. Open `ChaoticCreation/ChaoticCreation.sln` in Visual Studio.
2. Restore NuGet packages (Visual Studio does this automatically on first build; the project uses `packages.config`).
3. Build the solution (`Ctrl+Shift+B`).
4. Run with `F5`.

The SQLite databases ship with the project under `ChaoticCreation/sqlDatabase/`, so no separate database setup is required.
