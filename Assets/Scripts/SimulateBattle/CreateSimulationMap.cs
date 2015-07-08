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
	//プレイヤーの移動範囲
	private int playerMoveMentRange = 3;
	//クリック座標の
	private MapPoint clickTilePos;
	//移動可能範囲を示すチップ
	private GameObject dispMoveChip;
	//移動可能な範囲の参照
	List<GameObject> moveAblePosObjectList;

	void Awake () {
		foreach(int key in tempMapChipPrefabList.Keys) {
			MapChip tempMapChip = new MapChip(key, tempMapChipPrefabList[key]);
			tempMapChip.prefab = (GameObject)Resources.Load (tempMapChip.prefabName);
			mapChip.Add (key, tempMapChip);
		}
		dispMoveChip = (GameObject)Resources.Load ("SimulateBattle/TileUmi");
		this.clickTilePos = new MapPoint(0, 0);
		moveAblePosObjectList = new List<GameObject>();
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
		this.clickTilePos = new MapPoint(0, 0);
		this.drawMovementRange();
		this.dispOrderController();
	}

	//Unity全体の座標系から、表示領域内の座標を取得する
	Vector2 ConvertWorldToLocal(float worldx,float worldy){
		return new Vector2 (worldx - MapScreenOffsetX, CanvasHeight - worldy + MapScreenOffsetY);
	}

	//表示範囲内の座標から、Unity全体の座標系に直す
	Vector2 ConvertLocalToWorld(float localx,float localy){
		return new Vector2 (localx + MapScreenOffsetX, localy + MapScreenOffsetY);
	}

	//ローカル座標よりマス目の位置を取得する
	MapPoint ConvertLocalPositionToTile(float localx,float localy){
		return new MapPoint(Mathf.FloorToInt(localx/ChipSizeX), Mathf.FloorToInt(localy/ChipSizeY));

	}

	//タイル座標からローカル座標系への変換
	Vector2 ConvertTileToLocal(int i,int j){
		return new Vector2 (i*ChipSizeX,j*ChipSizeY);
	}

	//２点のタイル座標系からマンハッタン距離を求める
	float GetManhattanDistance (float startI, float startJ,float endI, float endJ) {
		float distance = Mathf.Abs(startI -endI) +Mathf.Abs(startJ -endJ);
		return distance;
	}


	// Update is called once per frame
	void Update () {
		// マウス入力で左クリックをした瞬間
		if (Input.GetMouseButtonDown (0)) {
			Vector2 LocalPos = ConvertWorldToLocal(Input.mousePosition.x,Input.mousePosition.y);
			this.clickTilePos = ConvertLocalPositionToTile(LocalPos.x,LocalPos.y);
			Vector2 UnitPos = ConvertTileToLocal(clickTilePos.x , clickTilePos.y );
			//クリックした位置にキャラクターを表示させる
			CharcterImage.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (UnitPos.x,-UnitPos.y, 0);
			drawMovementRange();
			this.dispOrderController();
		}

	}

	//移動可能オブジェクトな場所に画像を配置する
	private void drawMovementRange() {
		for (int i=moveAblePosObjectList.Count-1; i >= 0 ; --i) {
			Destroy(moveAblePosObjectList[i]);
		}
		moveAblePosObjectList.Clear ();

		for(int rangeX = -1 * this.playerMoveMentRange; rangeX <= this.playerMoveMentRange; rangeX++) {
			int targetX = this.clickTilePos.x + rangeX;
			for(int rangeY = -1 * this.playerMoveMentRange; rangeY <= this.playerMoveMentRange; rangeY++) {
				int targetY = this.clickTilePos.y + rangeY;
				if(!this.outOfTileBorders(targetX, targetY)) {
					if(this.GetManhattanDistance(this.clickTilePos.x, this.clickTilePos.y, targetX, targetY) <= this.playerMoveMentRange) {
						// 移動可能領域に新規オブジェクトを配置可視化する
						GameObject tempTile = Instantiate (dispMoveChip) as GameObject;
						tempTile.transform.SetParent (bg.transform, false);
						tempTile.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (targetX * ChipSizeX, -1 * targetY * ChipSizeY, 0);
						moveAblePosObjectList.Add (tempTile);
					}
				}
			}
		}
	}

	//タイルが表示範囲内かをチェックする
	private bool outOfTileBorders(int xTilePos, int yTilePos) {
		if(xTilePos < 0) return true;
		if(xTilePos >= MapTilePosXMax) return true;
		if(yTilePos < 0) return true;
		if(yTilePos >= MapTilePosYMax) return true;
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

	//表示順番の調整　MoveDisp->MapChip->Unitの順番に並べる
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
