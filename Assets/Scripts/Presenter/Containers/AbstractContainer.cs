using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractContainer : MonoBehaviour
{
  private List<ICardContainerItem> mCards = new List<ICardContainerItem>();
  public event EventHandler<CardContainerEventArgs> CardAdded;
  public void AddCard(ICardContainerItem item)
  {
    mCards.Add(item);
    item.OnAdd();

    if (CardAdded != null)
    {
      CardAdded(this, new CardContainerEventArgs(item));
    }
  }
  public void AddCards(List<ICardContainerItem> items)
  {
      foreach (var item in items) {
          AddCard(item);
      }
  }
  public void RemoveCard(ICardContainerItem item)
  {
    mCards.Remove(item);
    item.OnRemove();
  }
  public void RemoveCards(List<ICardContainerItem> items)
  {
      foreach (var item in items) {
          RemoveCard(item);
      }
  }
  public void SetCardHandler(bool isActive)
  {
    foreach (var card in mCards)
    {
      card.SetActiveSelectHandler(isActive);
    }
  }
}
