using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int HP = 100;
    public int atk = 5;
    public float monSpeed;
    public Transform StartPosition;

    private bool _isScroll = true;
    Animator animator;

	private void Start()
	{
        animator = GetComponent<Animator>();
	}

    int playerHP = 0;
    int cntLoop = 0;

    void Update()
    {
        // TODO 추후 수정
        if (GameManager.Instance.isScroll)
		{
            transform.Translate(Vector2.left * Time.deltaTime * monSpeed);
		}

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            float normalizeTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime; // normalizeTime => 애니메이션 진행도
            float normalizeTimeInProcess = normalizeTime - Mathf.Floor(normalizeTime); // Mathf.Floor => 소수점을 버리고 정수만 남김

            if (normalizeTimeInProcess >= 0.9f && // 애니메이션 진행 상황이 0.9f 이상이면서 진행도가 cntLoop 초과일때
                normalizeTime > cntLoop)
            {
                cntLoop += 1;
                playerHP = EnemyManager.Instance.Attack(atk);

                if (playerHP <= 0)
                {
                    animator.SetBool("attack", false);
                    cntLoop = 0;
                }
            }
        }
    }

    public void SetScroll(bool isScroll)
	{
        this._isScroll = isScroll;
        this.monSpeed = (isScroll) ? 5 : 0;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.tag == "Player")
        {
            GameManager.Instance.isScroll = false;
            animator.SetBool("attack", true);
        }
    }
}
