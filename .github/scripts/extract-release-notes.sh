#!/bin/bash
set -e

# This script extracts release notes for a specific version from CHANGELOG.md

VERSION=$1

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

# Extract the content between [VERSION] and the next version section
awk -v version="$VERSION" '
BEGIN {
    in_version = 0
    found = 0
}

# Found the version section we are looking for
$0 ~ "^## \\[" version "\\]" {
    in_version = 1
    found = 1
    next
}

# Hit the next version section or links section, stop
in_version == 1 && (/^## \[/ || /^\[.*\]:/) {
    exit
}

# Print lines within the version section
in_version == 1 {
    print $0
}

END {
    if (found == 0) {
        print "Release notes for version " version
    }
}
' "$CHANGELOG_FILE"
