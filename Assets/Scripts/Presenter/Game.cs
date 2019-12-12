using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
  private Dictionary<CurrentPlayer, List<AbstractCard>> cards;
  private Dictionary<CurrentPlayer, Attack> attacks;
  private Dictionary<CurrentPlayer, Field> fields;
  private CurrentPlayer currentStep;
  public HashSet<CommandorCard> freeCommandors { get; }

  private void BarbWireCallback(List<SquadCard> attacker, List<SquadCard> deffender)
  {
    int count = attacker.Count;
    attacker[(int)Math.Round(UnityEngine.Random.value * count)].protection--;
    attacker[(int)Math.Round(UnityEngine.Random.value * count)].protection--;
  }

  private void FortifiedTrenchCallback(List<SquadCard> attacker, List<SquadCard> deffender)
  {
    foreach (SquadCard deff in deffender)
    {
      deff.skills.armor++;
    }
  }

  public Game()
  {
    this.cards = new Dictionary<CurrentPlayer, List<AbstractCard>>();
    this.attacks = new Dictionary<CurrentPlayer, Attack>();
    this.fields = new Dictionary<CurrentPlayer, Field>();
    this.freeCommandors = new HashSet<CommandorCard>();
    Skills skills;
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
      for (int i = 0; i < 2; i++)
      {
        cards[player].Add(new FortificationCard(Rarity.General, "Колючая проволока", "", this.BarbWireCallback));
      }
      for (int i = 0; i < 2; i++)
      {
        cards[player].Add(new FortificationCard(Rarity.Rare, "Укрепленная траншея", "", this.FortifiedTrenchCallback));
      }
      for (int i = 0; i < 3; i++)
      {
        cards[player].Add(new SupportCard(Rarity.General, "Мобилизация", "", ViewAction.MOBILIZAZATION));
      }
      for (int i = 0; i < 3; i++)
      {
        cards[player].Add(new SupportCard(Rarity.General, "Тактический ход", "", ViewAction.TACTICAL_MOVE));
      }
      for (int i = 0; i < 2; i++)
      {
        cards[player].Add(new SupportCard(Rarity.Rare, "Снайпер", "", ViewAction.SNIPER));
      }
      cards[player].Add(new SupportCard(Rarity.Epic, "Рейд по тылам", "", ViewAction.REAR_RAID));
      cards[player].Add(new SupportCard(Rarity.Legendary, "Газы", "", ViewAction.GASES));
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
    currentStep = UnityEngine.Random.value > 0.5f ? CurrentPlayer.FIRST : CurrentPlayer.SECOND;
  }

  public void SetCommandor(CurrentPlayer player, CommandorCard commandor)
  {
    fields[player].AddCommandor(commandor);
  }

  public List<AbstractCard> GetSeveralCards(CurrentPlayer player, int numberOfCards)
  {
    List<AbstractCard> cards = new List<AbstractCard>(numberOfCards);
    for (uint i = 0; i < numberOfCards; i++)
    {
      int index = UnityEngine.Random.Range(0, this.cards[player].Count);
      cards.Add(this.cards[player][index]);
      this.cards[player].RemoveAt(index);
    }
    return cards;
  }

  public int GetRemainedCards(CurrentPlayer player)
  {
    return this.cards[player].Count;
  }

  public CurrentPlayer GetCurrentStep()
  {
    return currentStep;
  }

  public void AddCardsToHand(CurrentPlayer player, List<AbstractCard> cards)
  {
    fields[player].AddToHand(cards);
  }

  public void ApplyFortificationCard(CurrentPlayer player, FortificationCard card, Flank flank)
  {
    attacks[player == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST].SetFortificationCard(card, flank);
  }

  public void NextStep()
  {
    currentStep = (currentStep == CurrentPlayer.FIRST) ? CurrentPlayer.SECOND : CurrentPlayer.FIRST;
  }

  public HashSet<AbstractCard> GetCardsOnHand(CurrentPlayer player)
  {
    return this.fields[player].onHand;
  }

  public HashSet<AbstractCard> GetCardsFromDrop(CurrentPlayer player)
  {
    return this.fields[player].drop;
  }

  public void HitSquad(SquadCard card, Skills skills)
  {
    card.stamina -= (int)skills.shelling;
  }

  public void DropFortification(CurrentPlayer player, Flank flank)
  {
    fields[player].DropCard(attacks[player].GetFortificationCard(flank));
    attacks[player].SetFortificationCard(null, flank);
  }

  public void DropCardFromHand(CurrentPlayer player, AbstractCard card)
  {
    fields[player].DropCard(card);
  }

  public void AddCardsToFlank(CurrentPlayer player, HashSet<SquadCard> cards, Flank flank)
  {
    // Debug.Log(player);
    // Debug.Log(cards);
    // Debug.Log(flank);
    Debug.Log(fields[player].flanks);
    fields[player].flanks[flank].UnionWith(cards);
  }

  public HashSet<AbstractCard> GetActiveCardsFromFlank(CurrentPlayer player, Flank flank)
  {
    HashSet<AbstractCard> cards = new HashSet<AbstractCard>();
    foreach (AbstractCard card in fields[player].flanks[flank])
    {
      if (card.active)
      {
        cards.Add(card);
      }
    }
    return cards;
  }

  public void RefreshSquads()
  {
    foreach (CurrentPlayer player in Enum.GetValues(typeof(CurrentPlayer)))
    {
      foreach (Flank flank in Enum.GetValues(typeof(Flank)))
      {
        foreach (AbstractCard card in fields[player].flanks[flank])
        {
          card.active = true;
        }
      }
    }
  }

  public void SetAttackers(CurrentPlayer player, List<SquadCard> cards, Flank flank)
  {
    attacks[player].AddAttackers(cards, flank);
  }

  public void SetDeffenders(CurrentPlayer player, List<SquadCard> cards, Flank flank)
  {
    attacks[player == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST].AddDeffenders(cards, flank);
  }

  public void Attack(CurrentPlayer player)
  {
    fields[player].Attack(attacks[player].getHeadquartesHurt());
  }

  public int GetCardsCount(CurrentPlayer player, Flank flank)
  {
    return fields[player].flanks[flank].Count;
  }

}
