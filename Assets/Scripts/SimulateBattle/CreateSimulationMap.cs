using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateSimulationMap : MonoBehaviour {
	/// <summary>
	/// 出力先キャンバス
	/// </summary>
	public GameObject TargetCanvas;

	public const int MapScreenOffsetX = 0;
	public const int MapScreenOffsetY = 0;
	public const int ChipSizeX = 64;
	public const int ChipSizeY = 64;

	private int[,] map_chip_list = new int[,]{
		{0,1,1,0,1},
		{0,0,1,1,0}
	};
	private float CanvasHeight;
	private float CanvasWidth;


	
	// Use this for initialization
	void Start () {
		CanvasWidth = TargetCanvas.GetComponent<RectTransform> ().sizeDelta.x;
		CanvasHeight = TargetCanvas.GetComponent<RectTransform> ().sizeDelta.y;

		GameObject MapChipResouce = (GameObject)Resources.Load ("SimulateBattle/WoodChip");
		for (int i=0; i < map_chip_list.GetLength(0); i++) {
			for (int j=0; j < map_chip_list.GetLength(1); j++) {
				if(map_chip_list[i,j] == 1){
					GameObject MapChip= Instantiate (MapChipResouce) as GameObject;
					MapChip.transform.SetParent (TargetCanvas.transform, false);
					MapChip.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (i*ChipSizeX,-j*ChipSizeX, 0);
					MapChip.GetComponent<RectTransform> ().anchorMin = new Vector2(0,1);
					MapChip.GetComponent<RectTransform> ().anchorMax = new Vector2(0,1);
					MapChip.GetComponent<RectTransform> ().pivot = new Vector2(0,1);
				}
			}
		}
	}
	
	Vector2 GetLocalPosition(float x,float y){
		return new Vector2 (x - MapScreenOffsetX, CanvasHeight - y - MapScreenOffsetY);
	}

	Vector2 GetWorldPosition(float x,float y){
		return new Vector2 (x + MapScreenOffsetX, y + MapScreenOffsetY);
	}

	Vector2 GetMapTileAtPosition(float x,float y){
		return new Vector2 (Mathf.Floor(x/ChipSizeX), Mathf.Floor(y/ChipSizeY));
	}

	Vector2 GetMapPositionAtTile(int i,int j){
		return new Vector2 (i*ChipSizeX,j*ChipSizeY);
	}

	float GetManhattanDistance (float startI, float startJ,float endI, float endJ) {
		float distance = Mathf.Abs(startI -endI) +Mathf.Abs(startJ -endJ);
		return distance;
	}


	// Update is called once per frame
	void Update () {
		// マウス入力で左クリックをした瞬間
		if (Input.GetMouseButtonDown (0)) {
			Vector2 LocalPos = GetLocalPosition(Input.mousePosition.x,Input.mousePosition.y);
			Vector2 TilePos = GetMapTileAtPosition(LocalPos.x,LocalPos.y);
			Debug.Log(TilePos);

			Debug.Log("SetChip"+GetManhattanDistance(0.0f,0.0f,TilePos.x,TilePos.y));
		}
	}
}
