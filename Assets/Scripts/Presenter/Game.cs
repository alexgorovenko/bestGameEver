using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController
{
    private Dictionary<CurrentPlayer, AbstractController> players;
    private Dictionary<CurrentPlayer, List<AbstractCard>> cards;
    private Dictionary<CurrentPlayer, Attack> attacks;
    private Dictionary<CurrentPlayer, Field> fields;
    public HashSet<CommandorCard> freeCommandors { get; }

    public GameController(AbstractController player1, AbstractController player2)
    {
        players[CurrentPlayer.FIRST] = player1;
        players[CurrentPlayer.SECOND] = player2;
        foreach (CurrentPlayer player in Enum.GetValues(typeof(CurrentPlayer)))
        {
            attacks[player] = new Attack();
            fields[player] = new Field();
        }
        Skills skills = new Skills();
        skills.suppression = 1;
        freeCommandors.Add(new CommandorCard(Rarity.Rare, "Мастер защиты", "", skills, 1));
        skills = new Skills();
        skills.support = 1;
        freeCommandors.Add(new CommandorCard(Rarity.Rare, "Мастер атаки", "", skills, 1));
        skills = new Skills();
        skills.shelling = 1;
        freeCommandors.Add(new CommandorCard(Rarity.Epic, "Координатор", "", skills, 2));
        skills = new Skills();
        skills.inspiration = 1;
        freeCommandors.Add(new CommandorCard(Rarity.Legendary, "Ветеран", "", skills, 1));
    }

    public void SetCommandor(CurrentPlayer player, CommandorCard commandor)
    {
        fields[player].AddCommandor(commandor);
    }

    public List<AbstractCard> GetSeveralCards(CurrentPlayer player, int numberOfCards)
    {
        List<AbstractCard> cards = this.cards[player].GetRange(0, numberOfCards);
        this.cards[player].RemoveRange(0, numberOfCards);
        return cards;
    }

    public void AddCardsToHand(CurrentPlayer player, List<AbstractCard> cards)
    {
        fields[player].AddToHand(cards);
    }
    
}
