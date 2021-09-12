using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattlefieldCard : NetworkBehaviour
{
    [SerializeField] Image borderImage;
    public Color ownedColor;
    public Color lostColor;
    [SerializeField] Image creatureImage;
    [SerializeField] private List<AttackScoreController> attackScoreControllers;
    [SerializeField] private List<AttackScoreColor> attackScoreColors;

    uint originalOwnerId;
    uint currentOwner;
    CreatureCard creatureCard;

    public CreatureCard CreatureCard { get => creatureCard; set => creatureCard = value; }

    private void Awake()
    {
        attackScoreColors.Sort(AttackScoreColor.CompareByScore);
        attackScoreControllers.ForEach(controller => { controller.SetAttackScoreColors(attackScoreColors); });
    }

    public void SetControl(uint netId)
    {
        Debug.Log("Set Control " + netId, gameObject);
        currentOwner = netId;
        //Check if netId is client or opponent.
        if(netId == NetworkClient.connection.identity.netId)
        {
            borderImage.color = ownedColor;
        } else
        {
            borderImage.color = lostColor;
        }
    }

    [ClientRpc]
    public void RpcInit(int creatureID, uint originalOwnerNetId)
    {
        SetControl(originalOwnerNetId);
        CreatureCard newCard = FindObjectOfType<PlayerManager>().GetCardFromCardList(creatureID);
        creatureCard = newCard;
        originalOwnerId = originalOwnerNetId;
        GetComponent<InstantiatedCard>().playableCard = newCard;
        creatureImage.sprite = newCard.FaceImage;
        Debug.Log("New Card = " + newCard);
        foreach (Direction4 direction4 in Enum.GetValues(typeof(Direction4)))
        {
            attackScoreControllers.Where(controller => controller.direction4 == direction4).First()
                .SetAttackScore(newCard.GetAttackPointsByDirection(direction4));
        }
    }
}
