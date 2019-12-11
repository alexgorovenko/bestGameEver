using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropHandler : MonoBehaviour, IDropHandler
{
  [SerializeField] GameObject player;
  public void OnDrop(PointerEventData eventData)
  {
    if (gameObject.GetComponent<ContainerFlank>() != null)
    {
      switch (player.GetComponent<PlayerController>().currentDraggableCard.GetComponent<CardView>().card)
      {
        case SupportCard s:
          return;
      }
    }
    PlayerController script = player.GetComponent<PlayerController>();
    script.DropCardToFlank(script.currentDraggableCard, gameObject);
    player.GetComponent<PlayerController>().currentDraggableCard = null;
  }
}
