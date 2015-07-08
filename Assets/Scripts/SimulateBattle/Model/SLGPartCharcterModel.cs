using UnityEngine;
using System.Collections;

public class SLGPartCharcterModel {
	//キャラクターのタイル座標
	private float _TilePosX;
	private float _TilePosY;

	//すべての行動が完了
	private bool _isAllActionFinised;

	//攻撃完了　//アクションタイプ、完了、範囲
	private bool _isAttackEnd;
	//移動完了
	private bool _isMoveEnd;

	//プレイヤーの移動範囲
	private int _playerMoveMentRange = 3;
	
	public SLGPartCharcterModel(float Init_TilePosX,float Init_TilePosY,int Init_playerMoveMentRange,
	                       bool Init_isAllActionFinised,bool Init_isAttackEnd,bool Init_isMoveEnd)
	{
		_TilePosX = Init_TilePosX;
		_TilePosY = Init_TilePosY;
		_playerMoveMentRange = Init_playerMoveMentRange;
		_isAllActionFinised = Init_isAllActionFinised;
		_isAttackEnd = Init_isAttackEnd;
		_isMoveEnd = Init_isMoveEnd;
	}

	public float TilePosX {
		set{ this._TilePosX = value; }
		get{ return this._TilePosX; }
	}
	
	public float TilePosY {
		set{ this._TilePosY = value; }
		get{ return this._TilePosY; }
	}

	public int playerMoveMentRange {
		set{ this._playerMoveMentRange = value; }
		get{ return this._playerMoveMentRange; }
	}
	

	//選択可能な行動のリスト (表示用）
	public void SelectableActionList(){
	}

	//行動可能
	public bool isEnableAction()
	{
		//行動一覧のチェック


		return _isAllActionFinised;
	}




}
