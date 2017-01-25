using System;
namespace TileMap {
    [Serializable]
    public struct VisualTileMapData {
        public VisualTile[][] mapData;
        public Offset offset;
    }
}