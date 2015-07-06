using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapChip{
	public MapChip(int id, string prefabName) {
		this.id = id;
		this.prefabName = prefabName;
	}
	public int id
	{
		get;
		private set;
	}
	
	public string prefabName
	{
		get;
		private set;
	}

	public GameObject prefab{
		get;
		set;
	}


}
