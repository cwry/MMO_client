using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using TileMap;
using System;

[ExecuteInEditMode]
public class MapEditor : EditorWindow {
    bool blockStartButton = true;
    bool startButton;
    bool canDelete = false;
    TileMapCreator mapCreator;
    List<GameObject> quadTileList = new List<GameObject>();
    GameObject quadTile;

    public void OnEnable() {
        quadTile = Resources.Load<GameObject>("QuadTile");
        Selection.selectionChanged = () => {
            if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<TileComponent>()) {
                canDelete = true;
            } else {
                canDelete = false;
            }
        };
    }

    [MenuItem("Window/MapEditor")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(MapEditor));
    }

    public void OnGUI() {
        drawStartButton();
        if (GUILayout.Button("create second tile"
            , GUILayout.ExpandHeight(false)
            , GUILayout.ExpandWidth(false)
        )) {
            createTile(3, -3).ForEach(t => quadTileList.Add(t));
            createTile(-3, 3).ForEach(t => quadTileList.Add(t));
        }
        if (canDelete) {
            if (GUILayout.Button("delete column"
                , GUILayout.ExpandHeight(false)
                , GUILayout.ExpandWidth(false)
            )) {
                deleteColumn((int)Selection.activeGameObject.transform.position.x);
            }
            if (GUILayout.Button("delete row"
                , GUILayout.ExpandHeight(false)
                , GUILayout.ExpandWidth(false)
            )) {
                deleteRow((int)Selection.activeGameObject.transform.position.y);
            }
        }
    }

    public List<GameObject> createTile(int x, int y) {
        List<GameObject> insertedQuadTiles = new List<GameObject>();
        List<Tile> insertedTiles = mapCreator.insertColumn(x);
        mapCreator.insertRow(y).ForEach(t => insertedTiles.Add(t));
        insertedTiles.ForEach(t => {
            GameObject qTile = Instantiate(quadTile);
            TileComponent tileComponent = qTile.GetComponent<TileComponent>();
            tileComponent.tile = t;
            tileComponent.updateVisualLayers(new VisualLayer());
            insertedQuadTiles.Add(qTile);
        });
        return insertedQuadTiles;
    }

    public void drawStartButton() {
        blockStartButton = EditorGUILayout.BeginToggleGroup("Allow new map", blockStartButton);
        startButton = GUILayout.Button("Start new Map"
            , GUILayout.ExpandHeight(false)
            , GUILayout.ExpandWidth(false)
            );
        if (startButton) {
            blockStartButton = false;
            quadTileList.ForEach(qt => DestroyImmediate(qt));
            quadTileList = new List<GameObject>();
            mapCreator = new TileMapCreator();
            GameObject qTile = Instantiate(quadTile);
            TileComponent tileComponent = qTile.GetComponent<TileComponent>();
            tileComponent.tile = mapCreator.tileList[0][0];
            VisualLayer layer = new VisualLayer();
            layer.texture = "water_1";
            tileComponent.updateVisualLayers(layer);
            quadTileList.Add(qTile);
        }
        EditorGUILayout.EndToggleGroup();
    }
    public void deleteColumn(int x) {
        List<GameObject> deletedQuadTiles = new List<GameObject>();
        Debug.Log(quadTileList.Count + ": quadtilelist before");
        quadTileList.ForEach(t => {
            if (t.transform.position.x == x) {
                deletedQuadTiles.Add(t);
                DestroyImmediate(t);
            }
        });
        deletedQuadTiles.ForEach(t => quadTileList.Remove(t));
        Debug.Log(quadTileList.Count + ": quadtilelist after");
        Func<string> tlf = () => {
            List<Tile> tl = new List<Tile>();
            mapCreator.tileList.ForEach(l => l.ForEach(t => tl.Add(t)));
            return tl.Count.ToString();
        };
        Debug.Log(tlf() + ": mapcreator before");
        mapCreator.deleteColumn(x);
        Debug.Log(tlf() + ": mapcreator after");
    }
    public void deleteRow(int y) {
        List<GameObject> deletedQuadTiles = new List<GameObject>();
        quadTileList.ForEach(t => {
            if (t.transform.position.y == y) {
                deletedQuadTiles.Add(t);
                DestroyImmediate(t);
            }
        });
        deletedQuadTiles.ForEach(t => quadTileList.Remove(t));
        mapCreator.deleteRow(y);
    }
}
