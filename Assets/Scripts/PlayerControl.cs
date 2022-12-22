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

    // ���� ����Ʈ
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
            float normalizedTimeInProcess = normalizedTime - Mathf.Floor(normalizedTime); // Mathf.Floor => �Ҽ����� ������ ������ ����

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
        // fillAmount : 0 ~ 1������ ������ �̹��� ũ�⸦ ���� (����)
        this.hp_bar.fillAmount = curHP / maxHP;

        if (curHP <= 0)
        {
            Noti.text = "ü���� 0�� �Ǹ� ���ݼӵ��� ������ ���µ˴ϴ�.";
            dex = 0.5f;
            curHP = 0;
            // TODO : ���� ó��
            //animator.SetTrigger("death");
        }
        else
            Noti.text = "";

        return curHP;
	}

    public void AttackUp()
	{
        if (GameManager.Instance.Money < 1000)
            print("�ݾ��� �����ϴ�.");
        else
        {
            GameManager.Instance.SetMoney(-1000);
            att += 10;
            AttackTxt.text = "���� ���ݷ� : " + att;
        }
    }

    public void HPUp()
	{
        if (GameManager.Instance.Money < 100)
            print("�ݾ��� �����ϴ�.");
        else
        {
            GameManager.Instance.SetMoney(-100);
            curHP += 100;

            if (curHP >= maxHP)
                maxHP = curHP;

            HpTxt.text = "���� ü�� : " + curHP;
        }
    }

    public void DefUp()
	{
        if (GameManager.Instance.Money < 100)
            print("�ݾ��� �����ϴ�.");
        else
        {
            GameManager.Instance.SetMoney(-100);
            def += 1;
            DefTxt.text = "���� ���� : " + def;
        }
    }

    public void DexUp()
	{
        if (GameManager.Instance.Money < 100)
            print("�ݾ��� �����ϴ�.");
        else
        {
            GameManager.Instance.SetMoney(-100);
            animator.speed += 0.1f;
            BackgroundManager.Instance.UpSpeed(0.1f);
            dex += 1;
            DexTxt.text = "���� ��ø�� : " + dex;
        }
    }

    public void CreUp()
	{
        if (GameManager.Instance.Money < 100)
            print("�ݾ��� �����ϴ�.");
        else
        {
            GameManager.Instance.SetMoney(-100);
            cre += 1;
            CreTxt.text = "���� ġ��Ÿ Ȯ�� : " + cre + "%";
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
        if (type == "Atk")
            originValue += 1;
        else if (type == "Dex")
            originValue *= 2.0f;

        return originValue;
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
                att = (int)BuffChange(type, att);
                break;
            case "Dex":
                dex = (int)BuffChange(type, dex);
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
                break;
            case "Dex":
                break;
		}
	}
}
