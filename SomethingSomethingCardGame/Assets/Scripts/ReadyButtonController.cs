using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButtonController : MonoBehaviour
{
    bool clicked = false;
    PlayerManager playerManager;



    public void OnClick()
    {
        if(playerManager == null)
        {
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            playerManager = networkIdentity.GetComponent<PlayerManager>();
        }

        if (clicked)
        {
            clicked = false;
            GetComponentInChildren<Text>().text = "Ready";
            playerManager.CmdPlayerReady(playerManager.netId, false);
        }
        else
        {
            clicked = true;
            GetComponentInChildren<Text>().text = "Cancel";
            playerManager.CmdPlayerReady(playerManager.netId, true);
        }
    }

    public void ResetButton()
    {
        clicked = false;
        GetComponentInChildren<Text>().text = "Ready";

    }
}
