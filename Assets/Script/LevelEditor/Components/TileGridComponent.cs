using UnityEngine;
using System;
using TileMap;

[Serializable]
public class TileGridComponent : MonoBehaviour {
    private void Start() {
        TileMapCreator mapCreator = new TileMapCreator();
        mapCreator.insertColumn(122);
        mapCreator.insertColumn(-347);
        mapCreator.insertColumn(147);
        mapCreator.insertRow(155);
        mapCreator.insertRow(-288);
        mapCreator.insertRow(286);
        Debug.Log(mapCreator.tileList.Count + " columns");
        Debug.Log(mapCreator.tileList[0].Count + " rows");
        Debug.Log(mapCreator.tileList[3].Count + " fields in 3. column");
        Debug.Log(JsonUtility.ToJson(mapCreator.tileList[4][14]));
        Debug.Log(JsonUtility.ToJson(mapCreator.getTile(-343, -1000)));
    }
    	
	// Update is called once per frame
	void Update () {
	
	}
}
