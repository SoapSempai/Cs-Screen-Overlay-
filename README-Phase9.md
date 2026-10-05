# TEST 1 — Phase 9 Passive Telemetry

Phase 9 adds a passive, in-process telemetry provider. It accepts JSON telemetry
supplied by the application or another explicitly integrated telemetry source,
validates it, and exposes the latest valid DataSnapshot.

## Added files

- Data/TelemetryMessage.cs
- Data/TelemetryMessageParser.cs
- Data/TelemetryDataSource.cs
- Diagnostics/Phase9Diagnostics.cs

## Performance impact

LOW. The provider performs small JSON parsing and validation operations only
when telemetry is published or captured.

## Safety boundary

This provider does not open another process, read or write process memory,
inject code, manipulate CS2, automate mouse/keyboard input, or bypass
anti-cheat/anti-analysis mechanisms.

## Integration

After the existing Phase 8 block in Program.Main, replace the existing final
Phase 8 return with:

```csharp
var phase9 = Phase9Diagnostics.Run();

Console.WriteLine();
Console.WriteLine("=== TEST 1 Phase 9 Passive Telemetry ===");
Console.WriteLine($"Phase 9 checks passed: {phase9.ChecksPassed}");
Console.WriteLine($"Phase 9 checks failed: {phase9.ChecksFailed}");
Console.WriteLine($"Phase 9 result: {phase9.Message}");

return phase9.Passed ? 0 : 5;
```

Do not place this after an earlier return.

## Example telemetry payload

```json
{
  "Timestamp": "2026-01-01T00:00:01Z",
  "Source": "local-telemetry",
  "Sequence": 1,
  "Values": {
    "sample.value": 42.5
  }
}
```
