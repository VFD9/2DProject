using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public int att = 20;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("attack", false);
    }

    int mobHP = 0;
    int cntLoop = 0;

    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
		{
            float normalizeTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float normalizeTimeInProcess = normalizeTime - Mathf.Floor(normalizeTime); // Mathf.Floor => �Ҽ����� ������ ������ ����

            if(normalizeTimeInProcess >= 0.9f &&
                normalizeTime > cntLoop)
			{
                cntLoop += 1;
                mobHP = EnemyManager.Instance.Damaged(att);

                if(mobHP <= 0)
				{
                    animator.SetBool("attack", false);
                    cntLoop = 0;
				}
			}
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
}