using System;

namespace TileMap {
    [Serializable]
    public class VisualLayer {
        public string texture = "grass_1";
        public int depth = -2;
        public int rotation = 0;
        public bool flipX = false;
    }
}