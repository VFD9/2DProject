using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    IEnumerator Create()
	{
		yield return new WaitForSeconds(5.0f);

		CreateManager.Instance.CreateObject(gameObject);
	}
}
