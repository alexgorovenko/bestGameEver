using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ContainerDeck : AbstractContainer
{
    public Card GetCard()
    {
        if (this.mCards.Count == 0) return null;
        Card card = this.mCards[0];
        this.mCards.RemoveAt(0);
        return card;
    }

    public List<Card> GetCards(int number)
    {
        number = Math.Min(number, this.mCards.Count);
        List<Card> cards = new List<Card>(number);
        for (var i = 0; i < number; i++)
        {
            cards.Add(this.GetCard());
        }
        return cards;
    }
}
