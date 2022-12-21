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
        // TODO 추후 수정
        if (GameManager.Instance.isScroll)
		{
            transform.Translate(Vector2.left * Time.deltaTime * monSpeed);
		}

        // 애니메이터의 애니메이션 이름이 Attack1일때
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            // normalizedTime은 float형으로 정수부븐은 애니메이션의 루프횟수,
            // 소수부분은 현재 애니메이션의 진행정도를 의미한다.
            float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            // Mathf.Floor로 소수부분을 내림한 후 정수부분을 빼준다.
            float currentState = normalizedTime - Mathf.Floor(normalizedTime);

            if (currentState >= 0.9f && // 애니메이션 진행 상황이 0.9f 이상이면서 진행도가 cntLoop 초과일때
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
