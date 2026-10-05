# TEST 1 — Phase 7 Provider Architecture

Phase 7 hardens the passive data-provider boundary introduced in Phase 6.

## Components

- `Data/DataSnapshotValidator.cs` — validates snapshot structure and numeric values.
- `Data/SimulatedDataSource.cs` — deterministic local provider with reproducible timestamps and values.
- `Data/FaultInjectingDataSource.cs` — deterministic local failure provider used only by Phase 7 diagnostics.
- `Core/ApplicationHost.cs` — validates snapshots, enforces increasing sequence numbers, and updates lifecycle health on provider failures.
- `Diagnostics/Phase7Diagnostics.cs` — dependency-free deterministic architecture checks.

## Integration

Replace the existing Phase 6 versions of `Core/ApplicationHost.cs` and `Data/SimulatedDataSource.cs` with the files in this package. Add the new `Data/DataSnapshotValidator.cs`, `Data/FaultInjectingDataSource.cs`, and `Diagnostics/Phase7Diagnostics.cs`.

After the existing Phase 6 block in `Program.Main`, add:

```csharp
var phase7 = Phase7Diagnostics.Run();
Console.WriteLine();
Console.WriteLine("=== TEST 1 Phase 7 Provider Architecture ===");
Console.WriteLine($"Checks passed: {phase7.ChecksPassed}");
Console.WriteLine($"Checks failed: {phase7.ChecksFailed}");
Console.WriteLine($"Phase 7 status: {(phase7.Passed ? "PASS" : "FAIL")}");
Console.WriteLine($"Phase 7 message: {phase7.Message}");

if (!phase7.Passed)
    return 3;
```

Keep the existing Phase 6 `host.Stop()` before this diagnostic block. Phase 7 diagnostics create their own short-lived host for lifecycle and failure testing.

## Performance impact

- Snapshot validation: LOW.
- Sequence validation: LOW.
- Deterministic simulated provider: LOW.
- Failure-injection diagnostics: LOW; runs only during explicit startup diagnostics.
- No background polling thread is introduced.

## Safety boundary

This phase remains passive/offline. It does not add another-process memory access, automated input, anti-cheat bypass, or anti-analysis behavior.
