using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    // ���� Ÿ��
    public string type;
    // �ɷ�ġ �ø� �ۼ�������
    public float percentage;
    // ���� ���� �ð�
    public float duration;
    // ���� ���� ���� �ð�
    public float currentTime;
    // ���� ������
    public Image icon;

    PlayerControl player;

	public void Awake()
	{
        icon = GetComponent<Image>();
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
	}

    public void Init(string type, float per, float  du)
	{
        this.type = type;
        percentage = per;
        duration = du;
        currentTime = duration;
        icon.fillAmount = 1;

        Excute();
	}

    public void Excute()
	{
        // TODO : �÷��̾��� ���� ȿ�� �����.
        player.ChooseBuff(type);
        player.onBuff.Add(this.gameObject.GetComponent<Buff>());

        // ��Ƽ���̼� ����
        StartCoroutine(Activation());
	}

    IEnumerator Activation()
	{
        // TODO : ������ filAmount �ð��뺰�� �پ�鵵�� �۾�
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            currentTime--;
            icon.fillAmount = currentTime / duration;

            if (currentTime <= 0)
            {
                currentTime = 0;
                DestroyActivation();
                break;
            }
        }
	}

    public void DestroyActivation()
	{
        // TODO : ���� ������ ó���� �Լ�
        Destroy(gameObject);
	}
}
