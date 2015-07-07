using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreateSimulationMap : MonoBehaviour {
	/// <summary>
	/// 出力先キャンバス
	/// </summary>
	public GameObject TargetCanvas;
	public Image bg;

	public float MapScreenOffsetX = 0;
	public float MapScreenOffsetY = 0;
	public const int ChipSizeX = 64;
	public const int ChipSizeY = 64;
	public const int MapTilePosXMax = 8;
	public const int MapTilePosYMax = 8;

	private int[,] map_chip_list = new int[,]{
		{1,0,1,1,0},
		{0,1,1,0,0}
	};
	
	private Dictionary<int, string> tempMapChipPrefabList
		= new Dictionary<int, string>()
	{
		{1, "SimulateBattle/WoodChip"},
	};

	private Dictionary<int, MapChip> mapChip
		= new Dictionary<int, MapChip>();

	private float CanvasHeight;
	private float CanvasWidth;

	private float MapPosX;
	private float MapPosY;

	//キャラクターデータ
	private GameObject CharcterImage;
	private int playerMoveMentRange = 3;
	private MapPoint tilePos;
	private GameObject dispMoveChip;


	void Awake () {
		foreach(int key in tempMapChipPrefabList.Keys) {
			MapChip tempMapChip = new MapChip(key, tempMapChipPrefabList[key]);
			tempMapChip.prefab = (GameObject)Resources.Load (tempMapChip.prefabName);
			mapChip.Add (key, tempMapChip);
		}
		dispMoveChip = (GameObject)Resources.Load ("SimulateBattle/TileUmi");
	}

	// Use this for initialization
	void Start () {
		CanvasWidth = TargetCanvas.GetComponent<RectTransform> ().sizeDelta.x;
		CanvasHeight = TargetCanvas.GetComponent<RectTransform> ().sizeDelta.y;
		setBg();
		MapScreenOffsetX = bg.GetComponent<RectTransform> ().anchoredPosition.x; 
		MapScreenOffsetY = bg.GetComponent<RectTransform> ().anchoredPosition.y;

		for (int i=0; i < map_chip_list.GetLength(0); i++) {
			for (int j=0; j < map_chip_list.GetLength(1); j++) {
				if(map_chip_list[i,j] == 1){
					Assert.IsTrue(mapChip.ContainsKey(map_chip_list[i,j]));
					GameObject MapChip= Instantiate (mapChip[map_chip_list[i,j]].prefab) as GameObject;
					MapChip.transform.SetParent (bg.transform, false);
					MapChip.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (i*ChipSizeX,-j*ChipSizeY, 0);
					MapChip.GetComponent<RectTransform> ().anchorMin = new Vector2(0,1);
					MapChip.GetComponent<RectTransform> ().anchorMax = new Vector2(0,1);
					MapChip.GetComponent<RectTransform> ().pivot = new Vector2(0,1);
				}
			}
		}

		GameObject CharcterImageResouce = (GameObject)Resources.Load ("SimulateBattle/tak");
		CharcterImage= Instantiate (CharcterImageResouce) as GameObject;
		CharcterImage.transform.SetParent (bg.transform, false);
		CharcterImage.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0,0, 0);
		this.tilePos = new MapPoint(0, 0);
		this.drawMovementRange();
		this.dispOrderController();
	}
	
	Vector2 GetLocalPosition(float x,float y){
		return new Vector2 (x - MapScreenOffsetX, CanvasHeight - y + MapScreenOffsetY);
	}

	Vector2 GetWorldPosition(float x,float y){
		return new Vector2 (x + MapScreenOffsetX, y + MapScreenOffsetY);
	}

	MapPoint GetMapTileAtPosition(float x,float y){
		return new MapPoint(Mathf.FloorToInt(x/ChipSizeX), Mathf.FloorToInt(y/ChipSizeY));

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
			this.tilePos = GetMapTileAtPosition(LocalPos.x,LocalPos.y);
			Vector2 UnitPos = new Vector2(tilePos.x * ChipSizeX, tilePos.y * ChipSizeY);
			//Debug.Log("LocalPos:"+LocalPos);
			Debug.Log("TilePos:"+tilePos);



			//CharcterImage.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (LocalPos.x,-LocalPos.y, 0);
			CharcterImage.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (UnitPos.x,-UnitPos.y, 0);

			//Debug.Log("SetChip"+GetManhattanDistance(0.0f,0.0f,TilePos.x,TilePos.y));
		}
	}

	private void drawMovementRange() {
		for(int rangeX = -1 * this.playerMoveMentRange; rangeX <= this.playerMoveMentRange; rangeX++) {
			int targetX = this.tilePos.x + rangeX;
			for(int rangeY = -1 * this.playerMoveMentRange; rangeY <= this.playerMoveMentRange; rangeY++) {
				int targetY = this.tilePos.y + rangeY;
				if(!this.outOfBorders(targetX, targetY)) {
					if(this.GetManhattanDistance(this.tilePos.x, this.tilePos.y, targetX, targetY) <= this.playerMoveMentRange) {
						// 移動可能
						GameObject tempTile = Instantiate (dispMoveChip) as GameObject;
						tempTile.transform.SetParent (bg.transform, false);
						tempTile.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (targetX * ChipSizeX, -1 * targetY * ChipSizeY, 0);
					}
				}
			}
		}
	}
	
	private bool outOfBorders(int xPos, int yPos) {
		if(xPos < 0) return true;
		if(xPos >= MapTilePosXMax) return true;
		if(yPos < 0) return true;
		if(yPos >= MapTilePosYMax) return true;
		return false;
	}

	private void setBg() {
		Image[] images = (Image[])GameObject.FindObjectsOfType(typeof(Image));
		for(int i = 0;i < images.Length; i++) {
			if(images[i].name == "bg") {
				bg = images[i];
			}
		}
	}

	private void dispOrderController() {
		int idx = 0;
		Image[] items = bg.GetComponentsInChildren<Image>();
		foreach(Image item in items) {
			if(item.gameObject.layer == LayerMask.NameToLayer ("MoveDisp")) {
				item.transform.SetSiblingIndex(idx);
				idx++;
			}
		}
		foreach(Image item in items) {
			if(item.gameObject.layer == LayerMask.NameToLayer ("MapChip")) {
				item.transform.SetSiblingIndex(idx);
				idx++;
			}
		}
		foreach(Image item in items) {
			if(item.gameObject.layer == LayerMask.NameToLayer ("Unit")) {
				item.transform.SetSiblingIndex(idx);
				idx++;
			}
		}
	}

	struct MapPoint{
		public int x;
		public int y;
		public MapPoint(int _x, int _y)
		{
			this.x = _x;
			this.y = _y;
		}
	};
}
