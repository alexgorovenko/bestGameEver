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
  void Start()
  {
    for (var i = 0; i < 8; i++)
    {
      mCards.Add(null);
    }
  }
  public void PlaceCard(Card card, int position)
  {
    mCards[position] = card;
    card.position = position;
    RefreshActive();
  }
  public void RefreshActive()
  {
    Debug.Log("In Refresh");
    Debug.Log(mCards.Count);
    foreach (var card in this.mCards)
    {
      if (card != null)
      {
        Debug.Log(card.card.active);
        card.isSelectable = card.card.active;
      }
    }
  }
  public void DestroyDead()
  {
    for (var i = 0; i < 8; i++)
    {
      if (mCards[i] == null) continue;
      if (((SquadCard)mCards[i].card).stamina <= 0)
      {
        player.DropCardToDrop(mCards[i], player.game.GetCurrentStep() == currentPlayer);
        mCards[i] = null;
        CardPlaceholder _card = Instantiate(player.cardPlaceholder);
        _card.transform.SetParent(squads[i].transform);
      }
    }
  }
}
