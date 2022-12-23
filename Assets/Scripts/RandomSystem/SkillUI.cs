using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public Image img;
    public Text skillName;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void CardUISet(Skill skill)
	{
        img.sprite = skill.skillImage;
        skillName.text = skill.skillName;
	}
}
