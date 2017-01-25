using System;

namespace TileMap {
    [Serializable]
    public class VisualLayer {
        public int tileSet = 0;
        public int textureID = 0;
        public int depth = 0;
        public int rotation = 0;
        public bool flipX = false;
    }
}