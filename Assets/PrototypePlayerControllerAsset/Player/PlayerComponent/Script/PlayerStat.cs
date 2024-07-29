using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    PlayerItem playerItem;

    public PlayerItem GetPlayerItem()
    {
        return playerItem;
    }

    public PlayerItem SetPlayerItem(PlayerItem playerItem)
    {
        return this.playerItem = playerItem;
    }
}
