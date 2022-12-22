using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public float att = 20;
    Animator animator;

    public float curHP = 100;
    public float maxHP = 100;
    public float def = 0;
    public float dex = 0;
    public float cre = 0;

    public Image hp_bar;

    public Text Noti;

    public Text AttackTxt;
    public Text HpTxt;
    public Text DefTxt;
    public Text DexTxt;
    public Text CreTxt;

    // 버프 리스트
    public List<Buff> onBuff = new List<Buff>();

    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("attack", false);
    }

    float mobHP = 0;
    int cntLoop = 0;

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
		{
            float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float normalizedTimeInProcess = normalizedTime - Mathf.Floor(normalizedTime); // Mathf.Floor => 소수점을 버리고 정수만 남김

            if (normalizedTimeInProcess >= 0.9f &&
                normalizedTime > cntLoop)
			{
                int creRan = Random.Range(1, 1000);

                if (creRan < cre)
                    mobHP = EnemyManager.Instance.Damaged(att * creRan, Color.yellow);
                else
                    mobHP = EnemyManager.Instance.Damaged(att);

                cntLoop += 1;

                if (mobHP <= 0)
				{
                    animator.SetBool("attack", false);
                    cntLoop = 0;
				}
			}
		}
    }

    public float Damage(float att)
	{
        this.curHP -= (att - def);
        // fillAmount : 0 ~ 1까지의 값으로 이미지 크기를 조정 (비율)
        this.hp_bar.fillAmount = curHP / maxHP;

        if (curHP <= 0)
        {
            Noti.text = "체력이 0이 되면 공격속도가 최저로 리셋됩니다.";
            dex = 0.5f;
            curHP = 0;
            // TODO : 추후 처리
            //animator.SetTrigger("death");
        }
        else
            Noti.text = "";

        return curHP;
	}

    public void AttackUp()
	{
        if (GameManager.Instance.Money < 1000)
            print("금액이 적습니다.");
        else
        {
            GameManager.Instance.SetMoney(-1000);
            att += 10;
            AttackTxt.text = "현재 공격력 : " + att;
        }
    }

    public void HPUp()
	{
        if (GameManager.Instance.Money < 100)
            print("금액이 적습니다.");
        else
        {
            GameManager.Instance.SetMoney(-100);
            curHP += 100;

            if (curHP >= maxHP)
                maxHP = curHP;

            HpTxt.text = "현재 체력 : " + curHP;
        }
    }

    public void DefUp()
	{
        if (GameManager.Instance.Money < 100)
            print("금액이 적습니다.");
        else
        {
            GameManager.Instance.SetMoney(-100);
            def += 1;
            DefTxt.text = "현재 방어력 : " + def;
        }
    }

    public void DexUp()
	{
        if (GameManager.Instance.Money < 100)
            print("금액이 적습니다.");
        else
        {
            GameManager.Instance.SetMoney(-100);
            animator.speed += 0.1f;
            BackgroundManager.Instance.UpSpeed(0.1f);
            dex += 1;
            DexTxt.text = "현재 민첩성 : " + dex;
        }
    }

    public void CreUp()
	{
        if (GameManager.Instance.Money < 100)
            print("금액이 적습니다.");
        else
        {
            GameManager.Instance.SetMoney(-100);
            cre += 1;
            CreTxt.text = "현재 치명타 확률 : " + cre + "%";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            GameManager.Instance.isScroll = false;
            animator.SetBool("attack", true);
        }
    }

    //////////////////////////////////////////////////////////////////
    /// 버프 시스템 코드

    /// <summary>
    /// 버프를 체인지(추가) 하는 코드를 작성
    /// </summary>
    /// <param name="type">버프 타입</param>
    /// <param name="originValue"> 체인지(추가)할 스테이스 값</param>
    public float BuffChange(string type, float originValue)
	{
        if (type == "Atk")
            originValue += 1;
        else if (type == "Dex")
            originValue *= 2.0f;

        return originValue;
	}

    /// <summary>
    /// 버프 선택에 따른 값 변경
    /// </summary>
    /// <param name="type">버프 타입</param>
    public void ChooseBuff(string type)
	{
        switch(type)
		{
            case "Atk":
                att = (int)BuffChange(type, att);
                break;
            case "Dex":
                dex = (int)BuffChange(type, dex);
                break;
		}
	}

    /// <summary>
    /// 버프 시간이 종료되었을 때 효과를 빼주는 기능
    /// </summary>
    /// <param name="type">버프 타입</param>
    public void minusBuff(string type)
	{
        switch(type)
		{
            case "Atk":
                break;
            case "Dex":
                break;
		}
	}
}
