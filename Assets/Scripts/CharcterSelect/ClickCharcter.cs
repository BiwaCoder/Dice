using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickCharcter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//クリックしたキャラクターを保存してシーン遷移
	public void OnClickImage(){
		//選択したキャラクターの名前を保存する
		PlayerPrefs.SetString ("SelectCharcter", this.transform.GetComponent<Image> ().sprite.name);
		Application.LoadLevel("DiceGame");
	}
}
