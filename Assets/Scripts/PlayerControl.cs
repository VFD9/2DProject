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

    [Range(1, 100)]
    public float cre = 1;
    private float mincre = 1;
    private float maxcre = 100;

    public Image hp_bar;

    public Text Noti;

    public Text AttackTxt;
    public Text HpTxt;
    public Text DefTxt;
    public Text DexTxt;
    public Text CreTxt;

    private float attackspeed = 1;
    float attcount = 0;
    float[] numlist = new float[100];

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
                cntLoop += 1;
                attcount += 1;

                mobHP = EnemyManager.Instance.Damaged(att);

                if (attcount > 100)
                    attcount = 1;

                if (mobHP <= 0)
				{
                    animator.SetBool("attack", false);
                    cntLoop = 0;
				}
			}
		}

        HpTxt.text = "현재 체력 : " + curHP;
    }

    public float Damage(float att)
	{
        this.curHP -= att;
        // fillAmount : 0 ~ 1까지의 값으로 이미지 크기를 조정 (비율)
        this.hp_bar.fillAmount = curHP / maxHP;

        if (curHP <= 0)
		{
            // TODO : 추후 처리
            animator.SetTrigger("death");
		}

        return curHP;
	}

    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Monster")
		{
            GameManager.Instance.isScroll = false;
            animator.SetBool("attack", true);
        }
	}

    public void AttackUp()
	{
        if (GameManager.Instance.Money >= 1000)
        {
            GameManager.Instance.Money -= 1000;
            GameManager.Instance.MoneyTxt.text = GameManager.Instance.Money.ToString();
            att += 10;
            AttackTxt.text = "현재 공격력 : " + att;
        }
    }

    public void HPUp()
	{
        if (GameManager.Instance.Money >= 100)
        {
            GameManager.Instance.Money -= 100;
            GameManager.Instance.MoneyTxt.text = GameManager.Instance.Money.ToString();
            curHP += 100;
            maxHP += 100;
        }
    }

    public void DefUp()
	{
        if (GameManager.Instance.Money >= 100)
        {
            GameManager.Instance.Money -= 100;
            GameManager.Instance.MoneyTxt.text = GameManager.Instance.Money.ToString();
            def += 1;
            DefTxt.text = "현재 방어력 : " + def;
        }
    }

    public void DexUp()
	{
        if (GameManager.Instance.Money >= 100)
        {
            GameManager.Instance.Money -= 100;
            GameManager.Instance.MoneyTxt.text = GameManager.Instance.Money.ToString();
            animator.SetFloat("attackSpeed", attackspeed += 0.1f);
            dex += 1;
            DexTxt.text = "현재 민첩성 : " + dex;
        }
    }

    public void CreUp()
	{
        if (GameManager.Instance.Money >= 100)
        {
            GameManager.Instance.Money -= 100;
            GameManager.Instance.MoneyTxt.text = GameManager.Instance.Money.ToString();
            cre += 1;
            CreTxt.text = "현재 치명타 확률 : " + cre + "%";
        }
    }

    public void creatt(float _att)
    {
        for (int i = 0; i < 100; ++i)
        {
            float rand = Random.Range(1, 100);
            numlist[i] = rand;

            for (int j = 0; j < i; ++j)
                if (numlist[j] == numlist[i]) --i;

            if (numlist[i] <= cre)
                _att *= 2;
        }
    }
}
