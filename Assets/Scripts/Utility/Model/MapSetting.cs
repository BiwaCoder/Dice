using UnityEngine;
using System.Collections;

public class MapSetting : ScriptableObject {
	//オフセット
	public float MapScreenOffsetX;
	public float MapScreenOffsetY;
	//一つひとつのチップサイズ
	public int ChipSizeX;
	public int ChipSizeY;
	//タイルの最大数
	public int MapTilePosXMax;
	public int MapTilePosYMax;
	
	public float CanvasHeight;
	public float CanvasWidth;

	//Unity全体の座標系から、表示領域内の座標を取得する
	public Vector2 ConvertWorldToLocal(float worldx,float worldy){
		return new Vector2 (worldx - MapScreenOffsetX, CanvasHeight - worldy + MapScreenOffsetY);
	}
	
	//表示範囲内の座標から、Unity全体の座標系に直す
	public Vector2 ConvertLocalToWorld(float localx,float localy){
		return new Vector2 (localx + MapScreenOffsetX, localy + MapScreenOffsetY);
	}
	
	//ローカル座標よりマス目の位置を取得する
	public TileMapPoint ConvertLocalPositionToTile(float localx,float localy){
		return new TileMapPoint(Mathf.FloorToInt(localx/ChipSizeX), Mathf.FloorToInt(localy/ChipSizeY));
	}
	
	//タイル座標からローカル座標系への変換
	public Vector2 ConvertTileToLocal(float i,float j){
		return new Vector2 (i*ChipSizeX,j*ChipSizeY);
	}
	
	//２点のタイル座標系からマンハッタン距離を求める
	public float GetManhattanDistance (float startI, float startJ,float endI, float endJ) {
		float distance = Mathf.Abs(startI -endI) +Mathf.Abs(startJ -endJ);
		return distance;
	}
}
