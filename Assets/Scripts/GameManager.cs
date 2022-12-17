using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ManagerSingleton<GameManager>
{
    public bool isScroll = true;

    void Update()
    {
        BackgroundManager.Instance.SetScroll(this.isScroll);
    }
}
