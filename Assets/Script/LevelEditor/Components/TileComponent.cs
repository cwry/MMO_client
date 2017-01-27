using UnityEngine;
using System;
using System.Collections.Generic;
using TileMap;

[ExecuteInEditMode]
[Serializable]
public class TileComponent : MonoBehaviour {
    public static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
    public static GameObject visualQuadTile;

    Dictionary<int, GameObject> visualLayerQuadTiles = new Dictionary<int, GameObject>();

    Tile _tile;
    Vector3 tilePosition;

    public Tile tile {
        get {
            return _tile;
        }
        set {
            _tile = value;
            transform.position = new Vector3(_tile.x, _tile.y, 0);
            tilePosition = transform.position;
        }
    }

    Material material;
    private void Start() {
        /*material = GetComponent<Renderer>().sharedMaterial;
        material.SetTexture("_MainTex", getTexture(new VisualLayer().texture));*/
    }

    public void Update() {
        if(_tile != null) {
            if (_tile.x != tilePosition.x || _tile.y != tilePosition.y) {
                tile = tile;
                foreach (KeyValuePair<int, GameObject> vt in visualLayerQuadTiles) {
                    vt.Value.transform.position = new Vector3(tile.x, tile.y, vt.Value.transform.position.z);
                }
            }
        }
    }

    public void updateVisualLayers(params VisualLayer[] layers) {
        if (visualQuadTile == null) visualQuadTile = Resources.Load<GameObject>("VisualQuadTile");
        for (int i = 0; i < layers.Length; i++) {
            if (visualLayerQuadTiles.ContainsKey(layers[i].depth)) {
                DestroyImmediate(visualLayerQuadTiles[layers[i].depth].gameObject);
                visualLayerQuadTiles.Remove(layers[i].depth);
                tile.layers.Remove(layers[i].depth);
            }
            tile.layers.Add(layers[i].depth, layers[i]);
            GameObject visualTile = (GameObject)Instantiate(visualQuadTile, gameObject.transform);
            visualTile.transform.Rotate(0, 0, 0);
            visualTile.transform.Translate(-transform.position.x, transform.position.y, -layers[i].depth);
            Renderer renderer = visualTile.GetComponent<Renderer>();
            Material mat = new Material(renderer.sharedMaterial);
            renderer.materials = new Material[] { mat };             
            mat.SetTexture("_MainTex", getTexture(layers[i].texture));
            visualLayerQuadTiles.Add(layers[i].depth, visualTile);
        }
    }
    public Texture getTexture(string path) {
        if (!textures.ContainsKey(path)) textures.Add(path, Resources.Load<Texture>("Textures/" + path));
        return textures[path];
    }
    public void OnDestroy() {
        tile.layers = new Dictionary<int, VisualLayer>();
        foreach(KeyValuePair<int, GameObject> g in visualLayerQuadTiles) {
            DestroyImmediate(g.Value);
        }
    }
}
