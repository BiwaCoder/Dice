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

	public void OnClickImage(){
		Debug.Log ("SpriteName:" + this.transform.GetComponent<Image> ().sprite.name);
		PlayerPrefs.SetString ("SelectCharcter", this.transform.GetComponent<Image> ().sprite.name);
		Application.LoadLevel("DiceGame");
	}
}
