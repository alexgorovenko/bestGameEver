using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ContainerDeck : AbstractContainer
{
  private List<ICardContainerItem> mCards = new List<ICardContainerItem>();
  public event EventHandler<CardContainerEventArgs> CardAdded;
  public override void addCard(ICardContainerItem item)
  {
    mCards.Add(item);
    item.onAdd();

    if (CardAdded != null)
    {
      CardAdded(this, new CardContainerEventArgs(item));
    }
  }
  public override void removeCard(ICardContainerItem item)
  {
    mCards.Remove(item);
    item.onRemove();
  }
}
