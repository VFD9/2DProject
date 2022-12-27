using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour // ������Ʈ�� ������Ű�� ��ũ��Ʈ
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

	public void CreateObject(GameObject _Obj) // �̱��濡�� ������Ʈ�� ������ִ� �Լ�
	{
		GameObject enemy = Instantiate(_Obj);
		ObjectList.Add(enemy); // ���ٸ� �ʱⰪ ���� ���� ������Ʈ�� �����ϰ� List�� �־���.
	}

	private void FixedUpdate()
	{
		time -= Time.deltaTime; // time�� ���۰��� 5�ʿ��� deltaTime�� �ð���ŭ ��� ���ش�.

		if (time < 0.0f) // time�� ���� 0 �̸��� �� ��� �۵���.
		{
			time = 5.0f; // time�� �ٽ� 5�ʷ� �������ش�.

			CreateObject(new GameObject()); // ������Ʈ�� ������ִ� �Լ��� �ٽ� ȣ���Ѵ�.
			//CreateObject(EnemyObj);
		}
	}
}
