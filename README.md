# Industrial Installations Blazor App

Create and monitor linked industrial installations with sensors. Sensors update every 2 seconds and can be recorded to view an inline line chart.

## Features

- Create installations with name, location, and link to a previous installation.
- Add sensors (Temp, Speed, Torque) to installations.
- Live sensor updates every 2 seconds.
- Optional recording with inline SVG line plot.

## Run

```bash
# from repo root
 dotnet run
# open the URL printed in the console (e.g., http://localhost:5196)
```

## Tech

- Blazor Server (.NET 7)
- In-memory services: `InstallationService`, `SensorUpdateService`

## Repository

- Branching: feature branches, Conventional Commits
- Versioning: Semantic Versioning (tags like v0.1.0)
