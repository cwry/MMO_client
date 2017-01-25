using System;

namespace TileMap {
    [Serializable]
    public class LogicTile {
        public TileType type = TileType.SOLID;
        public int x = 0;
        public int y = 0;
    }
}