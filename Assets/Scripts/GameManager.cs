using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : ManagerSingleton<GameManager>
{
    public bool isScroll = true;

    // µ∑
    public float Money = 1000;
    public Text MoneyTxt;

	void Awake()
	{
		MoneyTxt.text = "1,000";
	}

	public void SetMoney(float money)
	{
		Money += money;
		StartCoroutine(Count(Money, Money - money));
	}

	IEnumerator Count(float target, float current)
	{
		float duration = 0.5f;
		float offset = (target - current) / duration;

		while(current < target)
		{
			current += offset * Time.deltaTime;

			MoneyTxt.text = string.Format("{0:n0}", (int)current); // {0:n0} => º“ºˆ¡°¿ª ∂º∞Ì «•Ω√«ÿ¡‹

			yield return null;
		}

		current = target;

		MoneyTxt.text = string.Format("{0:n0}", (int)current);
	}
}
