using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : ManagerSingleton2<BackgroundManager>
{
	public Background[] backgrounds;

	private float _speed = 5;
	private bool _isScroll = true;

	private float[] _leftPosX = new float[2];
	private float[] _rightPosX = new float[2];

	void Start()
	{
		for(int i = 0; i < backgrounds.Length; ++i)
		{
			Vector2 vect = backgrounds[i]._bg.sprite.rect.size /
				backgrounds[i]._bg.sprite.pixelsPerUnit; // 픽셀단위로 조정해서 씬에 올리가기 때문에 나눠야한다.

			_leftPosX[i] = -(vect.x);
			_rightPosX[i] = vect.x;
		}
	}

	void Update()
	{
		if(this._isScroll)
		{
			for(int i = 0; i < backgrounds.Length; ++i)
			{
				backgrounds[i].gameObject.transform.position +=
					new Vector3(-_speed, 0, 0) * Time.deltaTime;

				if(backgrounds[i].gameObject.transform.position.x < _leftPosX[i] * 1.8f)
				{
					int nIndex = (i == 0) ? 1 : 0;

					Vector3 nextPos = backgrounds[nIndex].gameObject.transform.position;
					nextPos = new Vector3(nextPos.x + _rightPosX[i] + (_rightPosX[i] / 2),
						nextPos.y, nextPos.z);
					backgrounds[i].gameObject.transform.position = nextPos;
				}
			}
		}
	}

	public void SetScroll(bool isScroll)
	{
		this._isScroll = isScroll;
		this._speed = (isScroll) ? 5 : 0;
	}
}
