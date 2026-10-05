# Phase 4 — Defensive Protection

This package adds a small, deployment-oriented protection layer.

## Included

- `Protection/ProtectionRuntime.cs`
- Configuration integrity validation using SHA-256.
- Release enforcement through `TEST1_CONFIG_SHA256`.
- `DEBUG_MODE` bypass for development.

## Performance

LOW. The check hashes one local configuration file during startup.

## Scope

This phase does not:
- inspect or modify another process;
- access CS2 memory;
- write game memory;
- generate automated input;
- bypass anti-cheat;
- implement anti-debugging/evasion behavior.

## Integration

Add the file under the project's `Protection` directory.

Call:

```csharp
var protection = ProtectionRuntime.ValidateConfiguration("appsettings.ini");
```

In Release, configure `TEST1_CONFIG_SHA256` to the trusted SHA-256 value of the deployed `appsettings.ini`.

Do not enable enforcement until the deployment process has established that trusted value.
