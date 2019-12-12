using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractContainer : MonoBehaviour
{
  private List<ICardContainerItem> mCards = new List<ICardContainerItem>();
  public event EventHandler<CardContainerEventArgs> CardAdded;
  public void addCard(ICardContainerItem item)
  {
    mCards.Add(item);
    item.OnAdd();

    if (CardAdded != null)
    {
      CardAdded(this, new CardContainerEventArgs(item));
    }
  }
  public void removeCard(ICardContainerItem item)
  {
    mCards.Remove(item);
    item.OnRemove();
  }
  public void SetCardHandler(bool isActive)
  {
    foreach (var card in mCards)
    {
      card.SetActiveSelectHandler(isActive);
    }
  }
}
