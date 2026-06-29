# RaiUtils 3.13.0 Release Notes

## Summary

- Releases `RaiUtils` version `3.13.0`.
- Aligns the coordinated package baseline with `OsLibCore 3.13.0`.
- Carries forward the current docs and class-diagram release markers without public API changes from `3.12.1`.

## Validation

- `dotnet build RaiUtils.csproj --nologo -v minimal`
- NuGet publishing remains wired through the parent sequential release chain and the tag-triggered `publish-nuget.yml` workflow.
