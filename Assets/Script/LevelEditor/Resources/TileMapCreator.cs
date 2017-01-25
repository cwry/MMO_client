using System;
using System.Collections.Generic;

namespace TileMap {
    [Serializable]
    public class TileMapCreator {
        public List<List<Tile>> tileList = new List<List<Tile>>();
        int offsetX = 0;
        int offsetY = 0;
        int width = 1;
        int height = 1;

        public TileMapCreator() {
            tileList.Insert(0, new List<Tile>());
            tileList[0].Insert(0, new Tile());
        }

        public void insertColumn(int x) {
            if (x < 0 && -x > offsetX) {
                int diff = offsetX + x;
                for (int i = 0; i < -diff; i++) {
                    tileList.Insert(i, new List<Tile>());
                    for (int c = 0; c < tileList[i + 1].Count; c++) {
                        Tile tile = new Tile();
                        tile.x = x + i;
                        tile.y = tileList[i + 1][c].y;
                        tileList[i].Insert(c, tile);
                    }
                }
                offsetX = -x;
                width = tileList.Count;
            } else if (x > 0 && x > width - offsetX) {
                int diff = x + offsetX - width;
                int startIndex = width;
                for (int i = startIndex; i <= startIndex + diff; i++) {
                    tileList.Insert(i, new List<Tile>());
                    for (int r = 0; r < tileList[i - 1].Count; r++) {
                        Tile tile = new Tile();
                        tile.x = i - offsetX;
                        tile.y = tileList[i - 1][r].y;
                        tileList[i].Insert(r, tile);
                    }
                }
                width = tileList.Count;
            }
        }
        public void insertRow(int y) {
            if (y < 0 && -y > offsetY) {
                int diff = offsetY + y;
                offsetY = -y;
                tileList.ForEach(column => {
                    for (int i = 0; i < -diff; i++) {
                        Tile tile = new Tile();
                        tile.x = column[0].x;
                        tile.y = y + i;
                        column.Insert(i, tile);
                    }
                });
                height = tileList[0].Count;
            } else if (y > 0 && y > height - offsetY) {
                int diff = y + offsetY - height;
                int startIndex = height;
                tileList.ForEach(column => {
                    for (int i = startIndex; i <= startIndex + diff; i++) {
                        Tile tile = new Tile();
                        tile.x = column[0].x;
                        tile.y = i - offsetY;
                        column.Insert(i, tile);
                    }
                });
                height = tileList[0].Count;
            }
        }
        public Tile getTile(int x, int y) {
            if (x + offsetX >= width || x + offsetX < 0 || y + offsetY >= height || y + offsetY < 0) return null;
            return tileList[x + offsetX][y + offsetY];
        }
        public void deleteColumn(int x) {
            if(x + offsetX == width -1 || x + offsetX == 0) {
                tileList[x + offsetX] = null;
            }
        }
        public void deleteRow(int y) {
            if(y + offsetY == height - 1 || y + offsetY == 0) {
                tileList.ForEach(column => {
                    column[y + offsetY] = null;
                });
            }
        }

    }
}