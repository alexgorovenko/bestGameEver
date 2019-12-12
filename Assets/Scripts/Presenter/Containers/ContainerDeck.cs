using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ContainerDeck : AbstractContainer
{
  public ICardContainerItem GetCard()
  {
    ICardContainerItem card = this.mCards[0];
    this.mCards.RemoveAt(0);
    return card;
  }

  public List<ICardContainerItem GetCards(int number)
  {
    List<ICardContainerItem> cards = new List<ICardContainerItem>(number);
    for (var i = 0; i < number; i++) {
      cards.Add(this.GetCard());
    }
    return cards;
  }
}
