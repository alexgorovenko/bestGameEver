using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ContainerFlank : AbstractContainer
{
  public Flank flank;
  private List<ICardContainerItem> mCards = new List<ICardContainerItem>();
  public event EventHandler<CardContainerEventArgs> CardAdded;
  public override void AddCard(ICardContainerItem item)
  {
    mCards.Add(item);
    item.onAdd();

    if (CardAdded != null)
    {
      CardAdded(this, new CardContainerEventArgs(item));
    }
  }
  public override void RemoveCard(ICardContainerItem item)
  {
    mCards.Remove(item);
    item.onRemove();
  }
}
