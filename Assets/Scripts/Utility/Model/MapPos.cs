using UnityEngine;
using System.Collections;

public struct MapPos  {
	private float _PosX;
	private float _PosY;

	public MapPos(float Init_posX,float Init_posY)
	{
		_PosX = Init_posX;
		_PosY = Init_posY;
	}

	public float PosX {
		set{ this._PosX = value; }
		get{ return this._PosX; }
	}
	
	public float PosY {
		set{ this._PosY = value; }
		get{ return this._PosY; }
	}

}
