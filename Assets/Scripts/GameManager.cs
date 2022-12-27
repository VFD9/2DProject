using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : ManagerSingleton<GameManager>
{
	// public 변수를 만들고 해당 스크립트에서 사용하지 않으면 다른 스크립트에서도 사용될 여지가 있고, 문제가 될 경우 찾아봐야 한다.
	public bool isScroll = true; 

	// 돈
	public float Money = 1000;
    public Text MoneyTxt;

	void Start()
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

			MoneyTxt.text = string.Format("{0:n0}", (int)current); // {0:n0} => 소수점을 떼고 표시해줌

			yield return null;
		}

		current = target;

		MoneyTxt.text = string.Format("{0:n0}", (int)current);
	}
}
