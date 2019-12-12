using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelectHandler : MonoBehaviour, ISelectHandler
{
  CardView card;
  public void OnSelect(PointerEventData eventData)
  {
    card.player.GetComponent<PlayerController>().SelectCard(card.card);
  }
}
