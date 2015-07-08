using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SLgPartCharcterView : MonoBehaviour {
	//キャラクターデータ
	private GameObject CharcterImage;
	private SLGPartCharcterModel _model;
	[SerializeField]
	private MapSetting setting;
	List<GameObject> moveAblePosObjectList;

	//移動可能範囲を示すチップ
	private GameObject dispMoveChip;
	
	// Use this for initialization
	void Start () {
		dispMoveChip = (GameObject)Resources.Load ("SimulateBattle/TileUmi");
		moveAblePosObjectList = new List<GameObject>();
	}

	public void InitModel(SLGPartCharcterModel Init_model)
	{
		_model = Init_model;
	}


	public void Move(float Tilex,float Tiley)
	{
		Vector2 UnitPos = setting.ConvertTileToLocal(Tilex , Tiley );
		//クリックした位置にキャラクターを表示させる
		this.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (UnitPos.x,-UnitPos.y, 0);

		TileMapPoint clickTilePos = new TileMapPoint ((int)Tilex, (int)Tiley);
		drawMovementRange (clickTilePos);
		dispOrderController ();
	}

	//移動可能オブジェクトな場所に画像を配置する
	private void drawMovementRange(TileMapPoint clickTilePos) {
		for (int i=moveAblePosObjectList.Count-1; i >= 0 ; --i) {
			Destroy(moveAblePosObjectList[i]);
		}
		moveAblePosObjectList.Clear ();
		
		for(int rangeX = -1 * _model.playerMoveMentRange; rangeX <= _model.playerMoveMentRange; rangeX++) {
			int targetX = clickTilePos.x + rangeX;
			for(int rangeY = -1 * _model.playerMoveMentRange; rangeY <= _model.playerMoveMentRange; rangeY++) {
				int targetY = clickTilePos.y + rangeY;
				if(!this.outOfTileBorders(targetX, targetY)) {
					if(setting.GetManhattanDistance(clickTilePos.x,clickTilePos.y, targetX, targetY) <= _model.playerMoveMentRange) {
						// 移動可能領域に新規オブジェクトを配置可視化する
						GameObject tempTile = Instantiate (dispMoveChip) as GameObject;
						tempTile.transform.SetParent (this.transform.parent, false);
						tempTile.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (targetX * setting.ChipSizeX, -1 * targetY * setting.ChipSizeY, 0);
						moveAblePosObjectList.Add (tempTile);
					}
				}
			}
		}
	}

	//タイルが表示範囲内かをチェックする
	private bool outOfTileBorders(int xTilePos, int yTilePos) {
		if(xTilePos < 0) return true;
		if(xTilePos >= setting.MapTilePosXMax) return true;
		if(yTilePos < 0) return true;
		if(yTilePos >= setting.MapTilePosYMax) return true;
		return false;
	}

	//表示順番の調整　MoveDisp->MapChip->Unitの順番に並べる
	private void dispOrderController() {
		
		int idx = 0;
		Image[] items = this.transform.parent.GetComponentsInChildren<Image>();
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

	// Update is called once per frame
	void Update () {
	
	}
}
