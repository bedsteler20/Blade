dotnet build --sc -c Release

mkdir -p obj/Flatpak/State
mkdir -p obj/Flatpak/Build

flatpak-builder --user --install --force-clean \
                --state-dir=obj/Flatpak/State  \
                obj/Flatpak/Build              \
                dev.bedsteler20.SharpGames.yaml