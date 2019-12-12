using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelectHandler : MonoBehaviour, ISelectHandler
{
  public void OnSelect(BaseEventData eventData)
  {
    PlayerController player = gameObject.GetComponent<Card>().player.GetComponent<PlayerController>();
    player.SelectCard(gameObject);
  }
}
