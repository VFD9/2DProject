using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : ManagerSingleton2<BuffManager>
{
	public GameObject buffPrefab;

	public void CreateBuff(string type, float per, float du, Sprite icon)
	{
		GameObject obj = Instantiate(buffPrefab, transform);
		obj.GetComponent<Buff>().Init(type, per, du);
		obj.GetComponent<Image>().sprite = icon;
	}
}
