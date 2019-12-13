using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ContainerFlank : AbstractContainer
{
  public Flank flank;
  public CurrentPlayer currentPlayer;
  [SerializeField] public List<GameObject> squads;
  [SerializeField] PlayerController player;
  public void PlaceCard(Card card, int position)
  {
    Debug.Log(mCards[position]);
    mCards[position] = card;
  }
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
  public void DestroyDead()
  {
    foreach (var card in this.mCards)
    {
      if (((SquadCard)card.card).stamina <= 0)
      {
        mCards.Remove(card);
        Destroy(card);
        player.DropCardToDrop(card, player.game.GetCurrentStep() == currentPlayer);
      }
    }
  }
}
