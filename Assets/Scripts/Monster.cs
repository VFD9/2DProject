using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int HP = 100;
    public float monSpeed;
    public Transform StartPosition;

    private bool _isScroll = true;

    void Update()
    {
        // TODO 추후 수정
        if(GameManager.Instance.isScroll)
		{
            transform.Translate(Vector2.left * Time.deltaTime * monSpeed);
		}
    }

    public void SetScroll(bool isScroll)
	{
        this._isScroll = isScroll;
        this.monSpeed = (isScroll) ? 5 : 0;
	}
}
