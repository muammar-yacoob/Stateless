using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    int playerIndex = 0;
    private void OnValidate()
    {
        playerIndex = transform.GetSiblingIndex();
        Debug.Log(playerIndex,this);
    }
}
