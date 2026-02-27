# Argument Parsing Refactor: Fix Defaults & Introduce ArgumentStyle

## Status: Implemented

## Summary
Fixed swapped default value separators in `ArgumentFormatOptions` and introduced a formal `ArgumentStyle` enum for style presets.

## Problem
The defaults were backwards relative to industry convention:
- Windows had `/name=value` instead of `/name:value`
- Unix had `--name:value` instead of `--name=value`

## Changes Made

### New: `ArgumentStyle.cs`
Enum with `Posix`, `Windows`, `Platform` values.

### Modified: `ArgumentFormatOptions.cs`
- Fixed defaults: Windows = `/` + `:`, Unix = `--` + `=`
- Added static presets: `Posix`, `Windows`, `Platform`
- Added `ForStyle(ArgumentStyle)` factory method
- Added `ResolvePlatformStyle()` with `BAM_ARG_STYLE` env var override
- Changed property setters to `init` for immutability

### Modified: `BamConsoleContext.cs`
- `Usage()` method now uses `ArgumentFormatOptions.Default.Prefix` and `.ValueSeparator` instead of hardcoded `/` and `:`

### Modified: `ArgumentFormatOptionsShould.cs`
- Fixed `DefaultHasExpectedValues` assertions (`:` on Windows, `=` on Unix)
- Added: `PosixStyleHasDoubleDashAndEquals`, `WindowsStyleHasSlashAndColon`, `PlatformStyleMatchesRuntimePlatform`, `ForStyleReturnCorrectOptions`

### Modified: `DefaultArgumentParserShould.cs`
- Renamed `ParseWithDefaultOptions` to `ParseWithPosixOptions` using explicit `ArgumentFormatOptions.Posix`
- Added `ParseWithWindowsOptions` test

## Backward Compatibility
- Explicit constructor calls (`new ArgumentFormatOptions("--", '=')`) unchanged
- `ArgumentFormatOptions.Default` behavior changes (this is the intended fix)
