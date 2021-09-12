using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CardMover : NetworkBehaviour
{

    public GameObject Canvas;

    private bool isDraggable = true;
    public bool isOverDropzone = false;
    public bool isDragging = false;
    private GameObject startParent;
    private Vector2 startPosition;
    private GameObject dropZone;
    private PlayerManager playerManager;
    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("Canvas");
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        playerManager = networkIdentity.GetComponent<PlayerManager>();
    }

    public void StartDrag()
    {
        if (isDragging == true)
        {
            return;
        }

        if (isDraggable == false)
        {
            return;
        }

        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;

    }

    public void EndDrag()
    {
        if (isDragging == false)
        {
            return;
        }
        if (isDraggable == false)
        {
            return;
        }
        isDragging = false;
        if (isOverDropzone)
        {
            transform.SetParent(dropZone.transform, true);
            (transform as RectTransform).anchorMin = new Vector2(0.5f, 0.5f);
            (transform as RectTransform).anchorMax = new Vector2(0.5f, 0.5f);
            (transform as RectTransform).localPosition = Vector2.zero;

            playerManager.CmdCreateCardOnServer(dropZone, GetComponent<InstantiatedCard>().playableCard.Id,playerManager.netId);
            Destroy(this.gameObject);
            playerManager.HandCards.Remove(this.gameObject);
            isDraggable = false;

        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(Canvas.transform, true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropzone = true;
        dropZone = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropzone = false;
        dropZone = null;
    }

}
