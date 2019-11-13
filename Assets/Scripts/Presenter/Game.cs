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
        Skills skills;
        players[CurrentPlayer.FIRST] = player1;
        players[CurrentPlayer.SECOND] = player2;
        foreach (CurrentPlayer player in Enum.GetValues(typeof(CurrentPlayer)))
        {
            attacks[player] = new Attack();
            fields[player] = new Field();
            cards[player] = new List<AbstractCard>(40);
            for (int i = 0; i < 3; i++)
            {
                skills = new Skills();
                cards[player].Add(new SquadCard(Rarity.General, "Линейная пехота", "", 2, 2, 2, skills));
            }
            for (int i = 0; i < 3; i++)
            {
                skills = new Skills();
                skills.agility = true;
                cards[player].Add(new SquadCard(Rarity.General, "Штрафники", "", 3, 1, 2, skills));
            }
            for (int i = 0; i < 3; i++)
            {
                skills = new Skills();
                cards[player].Add(new SquadCard(Rarity.General, "Окопники", "", 1, 2, 3, skills));
            }
            for (int i = 0; i < 3; i++)
            {
                skills = new Skills();
                skills.armor = 1;
                cards[player].Add(new SquadCard(Rarity.Rare, "Штурмовики", "", 3, 2, 1, skills));
            }
            for (int i = 0; i < 2; i++)
            {
                skills = new Skills();
                skills.agility = true;
                cards[player].Add(new SquadCard(Rarity.Rare, "Драгуны", "", 3, 2, 2, skills));
            }
            for (int i = 0; i < 2; i++)
            {
                skills = new Skills();
                skills.inspiration = 1;
                cards[player].Add(new SquadCard(Rarity.Rare, "Офицер", "", 2, 2, 2, skills));
            }
            skills = new Skills();
            skills.armorPiercing = true;
            skills.armor = 1;
            skills.massDamage = 1;
            cards[player].Add(new SquadCard(Rarity.Epic, "Огнеметчики", "", 2, 2, 1, skills));
            skills = new Skills();
            skills.sapper = 2;
            skills.armorPiercing = true;
            cards[player].Add(new SquadCard(Rarity.Epic, "Полевые инженеры", "", 2, 3, 2, skills));
            skills = new Skills();
            skills.intelligenceService = 2;
            cards[player].Add(new SquadCard(Rarity.Epic, "Скауты", "", 2, 3, 2, skills));
            skills = new Skills();
            skills.armor = 1;
            skills.breakthrough = true;
            cards[player].Add(new SquadCard(Rarity.Epic, "Уланы", "", 4, 2, 1, skills));
            skills = new Skills();
            skills.support = 1;
            skills.suppression = 2;
            cards[player].Add(new SquadCard(Rarity.Epic, "Пулеметчики", "", 0, 3, 2, skills));
            skills = new Skills();
            skills.medic = 2;
            cards[player].Add(new SquadCard(Rarity.Epic, "Полевой медик", "", 2, 3, 2, skills));
            skills = new Skills();
            skills.armor = 1;
            skills.breakthrough = true;
            skills.agility = true;
            cards[player].Add(new SquadCard(Rarity.Legendary, "Прыгуны", "", 4, 2, 2, skills));
            skills = new Skills();
            skills.shelling = 4;
            skills.support = 2;
            cards[player].Add(new SquadCard(Rarity.Legendary, "Техномаг", "", 2, 3, 2, skills));
            skills = new Skills();
            skills.block = 1;
            skills.suppression = 1;
            cards[player].Add(new SquadCard(Rarity.Legendary, "Дворфы защитники", "", 2, 4, 2, skills));
            skills = new Skills();
            skills.armor = 2;
            cards[player].Add(new SquadCard(Rarity.Legendary, "Танк", "", 2, 5, 0, skills));
        }
        skills = new Skills();
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
