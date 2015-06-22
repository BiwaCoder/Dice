using UnityEngine;
using System.Collections;

public struct CharcterData   {
	//名前
	private string _Name;
	//キャラクターのタイプ
	private string _CharType;
	//画像のパス
	private string _SpritePath;
	//体力
	private int _Hp;
	//所持金
	private int _Money;
	//エネミー仲間になりやすさ 0(0%) ~　100(100%)
	private int _GoodLooking;

	public CharcterData(string Init_Name,string Init_CharType,string Init_SpritePath,int Init_Hp,int Init_Money,int Init_GoodLooking)
	{
		_Name = Init_Name;
		_CharType = Init_CharType;
		_SpritePath = Init_SpritePath;
		_Hp = Init_Hp;
		_Money = Init_Money;
		_GoodLooking = Init_GoodLooking;
	}

	public string Name {
		set{ this._Name = value; }
		get{ return this._Name; }
	}

	
	public string CharType {
		set{ this._CharType = value; }
		get{ return this._CharType; }
	}

	public string SpritePath {
		set{ this._SpritePath = value; }
		get{ return this._SpritePath; }
	}

	public int Hp {
		set{ this._Hp = value; }
		get{ return this._Hp; }
	}

	public int Money {
		set{ this._Money = value; }
		get{ return this._Money; }
	}

	public int GoodLooking {
		set{ this._GoodLooking = value; }
		get{ return this._GoodLooking; }
	}

}
