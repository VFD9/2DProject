using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    // 버프 타입
    public string type;
    // 능력치 올릴 퍼센테이지
    public float percentage;
    // 버프 진행 시간
    public float duration;
    // 버프 현재 진행 시간
    public float currentTime;
    // 버프 아이콘
    public Image icon;

    PlayerControl player;

	public void Awake()
	{
        icon = GetComponent<Image>();
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
	}

    public void Init(string type, float per, float du)
	{
        this.type = type;
        percentage = per;
        duration = du;
        currentTime = duration;
        icon.fillAmount = 1;

        Excute();
	}

    public void Excute()
	{
        // TODO : 플레이어의 버프 효과 줘야함.
        player.onBuff.Add(this);
        player.ChooseBuff(type);

        // 액티베이션 실행
        StartCoroutine(Activation());
	}

    IEnumerator Activation()
	{
        // TODO : 아이콘 filAmount 시간대별로 줄어들도록 작업
        while (currentTime > 0)
        {
            currentTime -= 1.0f;
            icon.fillAmount = currentTime / duration;
            yield return new WaitForSeconds(1.0f);
        }

        icon.fillAmount = 0;
        currentTime = 0;

        DestroyActivation();
	}

    public void DestroyActivation()
	{
        // TODO : 버프 끝나고 처리할 함수
        player.onBuff.Remove(this);
        player.minusBuff(type);
        Destroy(gameObject);
	}
}
