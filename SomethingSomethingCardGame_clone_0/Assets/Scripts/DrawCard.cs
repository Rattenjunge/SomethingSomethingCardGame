using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class DrawCard : NetworkBehaviour
{
    private PlayerManager playerManager;
    public void OnClick()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        playerManager = networkIdentity.GetComponent<PlayerManager>();
        playerManager.DealCards();
    }
}
