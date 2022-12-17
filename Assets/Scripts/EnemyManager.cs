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
        GameObject obj = new GameObject(); // EnemyManager 에 새로운 오브젝트 생성
        obj.transform.SetParent(this.transform); // 만든 오브젝트의 부모를 EnemyManager로 설정
        obj.name = typeof(ObjectPool).Name; // 오브젝트의 이름은 오브젝트풀의 이름으로 설정
        this._pPool = obj.AddComponent<ObjectPool>(); // 만든 오브젝트에 ObjectPool 스크립트를 추가하고 pPool에 넣는다.
        this._pPool.Initialize(this.pPrefabMob, this.StartPos); // pPool을 Initialize() 함수로 초기값 설정

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
                GameObject obj = this.pMobQueue.Peek(); // Peek() 맨 처음에 있는거 돌려줌
                Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position); // 카메라에 있는 Viewport Rect를 기준으로 함

                if (pos.x < 0f)
				{
                    this._pPool.InsertQueue(this.pMobQueue.Dequeue()); // 카메라의 Viewport Rect에서 나가면 오브젝트를 뺀다.
                }
			}

            yield return null;
        }
    }
}
