# Release Automation Scripts

This directory contains scripts used by GitHub Actions for automated release management.

## Scripts

### update-changelog.sh

Automatically updates `CHANGELOG.md` when a new release tag is created.

**Usage:**
```bash
./update-changelog.sh <version>
```

**Example:**
```bash
./update-changelog.sh 0.0.2
```

**What it does:**
1. Takes the content from the `[Unreleased]` section in CHANGELOG.md
2. Creates a new version section with the current date: `## [0.0.2] - 2026-01-12`
3. Leaves `[Unreleased]` section empty for future changes
4. Updates the version comparison links at the bottom of the file

**Triggered by:**
- Release workflow when a new version tag (e.g., `v0.0.2`) is pushed

### extract-release-notes.sh

Extracts release notes for a specific version from `CHANGELOG.md`.

**Usage:**
```bash
./extract-release-notes.sh <version>
```

**Example:**
```bash
./extract-release-notes.sh 0.0.2
```

**What it does:**
1. Finds the version section in CHANGELOG.md
2. Extracts all content between that version and the next version section
3. Outputs the content to stdout for use in GitHub Release notes

**Triggered by:**
- Release workflow when creating a GitHub Release

## Workflow Integration

These scripts are automatically executed by the `release.yaml` workflow:

```yaml
# Step 1: Update CHANGELOG.md
- name: Update CHANGELOG.md
  run: |
    chmod +x .github/scripts/update-changelog.sh
    ./.github/scripts/update-changelog.sh ${{ env.VERSION }}

# Step 2: Commit changes
- name: Commit CHANGELOG.md changes
  run: |
    git config user.name "github-actions[bot]"
    git config user.email "github-actions[bot]@users.noreply.github.com"
    git add CHANGELOG.md
    git commit -m "chore: update CHANGELOG.md for v${{ env.VERSION }}"
    git push origin HEAD:main

# Step 3: Extract release notes
- name: Extract release notes from CHANGELOG
  run: |
    chmod +x .github/scripts/extract-release-notes.sh
    RELEASE_NOTES=$(./.github/scripts/extract-release-notes.sh ${{ env.VERSION }})
```

## Release Process

To create a new release:

1. **Update code and commit changes to main branch**

2. **Make sure all your changes are documented in CHANGELOG.md under `[Unreleased]`**
   ```markdown
   ## [Unreleased]

   ### Added
   - New feature X

   ### Changed
   - Modified behavior Y

   ### Fixed
   - Bug fix Z
   ```

3. **Create and push a version tag:**
   ```bash
   git tag v0.0.2
   git push origin v0.0.2
   ```

4. **The automation will:**
   - ✅ Update CHANGELOG.md (move `[Unreleased]` → `[0.0.2]`)
   - ✅ Commit the CHANGELOG changes back to main
   - ✅ Build the project
   - ✅ Run all tests
   - ✅ Create NuGet package
   - ✅ Publish to NuGet.org
   - ✅ Create GitHub Release with extracted release notes
   - ✅ Attach NuGet package to GitHub Release

## Manual Testing

You can test these scripts locally:

```bash
# Test CHANGELOG update (dry run - won't commit)
cd /path/to/Porticle.Reflection.Extensions
./.github/scripts/update-changelog.sh 0.0.2

# Test release notes extraction
./.github/scripts/extract-release-notes.sh 0.0.1
```

## Requirements

- Bash shell (available in Git Bash on Windows, or native on Linux/macOS)
- `awk` command (included in most Unix-like systems)
- Git configured with proper permissions in GitHub Actions
