using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ContainerFlank : AbstractContainer
{
  public Flank flank;
  public void RefreshActive ()
  {
    foreach (var card in this.mCards)
    {
      card.isSelectable = card.card.active;
    }
  }
}
