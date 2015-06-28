using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CreateCharcterSelect : MonoBehaviour {
	/// <summary>
	/// 出力先キャンバス
	/// </summary>
	public GameObject TargetCanvas;
	//表示するキャラクターデータ
	List<CharcterData> CharcterDataList ;

	// Use this for initialization
	void Start () {
		//キャラクターデータの登録
		CharcterDataList = new List<CharcterData>();
		CharcterDataList.Add(new CharcterData("セイバー","体力バカ","Image/c1",10000,100,10));
		CharcterDataList.Add(new CharcterData("遠坂","お金持ち","Image/c2",2000,200000,30));
		CharcterDataList.Add(new CharcterData("アーチャー","仲間にしやすい","Image/c3",300,3000,80));

		for (int i=0; i < CharcterDataList.Count; i++) {
			//表示のためのプレハブ取得
			GameObject CharcterBoardResouce = (GameObject)Resources.Load ("CharcterSelect/CharterBoard");
			GameObject CharcterBoard= Instantiate (CharcterBoardResouce) as GameObject;
			CharcterBoard.transform.SetParent (TargetCanvas.transform, false);
			//位置設定
			CharcterBoard.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (146+230*i, -205, 0);
			//スプライトへ画像埋め込み
			GameObject CharcterImageObjetct = CharcterBoard.transform.FindChild ("CharcterImg").gameObject;
			Sprite CharcterSpriteResouce = (Sprite)Resources.Load (CharcterDataList[i].SpritePath,typeof(Sprite));
			CharcterImageObjetct.GetComponent<Image>().sprite = CharcterSpriteResouce;
			//名前表示
			GameObject TextNameObjetct = CharcterBoard.transform.FindChild ("TextName").gameObject;
			TextNameObjetct.GetComponent<Text> ().text = CharcterDataList[i].Name;
			//タイプ表示
			GameObject TextTypeObject = CharcterBoard.transform.FindChild ("TextType").gameObject;
			TextTypeObject.GetComponent<Text> ().text = CharcterDataList[i].CharType;
			//パラメータ表示
			GameObject TextParamObject = CharcterBoard.transform.FindChild ("TextParam").gameObject;
			TextParamObject.GetComponent<Text> ().text =  "HP:" + CharcterDataList[i].Hp.ToString() + "\n" 
														+"所持金:" + CharcterDataList[i].Money.ToString() + "円\n" 
														+"仲間になる確率:" + CharcterDataList[i].GoodLooking.ToString() + "%";
		}


	

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
