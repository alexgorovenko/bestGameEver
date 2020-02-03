using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ContainerFlank : AbstractContainer
{
    public Flank flank;
    public CurrentPlayer currentPlayer;
    [SerializeField]
    public List<GameObject> squads;
    [SerializeField]
    PlayerController player;
    public CommandorCard commandor;
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
        if (((SquadCard)card.card).skills.instantSkills != null)
        {
            foreach (var skill in ((SquadCard)card.card).skills.instantSkills)
            {
                skill.Item1(skill.Item2, player);
            }
        }
        RefreshActive();
    }
    public void RefreshActive()
    {
        foreach (var card in this.mCards)
        {
            if (card != null)
            {
                card.isSelectable = card.card.active;
            }
        }
    }
    public int DestroyDead()
    {
        int count = 0;
        for (var i = 0; i < 8; i++)
        {
            if (mCards[i] == null) continue;
            if (((SquadCard)mCards[i].card).stamina <= 0)
            {
                count++;
                player.DropCardToDrop(mCards[i], player.game.GetCurrentStep() == currentPlayer);
                mCards[i] = null;
                CardPlaceholder _card = Instantiate(player.cardPlaceholder);
                _card.transform.SetParent(squads[i % 4].transform, false);
            }
        }
        return count;
    }
    public void SetDrag(CardsContainerDropHandler.State state)
    {
        foreach (GameObject squad in this.squads)
        {
            squad.GetComponent<CardsContainerDropHandler>().state = state;
        }
    }
    public void SetEnabledDrag(bool isEnabled)
    {
        GetComponent<CardsContainerDropHandler>().enabled = isEnabled;
        foreach (GameObject squad in this.squads)
        {
            squad.GetComponent<CardsContainerDropHandler>().enabled = isEnabled;
        }
    }
    public int GetCardsCapacity ()
    {
        int result = 0;
        for (var i = 0; i < 8; i++)
        {
            if (this.mCards[i] != null)
            {
                result++;
            }
        }
        return result;
    }
}
