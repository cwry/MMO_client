using System;

namespace TileMap {
    [Serializable]
    public struct LogicTileMapData {
        public LogicTile[][] mapData;
        public Offset offset;
    }
}