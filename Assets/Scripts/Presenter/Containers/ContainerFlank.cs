using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ContainerFlank : AbstractContainer
{
  public Flank flank;
  public void RefreshActive()
  {
    Debug.Log("In Refresh");
    Debug.Log(mCards.Count);
    foreach (var card in this.mCards)
    {
      Debug.Log(card.card.active);
      card.isSelectable = card.card.active;
    }
  }
}
