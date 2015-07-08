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

	//一つひとつのチップサイズ
	public const int ChipSizeX = 64;
	public const int ChipSizeY = 64;
	//タイルの最大数
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

	//キャラクターデータ
	private GameObject CharcterImage;
	//キャラクター
	SLGPartCharcterModel charModelData;

	//MAPの設定を持つスクリプタブルオブジェクト
	[SerializeField]
	private MapSetting setting;
	
	void Awake () {
		foreach(int key in tempMapChipPrefabList.Keys) {
			MapChip tempMapChip = new MapChip(key, tempMapChipPrefabList[key]);
			tempMapChip.prefab = (GameObject)Resources.Load (tempMapChip.prefabName);
			mapChip.Add (key, tempMapChip);
		}
	charModelData = new SLGPartCharcterModel (0, 1, 3, false, false, false);

	}

	// Use this for initialization
	void Start () {
		this.InitMapSetting ();
		this.LoadMapChip ();

		GameObject CharcterImageResouce = (GameObject)Resources.Load ("SimulateBattle/tak");
		CharcterImage= Instantiate (CharcterImageResouce) as GameObject;
		CharcterImage.transform.SetParent (bg.transform, false);
		CharcterImage.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0,0, 0);
		CharcterImage.GetComponent<SLgPartCharcterView> ().InitModel (charModelData);
	}

	void InitMapSetting()
	{
		//MAP設定の登録
		float CanvasWidth = TargetCanvas.GetComponent<RectTransform> ().sizeDelta.x;
		float CanvasHeight = TargetCanvas.GetComponent<RectTransform> ().sizeDelta.y;
		setBg();
		float MapScreenOffsetX = bg.GetComponent<RectTransform> ().anchoredPosition.x; 
		float MapScreenOffsetY = bg.GetComponent<RectTransform> ().anchoredPosition.y;
		setting.MapScreenOffsetX = MapScreenOffsetX;
		setting.MapScreenOffsetY = MapScreenOffsetY;
		setting.ChipSizeX = ChipSizeX;
		setting.ChipSizeY = ChipSizeY;
		setting.MapTilePosXMax = MapTilePosXMax;
		setting.MapTilePosYMax = MapTilePosYMax;
		setting.CanvasHeight = CanvasHeight;
		setting.CanvasWidth = CanvasWidth;
	}

	void LoadMapChip()
	{
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
	}

	// Update is called once per frame
	void Update () {
		// マウス入力で左クリックをした瞬間
		if (Input.GetMouseButtonDown (0)) {

		}

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
}
