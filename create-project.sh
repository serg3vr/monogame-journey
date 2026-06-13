#!/usr/bin/env bash
set -euo pipefail

NAME="${1:?Usage: $0 <ProjectName>}"
ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_DIR="$ROOT_DIR/$NAME"

if [ -d "$PROJECT_DIR" ]; then
  echo "Error: Directory '$PROJECT_DIR' already exists."
  exit 1
fi

echo "🎮 Creating MonoGame cross-platform desktop project: $NAME"

dotnet new mgdesktopgl -o "$PROJECT_DIR/$NAME" -n "$NAME"

cat > "$PROJECT_DIR/$NAME.slnx" <<SLNX
<Solution>
  <Project Path="$NAME/$NAME.csproj" />
</Solution>
SLNX

cat > "$PROJECT_DIR/.editorconfig" <<SLNX
[*.cs]
csharp_new_line_before_open_brace = methods, properties, accessors, types
csharp_new_line_before_for = false
csharp_new_line_before_else = false
csharp_new_line_before_catch = false
csharp_new_line_before_finally = false
SLNX


echo ""
echo "✅ Project '$NAME' created at: $PROJECT_DIR"
echo ""
echo "To run:"
echo "  cd $NAME && dotnet run --project $NAME"
