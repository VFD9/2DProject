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

    // ���� ����Ʈ
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
            float normalizedTimeInProcess = normalizedTime - Mathf.Floor(normalizedTime); // Mathf.Floor => �Ҽ����� ������ ������ ����

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

        AttackTxt.text = "���� ���ݷ� : " + curStatus.att;
        HpTxt.text = "���� ü�� : " + curStatus.curHp + "/" + curStatus.MaxHp;
        DefTxt.text = "���� ���� : " + curStatus.def;
        DexTxt.text = "���� ��ø�� : " + curStatus.dex;
        CreTxt.text = "���� ġ��Ÿ Ȯ�� : " + curStatus.cri + "%";
        GameManager.Instance.MoneyTxt.text = GameManager.Instance.Money.ToString();
    }

    public float Damage(float att)
	{
        if (att >= curStatus.def)
            curStatus.curHp -= (att - curStatus.def);

        // fillAmount : 0 ~ 1������ ������ �̹��� ũ�⸦ ���� (����)
        this.hp_bar.fillAmount = curStatus.curHp / curStatus.MaxHp;

        if (curStatus.curHp <= 0)
        {
            Noti.text = "ü���� 0�� �Ǹ� ���ݼӵ��� ������ ���µ˴ϴ�.";
            curStatus.dex = 0.5f;
            curStatus.curHp = 0;
            // TODO : ���� ó��
            //animator.SetTrigger("death");
        }
        else
            Noti.text = "";

        return curStatus.curHp;
	}

    public void AttackUp()
	{
        if (GameManager.Instance.Money < 1000)
            print("�ݾ��� �����ϴ�.");
        else
        {
            GameManager.Instance.SetMoney(-1000);
            curStatus.att += 10;
        }
    }

    public void HPUp()
	{
        if (GameManager.Instance.Money < 100)
            print("�ݾ��� �����ϴ�.");
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
            print("�ݾ��� �����ϴ�.");
        else
        {
            GameManager.Instance.SetMoney(-100);
            curStatus.def += 1;
        }
    }

    public void DexUp()
	{
        if (GameManager.Instance.Money < 100)
            print("�ݾ��� �����ϴ�.");
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
            print("�ݾ��� �����ϴ�.");
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
    /// ���� �ý��� �ڵ�

    /// <summary>
    /// ������ ü����(�߰�) �ϴ� �ڵ带 �ۼ�
    /// </summary>
    /// <param name="type">���� Ÿ��</param>
    /// <param name="originValue"> ü����(�߰�)�� �����̽� ��</param>
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
    /// ���� ���ÿ� ���� �� ����
    /// </summary>
    /// <param name="type">���� Ÿ��</param>
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
    /// ���� �ð��� ����Ǿ��� �� ȿ���� ���ִ� ���
    /// </summary>
    /// <param name="type">���� Ÿ��</param>
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
