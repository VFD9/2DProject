using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    public GameObject canvas;

    public void Initialize(Transform root, float damage, Color ? color = null)
	{
        // 컬러값 초기화
        color = (color == null) ? Color.red : color;

        Text tmp_text = GetComponent<Text>();

        // 기본 값이 red라면 크리티컬이 아님
        if (color == Color.red)
            tmp_text.text = damage.ToString();
        else
            tmp_text.text = "Critical " + damage;
        tmp_text.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        tmp_text.DOFade(0f, 1f);
        // color값이 null이라면 Color.red를 집어넣고, 아니라면 color를 집어넣는다.
        tmp_text.DOColor((color ?? Color.red), 0.5f);
        transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 1);
        transform.DOMove(transform.position + Vector3.up * 3, 1).OnComplete(() =>
        {
            Destroy(canvas);
        });
    }
}
