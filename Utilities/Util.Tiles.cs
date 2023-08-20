using System.Reflection;
using Terraria.ID;

namespace TerraUtil.Utilities;
public static partial class Util
{
    internal static FieldInfo TileId;

    /// <summary>
    /// Retrieves the tile at the location of <paramref name="point"/> in tile coordinates.
    /// </summary>
    /// <param name="point">The tile coordinates of the tile.</param>
    /// <returns>The <see cref="Tile"/> from <see cref="Main.tile"/> at the location of <paramref name="point"/>.</returns>
    public static Tile ToTile(this Point point)
    {
        return Main.tile[point];
    }

    /// <summary>
    /// Retrieves the tile at the location of <paramref name="vector"/> in world coordinates.
    /// </summary>
    /// <param name="vector">The world coordinates of the tile.</param>
    /// <returns>The <see cref="Tile"/> from <see cref="Main.tile"/> at the location of <paramref name="vector"/>.</returns>
    public static Tile ToTile(this Vector2 vector)
    {
        return ToTile(vector.ToTileCoordinates());
    }

    // TODO: finish
    // id = y + x * height
    // y = id - (x * height)
    // x = (id - y)/height
    public static ushort X(this Tile tile)
    {
        return (ushort)((uint)TileId.GetValue(tile) / Main.tile.Width - Main.tile.Height);
    }

    public static ushort Y(this Tile tile)
    {
        return (ushort)((uint)TileId.GetValue(tile) / Main.tile.Width - Main.tile.Height);
    }
}