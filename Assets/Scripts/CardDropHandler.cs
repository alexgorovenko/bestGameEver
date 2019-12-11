using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropHandler : MonoBehaviour, IDropHandler
{
  [SerializeField] GameObject player;
  public void OnDrop(PointerEventData eventData)
  {
    PlayerController script = player.GetComponent<PlayerController>();
    script.PlaceCardToFlank(script.currentDraggableCard, gameObject);
    player.GetComponent<PlayerController>().currentDraggableCard = null;
  }
}
