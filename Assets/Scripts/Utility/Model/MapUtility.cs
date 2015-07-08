using UnityEngine;
using System.Collections;

public class MapUtility {

	//オフセット
	private float _MapScreenOffsetX;
	private float _MapScreenOffsetY;
	//一つひとつのチップサイズ
	private int _ChipSizeX;
	private int _ChipSizeY;
	//タイルの最大数
	private int _MapTilePosXMax;
	private int _MapTilePosYMax;

	private float _CanvasHeight;
	private float _CanvasWidth;

	public MapUtility(float Init_MapScreenOffsetX, float Init_MapScreenOffsetY,
	                  int Init_ChipSizeX,int Init_ChipSizeY,int Init_MapTilePosXMax,int Init_MapTilePosYMax,
	                  float Init_CanvasHeight,float Init_CanvasWidth) 
	{
		_MapScreenOffsetX = Init_MapScreenOffsetX;
		_MapScreenOffsetY = Init_MapScreenOffsetY;
		_ChipSizeX = Init_ChipSizeX;
		_ChipSizeY = Init_ChipSizeY;
		_MapTilePosXMax = Init_MapTilePosXMax;
		_MapTilePosYMax = Init_MapTilePosYMax;
		_CanvasHeight = Init_CanvasHeight;
		_CanvasWidth = Init_CanvasWidth;

	}


	//Unity全体の座標系から、表示領域内の座標を取得する
	public Vector2 ConvertWorldToLocal(float worldx,float worldy){
		return new Vector2 (worldx - _MapScreenOffsetX, _CanvasHeight - worldy + _MapScreenOffsetY);
	}
	
	//表示範囲内の座標から、Unity全体の座標系に直す
	public Vector2 ConvertLocalToWorld(float localx,float localy){
		return new Vector2 (localx + _MapScreenOffsetX, localy + _MapScreenOffsetY);
	}
	
	//ローカル座標よりマス目の位置を取得する
	public TileMapPoint ConvertLocalPositionToTile(float localx,float localy){
		return new TileMapPoint(Mathf.FloorToInt(localx/_ChipSizeX), Mathf.FloorToInt(localy/_ChipSizeY));
	}
	
	//タイル座標からローカル座標系への変換
	public Vector2 ConvertTileToLocal(int i,int j){
		return new Vector2 (i*_ChipSizeX,j*_ChipSizeY);
	}
	
	//２点のタイル座標系からマンハッタン距離を求める
	public float GetManhattanDistance (float startI, float startJ,float endI, float endJ) {
		float distance = Mathf.Abs(startI -endI) +Mathf.Abs(startJ -endJ);
		return distance;
	}
}
