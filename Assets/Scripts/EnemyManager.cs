using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject pPrefabMob = null;
    public Transform StartPos;

    private ObjectPool _pPool;

    private Queue<GameObject> pMobQueue = new Queue<GameObject>();

    void Start()
    {
        GameObject obj = new GameObject(); // EnemyManager �� ���ο� ������Ʈ ����
        obj.transform.SetParent(this.transform); // ���� ������Ʈ�� �θ� EnemyManager�� ����
        obj.name = typeof(ObjectPool).Name; // ������Ʈ�� �̸��� ������ƮǮ�� �̸����� ����
        this._pPool = obj.AddComponent<ObjectPool>(); // ���� ������Ʈ�� ObjectPool ��ũ��Ʈ�� �߰��ϰ� pPool�� �ִ´�.
        this._pPool.Initialize(this.pPrefabMob, this.StartPos); // pPool�� Initialize() �Լ��� �ʱⰪ ����

        this.StartCoroutine(CreateEnemy());
        this.StartCoroutine(DeleteEnemy());
    }

    IEnumerator CreateEnemy()
	{
        yield return null;

        while(true)
		{
            GameObject mob = this._pPool.GetQueue();
            this.pMobQueue.Enqueue(mob);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
		}
	}

    IEnumerator DeleteEnemy()
	{
        yield return null;

        while (true)
        {
            if (this.pMobQueue.Count != 0)
			{
                GameObject obj = this.pMobQueue.Peek(); // Peek() �� ó���� �ִ°� ������
                Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position); // ī�޶� �ִ� Viewport Rect�� �������� ��

                if (pos.x < 0f)
				{
                    this._pPool.InsertQueue(this.pMobQueue.Dequeue()); // ī�޶��� Viewport Rect���� ������ ������Ʈ�� ����.
                }
			}

            yield return null;
        }
    }
}
