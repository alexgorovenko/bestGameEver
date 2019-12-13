using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardsContainerDropHandler : MonoBehaviour, IDropHandler
{
  [SerializeField] PlayerController player;
  [SerializeField] ContainerFlank flank;
  public void OnDrop(PointerEventData eventData)
  {
    switch (player.GetComponent<PlayerController>().currentDraggableCard.GetComponent<Card>().card)
    {
      case SupportCard s:
        return;
    }

    int position = gameObject.name[gameObject.name.Length - 1] - '0';

    player.GetComponent<PlayerController>().DropCardToFlank(
      player.GetComponent<PlayerController>().currentDraggableCard,
      position,
      flank);
    player.GetComponent<PlayerController>().currentDraggableCard = null;
  }
}
