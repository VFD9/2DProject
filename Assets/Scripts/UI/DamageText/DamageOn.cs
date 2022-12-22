using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOn : MonoBehaviour
{
    public GameObject prefabDamage;

    public void DamagedTxt(Transform _root, float damage, Color ? color = null)
	{
		DamagePopupRoot inst = Instantiate(prefabDamage, _root).GetComponent<DamagePopupRoot>();
		inst.popup.Initialize(_root, damage, color);
	}
}
