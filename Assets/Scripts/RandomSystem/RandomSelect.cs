using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RandomSelect : MonoBehaviour
{
	public List<Skill> deck = new List<Skill>();
	public int total = 0;
	Coroutine sc;

	void Start()
	{
		for (int i = 0; i < deck.Count; ++i)
		{
			total += deck[i].weight;
		}
	}

	// �����ϰ� ���õ� ��ų�� ���� ����Ʈ
	public List<Skill> result = new List<Skill>();
	public List<GameObject> skillOb = new List<GameObject>();

	public Transform parent;
	public GameObject skillPrefab;

	public void RandomStart()
	{
		if (sc == null)
		{
			for (int i = 0; i < skillOb.Count; ++i)
			{
				Destroy(skillOb[i]);
			}
			result.Clear();
			skillOb.Clear();

			sc = StartCoroutine("ResultSelect");
		}
	}

	IEnumerator ResultSelect()
	{
		for (int i = 0; i < 20; ++i)
		{
			result.Add(RandomCard());
			// ����ִ� ī�带 ����
			GameObject skillUI = Instantiate(skillPrefab, parent);
			skillUI.GetComponent<SkillUI>().CardUISet(result[i]);

			skillOb.Add(skillUI);

			yield return new WaitForSeconds(0.2f);
		}

		sc = null;
	}


	// TODO :: ���� ������ ������� �ٲ㵵 OK
	public Skill RandomCard()
	{
		int weight = 0;
		int selectNum = 0;

		// ���� ����� ���������� ��ȯ, ���� �Ҽ����� 5��� ¦���� ��ȯ
		selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));

		for (int i = 0; i < deck.Count; ++i)
		{
			weight += deck[i].weight;
			if (selectNum <= weight)
			{
				Skill temp = new Skill(deck[i]);
				return temp;
			}
		}
		return null;
	}
}
