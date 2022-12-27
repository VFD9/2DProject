using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour // 오브젝트를 생성시키는 스크립트
{
	static public CreateManager Instance = null;

	[Range(3.0f, 15.0f)]
 	private float time;
	[SerializeField] private GameObject EnemyObj;

	public List<GameObject> ObjectList = new List<GameObject>();

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
	}

	private void Start()
	{
		time = 5.0f;
	}

	public void CreateObject(GameObject _Obj) // 싱글톤에서 오브젝트를 만들어주는 함수
	{
		GameObject enemy = Instantiate(_Obj);
		ObjectList.Add(enemy); // 별다른 초기값 설정 없이 오브젝트를 복제하고 List에 넣었다.
	}

	private void FixedUpdate()
	{
		time -= Time.deltaTime; // time의 시작값인 5초에서 deltaTime의 시간만큼 계속 빼준다.

		if (time < 0.0f) // time의 값이 0 미만이 될 경우 작동됨.
		{
			time = 5.0f; // time을 다시 5초로 설정해준다.

			CreateObject(new GameObject()); // 오브젝트를 만들어주는 함수를 다시 호출한다.
			//CreateObject(EnemyObj);
		}
	}
}
