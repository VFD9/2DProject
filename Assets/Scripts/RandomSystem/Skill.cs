using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
	public string skillName;
	public Sprite skillImage;
	public int weight;

	public Skill(Skill skill)
	{
		this.skillName = skill.skillName;
		this.skillImage = skill.skillImage;
		this.weight = skill.weight;
	}
}
