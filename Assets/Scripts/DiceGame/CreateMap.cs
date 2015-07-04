using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CreateMap : MonoBehaviour {
	/// <summary>
	/// 出力先キャンバス
	/// </summary>
	public GameObject TargetCanvas;
	//ダイスの値を表示するTextをもつゲームオブジェクト
	public GameObject DiceValueObject;


	//現在位置の格納庫
	private Dictionary<int, MapPos> MapPosTable;
	//現在位置のインデックス
	private int currentUserPos = 0;
	//ゴール位置のインデックス
	private int maxIndex;

	//キャラクターデータ
	private GameObject CharcterImage;


	// Use this for initialization
	void Start () {
		//キャラクター選択データを取得
		string SelectCharcterName = PlayerPrefs.GetString ("SelectCharcter");
		string SelectCharcterPath = "Dicegame/" + SelectCharcterName + "_koma";

		//すごろくのマス目保存領域
		MapPosTable = new Dictionary<int,MapPos>();
		GameObject MapPointResouce = (GameObject)Resources.Load ("Dicegame/Image");
		GameObject CharcterImageResouce = (GameObject)Resources.Load (SelectCharcterPath);

		//すごろくのマス目をsin使っていい感じに配置する
		for (int i = 0,cnt=0; i<360;++cnt,i+=20) {
			GameObject MapPoint = Instantiate (MapPointResouce) as GameObject;
			MapPoint.transform.SetParent (TargetCanvas.transform, false);
			float x,y;
			x = 120*Mathf.Sin(i * Mathf.Deg2Rad);
			y = i - 150;

			MapPoint.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (x,y, 0);
			//マス目と座標位置を対応づけるために保存する
			MapPosTable.Add (cnt,new MapPos(x,y));
			maxIndex = cnt;
		}

		CharcterImage= Instantiate (CharcterImageResouce) as GameObject;
		CharcterImage.transform.SetParent (TargetCanvas.transform, false);
		CharcterImage.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (MapPosTable[0].PosX,MapPosTable[0].PosY, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//ダイスをふる
	public void RollDice(){
		int diceValue = Random.Range (0, 6);
		currentUserPos += diceValue;

		if (MapPosTable.ContainsKey (currentUserPos)) {
			//ダイスの目
			DiceValueObject.GetComponent<Text>().text = currentUserPos.ToString();
			CharcterImage.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (MapPosTable [currentUserPos].PosX, MapPosTable [currentUserPos].PosY, 0);
		} else {
			//ダイスの目
			DiceValueObject.GetComponent<Text>().text = currentUserPos.ToString();
			CharcterImage.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (MapPosTable [maxIndex].PosX, MapPosTable [maxIndex].PosY, 0);
		}
	}
}
