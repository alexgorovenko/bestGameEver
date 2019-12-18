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

  private void BarbWireCallback(List<SquadCard> attacker, List<SquadCard> deffender, Skills attackerSkills, Skills deffenderSkills)
  {
    int count = attacker.Count;
    attacker[(int)Math.Round(UnityEngine.Random.value * count)].addProtection--;
    attacker[(int)Math.Round(UnityEngine.Random.value * count)].addProtection--;
  }

  private void FortifiedTrenchCallback(List<SquadCard> attacker, List<SquadCard> deffender, Skills attackerSkills, Skills deffenderSkills)
  {
    deffenderSkills.armor++;
  }

  public Game()
  {
    this.cards = new Dictionary<CurrentPlayer, List<AbstractCard>>();
    this.attacks = new Dictionary<CurrentPlayer, Attack>();
    this.fields = new Dictionary<CurrentPlayer, Field>();
    this.freeCommandors = new HashSet<CommandorCard>();
    Skills skills;
    List<Tuple<Skills.SkillCallback, int>> activeSkills;
    List<Tuple<Skills.SkillCallback, int>> instantSkills;
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
      skills.pierce = true;
      skills.armor = 1;
      skills.massDamage = 1;
      cards[player].Add(new SquadCard(Rarity.Epic, "Огнеметчики", "", 2, 2, 1, skills));

      instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
      instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Sapper, 2));
      skills = new Skills(null, instantSkills);
      skills.pierce = true;
      cards[player].Add(new SquadCard(Rarity.Epic, "Полевые инженеры", "", 2, 3, 2, skills));

      instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
      instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Scouting, 2));
      skills = new Skills(null, instantSkills);
      cards[player].Add(new SquadCard(Rarity.Epic, "Скауты", "", 2, 3, 2, skills));

      skills = new Skills();
      skills.armor = 1;
      skills.breakthrough = true;
      cards[player].Add(new SquadCard(Rarity.Epic, "Уланы", "", 4, 2, 1, skills));

      activeSkills = new List<Tuple<Skills.SkillCallback, int>>();
      activeSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Support, 1));
      activeSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Suppression, 2));
      skills = new Skills(activeSkills, null);
      cards[player].Add(new SquadCard(Rarity.Epic, "Пулеметчики", "", 0, 3, 2, skills));

      skills = new Skills();
      skills.armor = 1;
      skills.breakthrough = true;
      skills.agility = true;
      cards[player].Add(new SquadCard(Rarity.Legendary, "Прыгуны", "", 4, 2, 2, skills));

      activeSkills = new List<Tuple<Skills.SkillCallback, int>>();
      activeSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Support, 2));
      instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
      instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Shelling, 4));
      skills = new Skills(activeSkills, instantSkills);
      cards[player].Add(new SquadCard(Rarity.Legendary, "Техномаг", "", 2, 3, 2, skills));

      activeSkills = new List<Tuple<Skills.SkillCallback, int>>();
      activeSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Block, 1));
      activeSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Suppression, 1));
      skills = new Skills(activeSkills, null);
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
      // for (int i = 0; i < 3; i++)
      // {
      //   cards[player].Add(new SupportCard(Rarity.General, "Мобилизация", "", ViewAction.MOBILIZAZATION));
      // }
      // for (int i = 0; i < 3; i++)
      // {
      //   cards[player].Add(new SupportCard(Rarity.General, "Тактический ход", "", ViewAction.TACTICAL_MOVE));
      // }
      // for (int i = 0; i < 2; i++)
      // {
      //   cards[player].Add(new SupportCard(Rarity.Rare, "Снайпер", "", ViewAction.SNIPER));
      // }
      // cards[player].Add(new SupportCard(Rarity.Epic, "Рейд по тылам", "", ViewAction.REAR_RAID));
      // cards[player].Add(new SupportCard(Rarity.Legendary, "Полевая медицина", "", ViewAction.FIELD_MEDICINE));
    }
    // Командиры

    // Мастер защиты
    activeSkills = new List<Tuple<Skills.SkillCallback, int>>();
    activeSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Suppression, 1));
    skills = new Skills(activeSkills, null);
    freeCommandors.Add(new CommandorCard(Rarity.Rare, "Мастер защиты", "", skills, 1));

    // Мастер атаки
    activeSkills = new List<Tuple<Skills.SkillCallback, int>>();
    activeSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Support, 1));
    skills = new Skills(activeSkills, null);
    freeCommandors.Add(new CommandorCard(Rarity.Rare, "Мастер атаки", "", skills, 1));

    // Координатор
    activeSkills = new List<Tuple<Skills.SkillCallback, int>>();
    activeSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Shelling, 1));
    skills = new Skills(activeSkills, null);
    freeCommandors.Add(new CommandorCard(Rarity.Epic, "Координатор", "", skills, 2));

    // Ветеран войны
    skills = new Skills();
    skills.inspiration = 1;
    freeCommandors.Add(new CommandorCard(Rarity.Legendary, "Ветеран войны", "", skills, 1));

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

  public CurrentPlayer GetNextStep()
  {
    return currentStep == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST;
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

  public void HitSquad(SquadCard card, int damage)
  {
    card.stamina -= damage;
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
    // Debug.Log(fields[player].flanks);
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
    Debug.Log("in ref sq");
    foreach (CurrentPlayer player in Enum.GetValues(typeof(CurrentPlayer)))
    {
      foreach (Flank flank in Enum.GetValues(typeof(Flank)))
      {
        Debug.Log("in ref sq2");
        foreach (AbstractCard card in fields[player].flanks[flank])
        {
          Debug.Log("in ref sq3");
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
    foreach (Flank flank in Enum.GetValues(typeof(Flank)))
    {
      attacks[player].ApplySkillsAttacker(flank, fields[player].GetCommandor(flank).skills);
      attacks[player].ApplySkillsDeffender(flank, fields[player == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST].GetCommandor(flank).skills);
    }
    fields[player].Attack(attacks[player].getHeadquartesHurt());
    RefreshSquads();
  }

  public void ApplyAttack(CurrentPlayer player)
  {
    attacks[player].ApplyAttack();
    fields[player].ApplyAttack();
    RefreshSquads();
  }

  public int GetCardsCount(CurrentPlayer player, Flank flank)
  {
    return fields[player].flanks[flank].Count;
  }

  public int GetHeadsquaterHealth(CurrentPlayer player)
  {
    return fields[player].headquartesStrength;
  }

}
