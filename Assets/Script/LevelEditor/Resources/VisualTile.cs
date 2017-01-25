using System;
using System.Collections.Generic;
namespace TileMap {
    [Serializable]
    public class VisualTile {
        public int x = 0;
        public int y = 0;
        public Dictionary<int, VisualLayer> layers = new Dictionary<int, VisualLayer>();
    }
}