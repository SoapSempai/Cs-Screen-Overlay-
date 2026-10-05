# TEST 1 — Phase 6 Service Architecture

Phase 6 introduces a small application/service boundary without adding process-memory access, automated input, anti-cheat bypass, or anti-analysis behavior.

## Components

- `Core/ApplicationHost.cs` — lifecycle, health state, snapshot orchestration.
- `Core/ApplicationHealth.cs` — explicit lifecycle health states.
- `Data/IDataSource.cs` — neutral data-source abstraction.
- `Data/DataSnapshot.cs` — immutable snapshot contract.
- `Data/SimulatedDataSource.cs` — deterministic local test source.
- `Telemetry/TelemetryEvent.cs` — structured telemetry contract.
- `Telemetry/ITelemetrySink.cs` — telemetry abstraction.
- `Telemetry/ConsoleTelemetrySink.cs` — dependency-free console sink.
- `Services/ApplicationComposition.cs` — default dependency composition.
- `Program.Phase6.cs.txt` — exact integration block for the existing `Program.cs`.

## Integration

Do not create a second `Program.cs`. Keep the existing Phase 4 protection and Phase 2B diagnostics at the beginning of `Main`, then add the contents of `Program.Phase6.cs.txt` after those checks succeed.

The supplied data source is intentionally simulated. It establishes the interface boundary for future passive/offline data providers without granting the application another-process memory access or automated control.

## Performance impact

- Lifecycle/health state: LOW.
- Structured console telemetry: LOW for the current startup/snapshot volume.
- Simulated data source: LOW.
- No background polling thread is introduced by Phase 6.

## Validation target

Debug and Release should both compile under the existing `net10.0` x64 project. Existing protection and offset diagnostics must continue to run before the Phase 6 host is created.
