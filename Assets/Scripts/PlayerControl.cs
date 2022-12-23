using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct PlayerStatus
{
    public float curHp;
    public float MaxHp;
    public float dex;
    public float def;
    public float cri;
    public float att;

    public PlayerStatus(float HP, float MaxHP, float dex, float def, float cri, float att)
	{
        this.curHp = HP;
        this.MaxHp = MaxHP;
        this.dex = dex;
        this.def = def;
        this.cri = cri;
        this.att = att;
	}
}

public class PlayerControl : MonoBehaviour
{
    Animator animator;

    public PlayerStatus curStatus;
    public PlayerStatus originStatus;

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

        this.originStatus = new PlayerStatus(100, 100, 1, 1, 1, 20);
        this.curStatus = this.originStatus;
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

                if (creRan < curStatus.cri)
                    mobHP = EnemyManager.Instance.Damaged(curStatus.att * creRan, Color.yellow);
                else
                    mobHP = EnemyManager.Instance.Damaged(curStatus.att);

                cntLoop += 1;

                if (mobHP <= 0)
				{
                    animator.SetBool("attack", false);
                    cntLoop = 0;
				}
			}
		}

        AttackTxt.text = "현재 공격력 : " + curStatus.att;
        HpTxt.text = "현재 체력 : " + curStatus.curHp + "/" + curStatus.MaxHp;
        DefTxt.text = "현재 방어력 : " + curStatus.def;
        DexTxt.text = "현재 민첩성 : " + curStatus.dex;
        CreTxt.text = "현재 치명타 확률 : " + curStatus.cri + "%";
        GameManager.Instance.MoneyTxt.text = GameManager.Instance.Money.ToString();
    }

    public float Damage(float att)
	{
        if (att >= curStatus.def)
            curStatus.curHp -= (att - curStatus.def);

        // fillAmount : 0 ~ 1까지의 값으로 이미지 크기를 조정 (비율)
        this.hp_bar.fillAmount = curStatus.curHp / curStatus.MaxHp;

        if (curStatus.curHp <= 0)
        {
            Noti.text = "체력이 0이 되면 공격속도가 최저로 리셋됩니다.";
            curStatus.dex = 0.5f;
            curStatus.curHp = 0;
            // TODO : 추후 처리
            //animator.SetTrigger("death");
        }
        else
            Noti.text = "";

        return curStatus.curHp;
	}

    public void AttackUp()
	{
        if (GameManager.Instance.Money < 1000)
            print("금액이 적습니다.");
        else
        {
            GameManager.Instance.SetMoney(-1000);
            curStatus.att += 10;
        }
    }

    public void HPUp()
	{
        if (GameManager.Instance.Money < 100)
            print("금액이 적습니다.");
        else
        {
            GameManager.Instance.SetMoney(-100);
            curStatus.curHp += 100;
            curStatus.MaxHp += 100;
        }
    }

    public void DefUp()
	{
        if (GameManager.Instance.Money < 100)
            print("금액이 적습니다.");
        else
        {
            GameManager.Instance.SetMoney(-100);
            curStatus.def += 1;
        }
    }

    public void DexUp()
	{
        if (GameManager.Instance.Money < 100)
            print("금액이 적습니다.");
        else
        {
            GameManager.Instance.SetMoney(-100);
            curStatus.dex += 1;
            animator.speed += curStatus.dex * 0.1f;
        }
    }

    public void CreUp()
	{
        if (GameManager.Instance.Money < 100)
            print("금액이 적습니다.");
        else
        {
            GameManager.Instance.SetMoney(-100);
            curStatus.cri += 1;
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
        if (onBuff.Count > 0)
		{
            float temp = 0;
            for (int i = 0; i < onBuff.Count; ++i)
			{
                if (onBuff[i].type.Equals(type))
                    temp += originValue * onBuff[i].percentage;
			}
            return originValue + temp;
		}
        else
		{
            return originValue;
		}
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
                curStatus.att = (float)BuffChange(type, curStatus.att);
                break;
            case "Dex":
                curStatus.dex = (float)BuffChange(type, curStatus.dex);
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
                this.curStatus.att = (float)BuffChange(type, originStatus.att);
                break;
            case "Dex":
                this.curStatus.dex = (float)BuffChange(type, originStatus.dex);
                break;
		}
	}
}
