using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Card : MonoBehaviour, ICardContainerItem
{
  public string Name { get; }
  [SerializeField] public Sprite Image { get; }

  public void OnAdd() { }
  public void OnPlay() { }
  public void OnRemove() { }
  public Card(string name)
  {
    this.Name = name;
  }
  public void SetActiveSelectHandler(bool state)
  {

  }
}
