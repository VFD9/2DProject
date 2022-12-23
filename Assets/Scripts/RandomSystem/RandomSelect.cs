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

	// 랜덤하게 선택된 스킬을 담을 리스트
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
			// 비어있는 카드를 생성
			GameObject skillUI = Instantiate(skillPrefab, parent);
			skillUI.GetComponent<SkillUI>().CardUISet(result[i]);

			skillOb.Add(skillUI);

			yield return new WaitForSeconds(0.2f);
		}

		sc = null;
	}


	// TODO :: 랜덤 조건은 마음대로 바꿔도 OK
	public Skill RandomCard()
	{
		int weight = 0;
		int selectNum = 0;

		// 가장 가까운 정수값으로 변환, 만약 소수점이 5라면 짝수로 변환
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
