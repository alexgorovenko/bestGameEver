using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardsContainerDropHandler : MonoBehaviour, IDropHandler
{
    public enum State
    {
        ATTACK,
        DEFENCE
    };
    [SerializeField]
    PlayerController player;
    [SerializeField]
    ContainerFlank flank;
    public State state = State.ATTACK;
    public void OnDrop(PointerEventData eventData)
    {
        if (this.state == State.ATTACK) {
            switch (player.currentDraggableCard.GetComponent<Card>().card)
            {

                case SupportCard s:
                    return;
            }
            int _position = player.currentDraggableCard.GetComponent<Card>().position;
            if (_position != -1)
            {
                CardPlaceholder _card = Instantiate(player.cardPlaceholder);
                Transform container = flank.transform.Find("Squads").Find($"CardsContainer-{_position}");
                _card.transform.SetParent(container.transform, false);
            }
            int position = gameObject.name[gameObject.name.Length - 1] - '0';
            player.DropCardToFlank(player.currentDraggableCard, position, flank);
            player.currentDraggableCard = null;
        }
        else if (this.state == State.DEFENCE)
        {
            if (player.currentDraggableCard.GetComponent<Card>().card is SquadCard &&
                player.currentDraggableCard.GetComponent<Card>().card.active) {
                int _position = gameObject.name[gameObject.name.Length - 1] - '0';
                CardSquad card = (CardSquad)player.currentDraggableCard.GetComponent<Card>();
                if (_position != -1)
                {
                    card.attackCard = player.GetOppositeCard(card, _position);
                    if (card.attackCard)
                    {
                        card.attackCard.attackCard = card;
                    }
                }
                else
                {
                    if (card.attackCard)
                    {
                        card.attackCard.attackCard = null;
                    }
                    card.attackCard = null;
                }
                card.Highlight(card.attackCard != null);
            }
        }
    }
}
