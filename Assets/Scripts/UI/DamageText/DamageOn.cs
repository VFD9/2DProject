using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOn : MonoBehaviour
{
    public GameObject prefabDamage;

    public void DamagedTxt()
	{
		GameObject inst = Instantiate(prefabDamage, this.transform);
	}
}
