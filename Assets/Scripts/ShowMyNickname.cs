using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Fusion;

public class ShowMyNickname : NetworkBehaviour
{
    public PlayerNickname playerNickname;
    void Start()
    {
        if(HasStateAuthority == true)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = playerNickname.NetworkedNickname;
    }
}
