#!/bin/bash
set -e

# This script updates CHANGELOG.md when a new version tag is created
# It moves content from [Unreleased] to the new version section

VERSION=$1
DATE=$(date +%Y-%m-%d)

if [ -z "$VERSION" ]; then
    echo "Error: Version parameter is required"
    echo "Usage: $0 <version>"
    exit 1
fi

CHANGELOG_FILE="CHANGELOG.md"

if [ ! -f "$CHANGELOG_FILE" ]; then
    echo "Error: $CHANGELOG_FILE not found"
    exit 1
fi

echo "Updating CHANGELOG.md for version $VERSION (date: $DATE)"

# Create a temporary file
TEMP_FILE=$(mktemp)

# Read the changelog and update it
awk -v version="$VERSION" -v date="$DATE" '
BEGIN {
    in_unreleased = 0
    unreleased_content = ""
    header_printed = 0
}

# Print everything before [Unreleased]
/^## \[Unreleased\]/ {
    in_unreleased = 1
    print $0
    print ""
    next
}

# If we are in the unreleased section, collect content
in_unreleased == 1 && /^## \[/ {
    # We hit the next version section, so print the new version section
    print "## [" version "] - " date
    print unreleased_content
    in_unreleased = 0
    print $0
    next
}

in_unreleased == 1 {
    # Collect unreleased content (skip empty lines at the start)
    if (unreleased_content != "" || $0 !~ /^[[:space:]]*$/) {
        unreleased_content = unreleased_content $0 "\n"
    }
    next
}

# Update the links at the bottom
/^\[Unreleased\]:/ {
    print "[Unreleased]: https://github.com/Machibuse/Porticle.Reflection.Extensions/compare/v" version "...HEAD"
    print "[" version "]: https://github.com/Machibuse/Porticle.Reflection.Extensions/releases/tag/v" version
    next
}

# Skip old version link if it matches our new version (shouldn't happen, but just in case)
$0 ~ "^\\[" version "\\]:" {
    next
}

# Print all other lines
{ print $0 }
' "$CHANGELOG_FILE" > "$TEMP_FILE"

# Replace the original file
mv "$TEMP_FILE" "$CHANGELOG_FILE"

echo "CHANGELOG.md updated successfully"
echo ""
echo "New version section created: [$VERSION] - $DATE"
