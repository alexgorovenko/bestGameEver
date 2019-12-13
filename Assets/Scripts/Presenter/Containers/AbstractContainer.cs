using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractContainer : MonoBehaviour
{
  protected List<Card> mCards = new List<Card>();
  public event EventHandler<CardContainerEventArgs> CardAdded;
  public void AddCard(Card item)
  {
    mCards.Add(item);
    item.OnAdd();

    if (CardAdded != null)
    {
      CardAdded(this, new CardContainerEventArgs(item));
    }
  }
  public void AddCards(List<Card> items)
  {
    foreach (var item in items)
    {
      AddCard(item);
    }
  }
  public void RemoveCard(Card item)
  {
    mCards.Remove(item);
    item.OnRemove();
  }
  public void RemoveCards(List<Card> items)
  {
    foreach (var item in items)
    {
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
  public List<Card> GetCards()
  {
    return this.mCards;
  }
}
