using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DropZone : NetworkBehaviour
{
    // Start is called before the first frame update
    public Vector2 position = new Vector2();
    private PlayerManager playerManager;
    public InstantiatedCard card;
    private void Start()
    {
      
    }


    //[ClientRpc]
    // private void RpcSpawnCardOnClient(GameObject card)
    // {
    //     this.GetComponent<BoxCollider2D>().enabled = false;
    //     Instantiate(card);
    //     card.transform.localPosition = Vector2.zero;
    //     card.transform.SetParent(this.gameObject.transform);
    // }
}

