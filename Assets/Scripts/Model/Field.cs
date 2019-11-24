﻿using System.Collections.Generic;

public class Field
{
    public uint headquartesStrength { get; set; }
    private CommandorCard commandorLeft;
    private CommandorCard commandorRight;
    public Dictionary<Flang, HashSet<SquadCard>> flangs { get; }
    public HashSet<AbstractCard> onHand { get; }
    public HashSet<AbstractCard> drop { get; }


    public Field()
    {
        this.headquartesStrength = 30;
        this.onHand = new HashSet<AbstractCard>();
        this.drop = new HashSet<AbstractCard>();
    }

    public void AddToHand(List<AbstractCard> cards)
    {
        onHand.UnionWith(cards);
    }

    public void AddToHand(AbstractCard card)
    {
        onHand.Add(card);
    }

    public void Drop()
    {
        foreach (AbstractCard card in onHand)
        {
            if (card.GetType() == typeof(SquadCard) && ((SquadCard)card).stamina <= 0)
            {
                DropCard(card);
            } 
        }
            
    }

    public void DropCard(AbstractCard card)
    {
        onHand.Remove(card);
        drop.Add(card);
    }

    public void AddCommandor(CommandorCard commandor)
    {
        if (commandorLeft == null)
        {
            commandorLeft = commandor;
        }
        else if (commandorRight == null)
        {
            commandorRight = commandor;
        }
    }

    public void Attack(uint points)
    {
        this.headquartesStrength -= points;
    }
}