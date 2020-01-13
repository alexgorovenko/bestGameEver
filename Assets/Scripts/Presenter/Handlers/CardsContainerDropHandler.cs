using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardsContainerDropHandler : MonoBehaviour, IDropHandler
{
    [SerializeField]
    PlayerController player;
    [SerializeField]
    ContainerFlank flank;
    public void OnDrop(PointerEventData eventData)
    {
        switch (player.currentDraggableCard.GetComponent<Card>().card)
        {
            case SupportCard s:
                return;
        }
        int _position = player.currentDraggableCard.GetComponent<Card>().position;
        Debug.Log("TADA");
        Debug.Log(_position);
        if (_position != -1)
        {
            CardPlaceholder _card = Instantiate(player.cardPlaceholder);
            Transform container = flank.transform.Find("Squads").Find($"CardsContainer-{_position}");
            _card.transform.SetParent(container.transform, false);
        }
        int position = gameObject.name[gameObject.name.Length - 1] - '0';
        Debug.Log(position);
        player.DropCardToFlank(player.currentDraggableCard, position, flank);
        player.currentDraggableCard = null;
    }
}
