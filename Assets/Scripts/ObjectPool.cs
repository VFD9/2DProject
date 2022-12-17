using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private GameObject pPrefabMob = null; // 생성할 프리팹

    public Queue<GameObject> pQueue = new Queue<GameObject>(); // 객체를 저장할 큐

    private Transform StartPos;

    public void Initialize(GameObject pPrefabMob, Transform StartPos)
	{
        this.pPrefabMob = pPrefabMob;
        this.StartPos = StartPos;

        for (int i = 0; i < 15; ++i)
		{
            GameObject obj = Instantiate(pPrefabMob, this.transform);
            pQueue.Enqueue(obj);
            obj.transform.position = StartPos.position;
            obj.SetActive(false);
		}
	}

    // 사용한 객체를 Pool(Queue)에 반납
    public void InsertQueue (GameObject obj)
	{
        pQueue.Enqueue(obj); // Enqueue() 큐에 넣어준다.
        obj.transform.position = this.StartPos.position;
        obj.SetActive(false);
	}

    // 객체를 가져오는 함수
    public GameObject GetQueue()
	{
        GameObject obj = pQueue.Dequeue(); // Dequeue() 가져오는 것이므로 큐에서 빼준다.
        obj.SetActive(true);
        return obj;
	}
}
