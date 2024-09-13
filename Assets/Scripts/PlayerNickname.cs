using Fusion;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNickname : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(NicknameChanged))]

    public string NetworkedNickname { get; set; } = "Some nickname";


    void Start()
    {
        NetworkedNickname = GameManager.Instance.PlayerNickName;
        Debug.Log(NetworkedNickname);
    }

    void NicknameChanged()
    {
        Debug.Log("Nickname changed to: [NetworkedNickname]");
    }
}
