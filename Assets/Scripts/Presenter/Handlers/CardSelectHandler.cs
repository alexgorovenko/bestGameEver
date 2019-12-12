using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelectHandler : MonoBehaviour
{
  public void OnSelect()
  {
    Debug.Log("TADA");
    PlayerController player = gameObject.GetComponent<Card>().player.GetComponent<PlayerController>();
    player.SelectCard(gameObject);
  }
}
