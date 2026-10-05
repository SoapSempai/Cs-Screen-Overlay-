# TEST 1 — Phase 8 Release Validation

Phase 8 adds a deterministic release-validation gate over the already validated
protection, runtime, offset, service, and provider layers.

## Added file

- `Diagnostics/Phase8Validation.cs`

## Performance impact

LOW. The validation runs synchronously and performs only the existing local
diagnostic/provider checks. It does not introduce background polling, process
memory access, automated input, anti-cheat bypass, or anti-analysis behavior.

## Integration

After the existing Phase 7 diagnostic block in `Program.Main`, add:

```csharp
var phase8 = Phase8Validation.Run("appsettings.ini");

Console.WriteLine();
Console.WriteLine("=== TEST 1 Phase 8 Release Validation ===");
Console.WriteLine($"Phase 8 checks passed: {phase8.ChecksPassed}");
Console.WriteLine($"Phase 8 checks failed: {phase8.ChecksFailed}");
Console.WriteLine($"Phase 8 result: {phase8.Message}");

return phase8.Passed ? 0 : 4;
```

Do not place this after an earlier `return`.

## Release gate

A successful release run should report:

- Phase 8 checks passed: 2
- Phase 8 checks failed: 0
- `All Phase 8 release validation checks passed.`

The existing Phase 4 integrity check remains the startup gate before this
validation executes.
