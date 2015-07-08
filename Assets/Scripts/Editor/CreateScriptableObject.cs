using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateScriptableObjectSample : MonoBehaviour {
	[MenuItem("Assets/Create/ScriptableObjectSample")]
	public static void CreateAsset()
	{
		CreateAsset<MapSetting>();
	}
	
	public static void CreateAsset<Type>() where Type : ScriptableObject{
		Type item = ScriptableObject.CreateInstance<Type>();
		
		string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Data/" + typeof(Type) + ".asset");
		
		AssetDatabase.CreateAsset(item, path);
		AssetDatabase.SaveAssets();
		
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = item;
	}
}
