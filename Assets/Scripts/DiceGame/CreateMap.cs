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
	private int maxIndex;

	private GameObject CharcterImage;


	// Use this for initialization
	void Start () {
		string SelectCharcterName = PlayerPrefs.GetString ("SelectCharcter");
		string SelectCharcterPath = "Dicegame/" + SelectCharcterName + "_koma";


		MapPosTable = new Dictionary<int,MapPos>();
		GameObject MapPointResouce = (GameObject)Resources.Load ("Dicegame/Image");
		GameObject CharcterImageResouce = (GameObject)Resources.Load (SelectCharcterPath);

		for (int i = 0,cnt=0; i<360;++cnt,i+=20) {
			GameObject MapPoint = Instantiate (MapPointResouce) as GameObject;
			MapPoint.transform.SetParent (TargetCanvas.transform, false);
			float x,y;
			x = 120*Mathf.Sin(i * Mathf.Deg2Rad);
			y = i - 150;

			MapPoint.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (x,y, 0);
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

	public void RollDice(){
		int diceValue = Random.Range (0, 6);
		currentUserPos += diceValue;

		if (MapPosTable.ContainsKey (currentUserPos)) {
			DiceValueObject.GetComponent<Text>().text = currentUserPos.ToString();
			CharcterImage.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (MapPosTable [currentUserPos].PosX, MapPosTable [currentUserPos].PosY, 0);
		} else {
			DiceValueObject.GetComponent<Text>().text = currentUserPos.ToString();
			CharcterImage.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (MapPosTable [maxIndex].PosX, MapPosTable [maxIndex].PosY, 0);
		}
	}
}
