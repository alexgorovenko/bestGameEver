using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Card : MonoBehaviour, ICardContainerItem
{
  string Name;
  [SerializeField] Sprite Image;
  public Card(string name)
  {
    this.Name = name;
  }

  public void SetActiveSelectHandler(bool state)
  {

  }
}
