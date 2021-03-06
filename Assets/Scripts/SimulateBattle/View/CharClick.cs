﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CharClick : MonoBehaviour,IPointerDownHandler {
	[SerializeField]
	private MapSetting setting;

	public void OnPointerDown  (PointerEventData eventData)
	{
		Vector2 LocalPos = setting.ConvertWorldToLocal(Input.mousePosition.x,Input.mousePosition.y);
		TileMapPoint clickTilePos = setting.ConvertLocalPositionToTile(LocalPos.x,LocalPos.y);
		Debug.Log (clickTilePos.x+":"+clickTilePos.y);

		//一升横に移動
		this.GetComponent<SLgPartCharcterView> ().Move (clickTilePos.x+1, clickTilePos.y);

	}



}
