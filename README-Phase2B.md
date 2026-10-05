# Phase 2B — root-project integration

Extract the contents of this archive **directly into**:

`C:\Users\suryansh\source\repos\TEST 1\TEST 1`

Do not create a `TEST1_Phase2B` or `TEST1_Phase2A` subdirectory.

Merge the `DEBUG_MODE` PropertyGroup from `TEST 1.csproj.phase2b.props.xml` into the existing `TEST 1.csproj`. Do not replace the existing project file.

Then run:

```powershell
dotnet clean
dotnet restore
dotnet build -c Debug -p:Platform=x64
```

Phase 2B is passive diagnostics only. It does not read another process, inject input, use a driver, or manipulate game state.

Performance impact: LOW. The diagnostics are startup/on-demand work; there is no continuous polling loop.

The supplied offset dump supports offset-reference validation, but it does not establish an existing authentication, licensing, encryption, hardware-fingerprinting, or anti-debug implementation.
