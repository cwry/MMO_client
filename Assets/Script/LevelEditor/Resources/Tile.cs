using System;
using System.Collections.Generic;

namespace TileMap {
    [Serializable]
    public class Tile {
        public int x = 0;
        public int y = 0;
        public TileType type = TileType.SOLID;
        public Dictionary<int, VisualLayer> layers = new Dictionary<int, VisualLayer>();
    }
}
