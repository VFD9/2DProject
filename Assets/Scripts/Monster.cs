using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float HP = 100;
    public float monSpeed;

    private Animator animator;

    private float playerHP = 0;
    private int cntLoop = 0;
    private PlayerControl player;
    private float playerDef;

    public float att = 5;

    private void Start()
	{
        animator = GetComponent<Animator>();
	}

    void Update()
    {
        // TODO ���� ����
        if (GameManager.Instance.isScroll)
		{
            transform.Translate(Vector2.left * Time.deltaTime * monSpeed);
		}

        // �ִϸ������� �ִϸ��̼� �̸��� Attack1�϶�
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            // normalizedTime�� float������ �����κ��� �ִϸ��̼��� ����Ƚ��,
            // �Ҽ��κ��� ���� �ִϸ��̼��� ���������� �ǹ��Ѵ�.
            float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            // Mathf.Floor�� �Ҽ��κ��� ������ �� �����κ��� ���ش�.
            float currentState = normalizedTime - Mathf.Floor(normalizedTime);

            if (currentState >= 0.9f && // �ִϸ��̼� ���� ��Ȳ�� 0.9f �̻��̸鼭 ���൵�� cntLoop �ʰ��϶�
                normalizedTime > cntLoop)
            {
                if (att >= playerDef)
                {
                    playerHP = player.Damage(att - playerDef);
                    cntLoop += 1;
                }

                if (playerHP <= 0)
                {
                    animator.SetBool("attack", false);
                    cntLoop = 0;
                }
            }
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
            animator.SetBool("attack", true);
            player = collision.gameObject.GetComponent<PlayerControl>();
            playerDef = player.def;
        }
	}
}
