using System;
using System.Collections.Generic;
using UnityEngine;
namespace TileMap {
    [Serializable]
    public class TileMapCreator {
        public List<List<Tile>> tileList = new List<List<Tile>>();
        int offsetX = 0;
        int offsetY = 0;
        int width = 1;
        int height = 1;
        delegate void mapTile(Tile tile);
        void defaultMapTile(Tile tile) { }

        public TileMapCreator() {
            tileList.Insert(0, new List<Tile>());
            tileList[0].Insert(0, new Tile());
        }

        public List<Tile> insertColumn(int x) {
            List<Tile> insertedTiles = new List<Tile>();
            if (x < 0 && -x > offsetX) {
                int diff = x + offsetX;
                for (int i = 0; i < -diff; i++) {
                    tileList.Insert(i, new List<Tile>());
                    for (int c = 0; c < tileList[i + 1].Count; c++) {
                        Tile tile = new Tile();
                        tile.x = x + i;
                        tile.y = tileList[i + 1][c].y;
                        insertedTiles.Add(tile);
                        tileList[i].Insert(c, tile);
                    }
                }
                offsetX = -x;
            } else if (x > 0 && x > width - offsetX) {
                int diff = x + offsetX - width;
                int startIndex = width;
                for (int i = startIndex; i <= startIndex + diff; i++) {
                    tileList.Insert(i, new List<Tile>());
                    for (int r = 0; r < tileList[i - 1].Count; r++) {
                        Tile tile = new Tile();
                        tile.x = i - offsetX;
                        tile.y = tileList[i - 1][r].y;
                        insertedTiles.Add(tile);
                        tileList[i].Insert(r, tile);
                    }
                }
            }
            width = tileList.Count;
            return insertedTiles;
        }

        public List<Tile> insertRow(int y) {
            List<Tile> insertedTiles = new List<Tile>();
            if (y < 0 && -y > offsetY) {
                int diff = offsetY + y;
                tileList.ForEach(column => {
                    for (int i = 0; i < -diff; i++) {
                        Tile tile = new Tile();
                        tile.x = column[0].x;
                        tile.y = y + i;
                        insertedTiles.Add(tile);
                        column.Insert(i, tile);
                    }
                });
                offsetY = -y;
            } else if (y > 0 && y > height - offsetY) {
                int diff = y + offsetY - height;
                int startIndex = height;
                tileList.ForEach(column => {
                    for (int i = startIndex; i <= startIndex + diff; i++) {
                        Tile tile = new Tile();
                        tile.x = column[0].x;
                        tile.y = i - offsetY;
                        insertedTiles.Add(tile);
                        column.Insert(i, tile);
                    }
                });
            }
            height = tileList[0].Count;
            return insertedTiles;
        }

        public Tile getTile(int x, int y) {
            if (x + offsetX >= width || x + offsetX < 0 || y + offsetY >= height || y + offsetY < 0) return null;
            return tileList[x + offsetX][y + offsetY];
        }
        public List<Tile> deleteColumn(int x) {
            Debug.Log("offsetX before: " + offsetX);
            List<Tile> deletedTiles = new List<Tile>();
            if (x + offsetX >= 0 && x + offsetX < width) {
                tileList[x + offsetX].ForEach(tile => {
                    deletedTiles.Add(tile);
                });
                tileList.RemoveAt(x + offsetX);
                width = tileList.Count;
                bool offsetReduce = false;
                if (x == 0 && !tileList.Exists(tl => tl.Exists(t => t.x > 0))) {
                    for (int i = 0; i < width; i++) {
                        tileList[i].ForEach(t => { t.x += 1; });
                    }
                    offsetReduce = true;
                } else if (x >= 0) {
                    for (int i = x + offsetX; i < width; i++) {
                        tileList[i].ForEach(tile => {
                            tile.x -= 1;
                        });
                    }
                } else {
                    for (int i = 0; i < x + offsetX; i++) {
                        tileList[i].ForEach(tile => {
                            tile.x += 1;
                        });
                    }
                    offsetReduce = true;
                }
                offsetX = (offsetReduce) ? offsetX - 1 : offsetX;

                Debug.Log("offsetX after: " + offsetX);
            }
            return deletedTiles;
        }

        public List<Tile> deleteRow(int y) {
            List<Tile> deletedTiles = new List<Tile>();
            if (y + offsetY >= 0 && y + offsetY < height) {
                tileList.ForEach(column => {
                    deletedTiles.Add(column[y + offsetY]);
                    column.Remove(column[y + offsetY]);
                });
                height = tileList[0].Count;
                bool offsetReduce = false;
                tileList.ForEach(column => {
                    if (y == 0 && !column.Exists(t => t.y > 0)) {
                        for (int i = 0; i < height; i++) {
                            column[i].y += 1;
                        }
                        offsetReduce = true;
                    } else if (y >= 0) {
                        for (int i = y + offsetY; i < height; i++) {
                            column[i].y -= 1;
                        }
                    } else {
                        for (int i = 0; i < y + offsetY; i++) {
                            column[i].y += 1;
                        }
                        offsetReduce = true;
                    }
                });
                offsetY = (offsetReduce) ? offsetY - 1 : offsetY;
            }
            return deletedTiles;
        }
    }
}