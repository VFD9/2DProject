using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : ManagerSingleton2<EnemyManager>
{
    public GameObject pPrefabMob = null;
    public GameObject Player;
    public Transform StartPos;

    private ObjectPool _pPool;

    private Queue<GameObject> pMobQueue = new Queue<GameObject>();

    // ��� ������
    public ItemFX prefabItem;
    public Transform toTweenPos;
    public Transform goldParent;
    public Transform fromPos;
    public Transform HP;

    void Start()
    {
        GameObject obj = new GameObject(); // EnemyManager �� ���ο� ������Ʈ ����
        obj.transform.SetParent(this.transform); // ���� ������Ʈ�� �θ� EnemyManager�� ����
        obj.name = typeof(ObjectPool).Name; // ������Ʈ�� �̸��� ������ƮǮ�� �̸����� ����
        this._pPool = obj.AddComponent<ObjectPool>(); // ���� ������Ʈ�� ObjectPool ��ũ��Ʈ�� �߰��ϰ� pPool�� �ִ´�.
        this._pPool.Initialize(this.pPrefabMob, this.StartPos); // pPool�� Initialize() �Լ��� �ʱⰪ ����

        this.StartCoroutine(CreateEnemy());
    }

    IEnumerator CreateEnemy()
	{
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));

        while(true)
		{
            if (GameManager.Instance.isScroll)
            {
                GameObject mob = this._pPool.GetQueue();
                Monster cMob = mob.GetComponent<Monster>();
                cMob.HP = 100;

                this.pMobQueue.Enqueue(mob);
                yield return new WaitForSeconds(Random.Range(1f, 1.5f));
            }
            else
                yield break;
		}
	}

    public int Damaged(int att)
	{
        if (this.pMobQueue.Count != 0)
		{
            GameObject obj = this.pMobQueue.Peek();
            Monster mob = obj.GetComponent<Monster>();
            mob.HP -= att;

            if (mob.HP <= 0)
			{
                // �� �ö󰡴� �ִϸ��̼�
                this.SetMoney();

                GameManager.Instance.isScroll = true;
                this.StartCoroutine(CreateEnemy());
                this._pPool.InsertQueue(this.pMobQueue.Dequeue());
                Player.GetComponent<PlayerControl>().HP += 10;
                HP.GetComponent<Image>().fillAmount += 0.1f;
			}
            else
			{
                DamageOn damageTxt = obj.GetComponent<DamageOn>();
                damageTxt.DamagedTxt();
			}

            return mob.HP;
		}

        return 0;
	}
    
    public int Attack(int atk)
    {
        if (Player.GetComponent<PlayerControl>().HP != 0)
        {
            Player.GetComponent<PlayerControl>().HP -= atk;
            HP.GetComponent<Image>().fillAmount -= atk * 0.01f;

            return atk;
        }

        return 0;
    }

    void SetMoney()
	{
        int randCount = Random.Range(5, 10);
        for(int i = 0; i < randCount; ++i)
		{
            Vector3 screenPos = Camera.main.WorldToScreenPoint(fromPos.position);
            ItemFX itemFx = Instantiate(prefabItem, screenPos, Quaternion.identity);
            itemFx.transform.SetParent(goldParent);
            itemFx.Explosion(screenPos, toTweenPos.position, 150f);
		}

        GameManager.Instance.SetMoney(Random.Range(50, 100));
	}

    //IEnumerator DeleteEnemy()
	//{
    //    yield return null;
    //
    //    while (true)
    //    {
    //        if (this.pMobQueue.Count != 0)
	//		{
    //            GameObject obj = this.pMobQueue.Peek(); // Peek() �� ó���� �ִ°� ������
    //            Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position); // ī�޶� �ִ� Viewport Rect�� �������� ��
    //
    //            if (pos.x < 0f)
	//			  {
    //                this._pPool.InsertQueue(this.pMobQueue.Dequeue()); // ī�޶��� Viewport Rect���� ������ ������Ʈ�� ����.
    //            }
	//		}
    //
    //        yield return null;
    //    }
    //}
}
