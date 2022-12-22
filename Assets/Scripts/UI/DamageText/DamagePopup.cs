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
        // �÷��� �ʱ�ȭ
        color = (color == null) ? Color.red : color;

        Text tmp_text = GetComponent<Text>();

        // �⺻ ���� red��� ũ��Ƽ���� �ƴ�
        if (color == Color.red)
            tmp_text.text = damage.ToString();
        else
            tmp_text.text = "Critical " + damage;
        tmp_text.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        tmp_text.DOFade(0f, 1f);
        // color���� null�̶�� Color.red�� ����ְ�, �ƴ϶�� color�� ����ִ´�.
        tmp_text.DOColor((color ?? Color.red), 0.5f);
        transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 1);
        transform.DOMove(transform.position + Vector3.up * 3, 1).OnComplete(() =>
        {
            Destroy(canvas);
        });
    }
}
