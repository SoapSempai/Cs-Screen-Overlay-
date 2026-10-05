# TEST 1 — Character Visual Analysis

This version replaces the old purple-pixel detector with a screen-only character-reference matcher.

## What changed

- Purple-pixel detection is removed from the active analysis pipeline.
- Character reference images can be loaded from PNG/JPG/BMP files.
- References are validated for file size, dimensions and decoder integrity.
- Desktop frames are validated before analysis.
- The analyzer searches the captured screen for the best matching reference.
- The existing bounding-box overlay remains.
- A 2D estimated skeleton is drawn inside the detected character box.
- Confidence and analysis timing are displayed in the dashboard.
- Diagnostics use staged validation/reporting inspired by the supplied ZIP.
- No process memory access, DLL mapping, remote-thread execution or detector-driven mouse control is included.
- F8/F9 and fixed-coordinate Move/Click remain manual test-harness features only.

## Important limitation

The current matcher is a lightweight local reference matcher. It works best when reference images are tightly cropped around the character and have a visual appearance similar to the live screen.

The skeleton is geometric/estimated from the detected box. It is NOT a real pose-estimation model.

For accurate head/elbow/wrist/knee/ankle landmarks across arbitrary poses, the next upgrade should use a dedicated pose-estimation model (for example an ONNX pose model) rather than guessing joints from a rectangle.

## Using Google images

Save/download a few tightly cropped images of the character locally, then use:

LOAD REFERENCE IMAGES

Do not give the application a Google search page or a URL. Give it actual image files.

## Installation

The included Install-CharacterAnalysis.ps1 backs up the existing TEST 1 source files and replaces the application source with this version.

Run PowerShell in the extracted package:

    Set-ExecutionPolicy -Scope Process Bypass
    .\Install-CharacterAnalysis.ps1 -ProjectPath "C:\Users\suryansh\source\repos\TEST 1\TEST 1"

Then:

    cd "C:\Users\suryansh\source\repos\TEST 1\TEST 1"
    dotnet build
    dotnet run

The installer creates a timestamped backup directory before replacing files.

## Safety boundary

The detector is never connected to SafeInputController. Character detections only update the visual overlay and dashboard.
