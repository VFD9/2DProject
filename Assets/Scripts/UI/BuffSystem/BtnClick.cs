using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnClick : MonoBehaviour
{
    public string type;
    public float per;
    public float duration;
    public Sprite icon;

    public void Click()
	{
        int buffCount = GameObject.Find("Player").GetComponent<PlayerControl>().onBuff.Count;

        if (buffCount >= 5)
            return;

        BuffManager.Instance.CreateBuff(type, per, duration, icon);
	}
}
