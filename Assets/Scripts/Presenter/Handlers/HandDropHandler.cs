using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandDropHandler : MonoBehaviour, IDropHandler
{
  [SerializeField] GameObject player;
  public void OnDrop(PointerEventData eventData)
  {
    PlayerController script = player.GetComponent<PlayerController>();
    script.AddCardToHand(script.currentDraggableCard);
    player.GetComponent<PlayerController>().currentDraggableCard = null;
  }
}
