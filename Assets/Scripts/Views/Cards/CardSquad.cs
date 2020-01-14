using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class CardSquad : Card
{
    [SerializeField]
    public TextMeshProUGUI fieldAttack;
    [SerializeField]
    public TextMeshProUGUI fieldStamina;
    [SerializeField]
    public TextMeshProUGUI fieldDefence;
    public CardSquad attackCard;

    public void SetCard(SquadCard card)
    {
        if (card.GetType() == typeof(SupportCard)) isDraggable = false;
        this.card = card;
        fieldName.SetText(card.name);
        fieldAttack.SetText($"{card.attack}");
        fieldStamina.SetText($"{card.stamina}");
        fieldDefence.SetText($"{card.protection}");
    }

    public void Update()
    {
        if (this.card == null) return;
        fieldAttack.SetText($"{((SquadCard)card).attack}");
        fieldStamina.SetText($"{((SquadCard)card).stamina}");
        fieldDefence.SetText($"{((SquadCard)card).protection}");
    }
}
