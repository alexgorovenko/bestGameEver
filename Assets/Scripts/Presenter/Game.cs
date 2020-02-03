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
    public HashSet<CommandorCard> freeCommandors1 { get; }
    public HashSet<CommandorCard> freeCommandors2 { get; }

    private void CityRuinsCallback(List<SquadCard> enemySquads, List<SquadCard> mySquads, Skills enemySkills, Skills mySkills, bool isMyAttack)
    {
        for (int i = 0; i < 4; i++)
        {
            if (enemySquads[i] != null && enemySquads[i].skills.agility)
            {
                enemySquads[i].skills.agility = false;
                enemySquads[i] = null;
            }
            if (mySquads[i] != null && mySquads[i].skills.agility)
            {
                mySquads[i].skills.agility = false;
                mySquads[i] = null;
            }
        }

    }

    private void BarricadeCallback(List<SquadCard> enemySquads, List<SquadCard> mySquads, Skills enemySkills, Skills mySkills, bool isMyAttack)
    {
        mySkills.armor++;
    }

    private void LivingBlackThornCallback(List<SquadCard> enemySquads, List<SquadCard> mySquads, Skills enemySkills, Skills mySkills, bool isMyAttack)
    {
        if (isMyAttack) return;
        List<int> indexes = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            if (enemySquads[i] != null)
            {
                indexes.Add(i);
            }
        }
        if (indexes.Count == 0) return;
        enemySquads[indexes[(int)Math.Round(UnityEngine.Random.value * (indexes.Count - 1))]].addStamina--;
    }

    private void ClanBannerCallback(List<SquadCard> enemySquads, List<SquadCard> mySquads, Skills enemySkills, Skills mySkills, bool isMyAttack)
    {
        for (int i = 0; i < 4; i++)
        {
            if (mySquads[i] == null) continue;
            mySquads[i].addProtection++;
        }
    }

    private List<AbstractCard> CreateFirstSetOfCards()
    {
        Skills skills;
        List<Tuple<Skills.SkillCallback, int>> instantSkills;
        List<AbstractCard> cards = new List<AbstractCard>(40);
        for (int i = 0; i < 3; i++)
        {
            skills = new Skills();
            skills.brotherhood = true;
            cards.Add(new SquadCard(Rarity.General, "Добровольческая бригада", "", 1, 1, 1, skills));
            cards[cards.Count - 1].priority = 6;
        }
        for (int i = 0; i < 3; i++)
        {
            skills = new Skills();
            skills.armor = 1;
            cards.Add(new SquadCard(Rarity.General, "Рабочий автоматон", "", 2, 2, 1, skills));
            cards[cards.Count - 1].priority = 7;
        }
        for (int i = 0; i < 3; i++)
        {
            instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
            instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Scouting, 1));
            skills = new Skills(instantSkills);
            cards.Add(new SquadCard(Rarity.General, "Контрабандист-подпольщик", "", 1, 1, 1, skills));
            cards[cards.Count - 1].priority = 8;
        }
        for (int i = 0; i < 3; i++)
        {
            skills = new Skills();
            skills.brotherhood = true;
            cards.Add(new SquadCard(Rarity.Rare, "5-я нортумберлендская бригада", "", 0, 3, 1, skills));
            cards[cards.Count - 1].priority = 9;
        }
        for (int i = 0; i < 2; i++)
        {
            skills = new Skills();
            skills.inspiration = 1;
            cards.Add(new SquadCard(Rarity.Rare, "Агитатор", "", 1, 3, 1, skills));
            cards[cards.Count - 1].priority = 10;
        }
        for (int i = 0; i < 2; i++)
        {
            instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
            instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Shelling, 2));
            skills = new Skills(instantSkills);
            cards.Add(new SquadCard(Rarity.Rare, "Бомбисты", "", 3, 2, 1, skills));
            cards[cards.Count - 1].priority = 11;
        }
        skills = new Skills();
        skills.massDamage = 1;
        cards.Add(new SquadCard(Rarity.Epic, "Огнемётчики", "", 2, 3, 0, skills));
        cards[cards.Count - 1].priority = 19;

        instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
        instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Shelling, 2));
        skills = new Skills(instantSkills);
        skills.armor = 1;
        cards.Add(new SquadCard(Rarity.Epic, "Боевой автоматон", "", 3, 2, 2, skills));
        cards[cards.Count - 1].priority = 18;

        skills = new Skills();
        skills.agility = true;
        skills.inspiration = 1;
        skills.brotherhood = true;
        cards.Add(new SquadCard(Rarity.Epic, "Герой сопротивления", "", 1, 1, 1, skills));
        cards[cards.Count - 1].priority = 17;

        instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
        instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Sapper, 1));
        skills = new Skills();
        skills.pierce = true;
        cards.Add(new SquadCard(Rarity.Epic, "Инженеры-подрывники", "", 2, 3, 2, skills));
        cards[cards.Count - 1].priority = 15;

        skills = new Skills();
        skills.agility = true;
        skills.armor = 2;
        cards.Add(new SquadCard(Rarity.Epic, "\"непреклонные\"", "", 0, 3, 2, skills));
        cards[cards.Count - 1].priority = 16;

        skills = new Skills();
        skills.agility = true;
        skills.armor = 1;
        cards.Add(new SquadCard(Rarity.Epic, "\"железнобокие\"", "", 4, 2, 0, skills));
        cards[cards.Count - 1].priority = 20;

        skills = new Skills();
        skills.pierce = true;
        skills.brotherhood = true;
        cards.Add(new SquadCard(Rarity.Legendary, "33й нотингемский гвардейский полк", "", 0, 3, 2, skills));
        cards[cards.Count - 1].priority = 21;

        instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
        instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Shelling, 3));
        skills = new Skills(instantSkills);
        skills.massDamage = 1;
        skills.pierce = true;
        cards.Add(new SquadCard(Rarity.Legendary, "Техномаг", "", 2, 2, 2, skills));
        cards[cards.Count - 1].priority = 23;

        skills = new Skills();
        skills.inspiration = 2;
        skills.brotherhood = true;
        cards.Add(new SquadCard(Rarity.Legendary, "\"Бедняк\" Таллер", "", 1, 2, 1, skills));
        cards[cards.Count - 1].priority = 24;

        instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
        instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Shelling, 1));
        skills = new Skills(instantSkills);
        skills.armor = 2;
        cards.Add(new SquadCard(Rarity.Legendary, "Паровой танк", "", 3, 4, 0, skills));
        cards[cards.Count - 1].priority = 22;

        for (int i = 0; i < 2; i++)
        {
            cards.Add(new FortificationCard(Rarity.General, "Городские руины", "", this.CityRuinsCallback));
            cards[cards.Count - 1].priority = 2;
        }

        for (int i = 0; i < 2; i++)
        {
            cards.Add(new FortificationCard(Rarity.Rare, "Баррикада", "", this.BarricadeCallback));
            cards[cards.Count - 1].priority = 5;
        }

        for (int i = 0; i < 3; i++)
        {
            cards.Add(new SupportCard(Rarity.General, "Восстание", "", ViewAction.INSURRECTION));
            cards[cards.Count - 1].priority = 3;
        }
        for (int i = 0; i < 3; i++)
        {
            cards.Add(new SupportCard(Rarity.General, "Контрабанда", "", ViewAction.SMUGGLING));
        }
        for (int i = 0; i < 2; i++)
        {
            cards.Add(new SupportCard(Rarity.Rare, "Засада", "", ViewAction.AMBUSH));
        }
        cards.Add(new SupportCard(Rarity.Epic, "Налёт на скалы", "", ViewAction.RAIDONTHEROCKS));
        cards.Add(new SupportCard(Rarity.Epic, "Полевая медицина", "", ViewAction.FIELDMEDICINE));
        return cards;
    }

    private HashSet<CommandorCard> CreateFirstSetOfCommandors()
    {
        Skills skills;
        HashSet<CommandorCard> commandors = new HashSet<CommandorCard>();
        // Мастер защиты
        skills = new Skills();
        skills.revenge = true;
        commandors.Add(new CommandorCard(Rarity.Rare, "Лидер сопротивления", "", skills, 1));

        // Мастер атаки
        skills = new Skills();
        skills.pierce = true;
        commandors.Add(new CommandorCard(Rarity.Rare, "Алхимик-экспериментатор", "", skills, 1));

        // Координатор
        skills = new Skills();
        skills.forceShelling = true;
        commandors.Add(new CommandorCard(Rarity.Epic, "Координатор", "", skills, 2));

        // Ветеран войны
        skills = new Skills();
        skills.inspiration = 1;
        commandors.Add(new CommandorCard(Rarity.Legendary, "Плазменный оратор", "", skills, 1));
        return commandors;
    }

    private List<AbstractCard> CreateSecondSetOfCards()
    {
        Skills skills;
        List<Tuple<Skills.SkillCallback, int>> instantSkills;
        List<AbstractCard> cards = new List<AbstractCard>(40);
        for (int i = 0; i < 3; i++)
        {
            skills = new Skills();
            cards.Add(new SquadCard(Rarity.General, "14й полк королевской линейной пехоты", "", 2, 2, 2, skills));
        }
        for (int i = 0; i < 3; i++)
        {
            skills = new Skills();
            skills.agility = true;
            cards.Add(new SquadCard(Rarity.General, "Горцы защитники", "", 0, 2, 3, skills));
        }
        for (int i = 0; i < 3; i++)
        {
            instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
            instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Scouting, 1));
            skills = new Skills(instantSkills);
            cards.Add(new SquadCard(Rarity.General, "Хоббилары разведчики", "", 2, 1, 0, skills));
        }
        for (int i = 0; i < 3; i++)
        {
            skills = new Skills();
            skills.agility = true;
            cards.Add(new SquadCard(Rarity.Rare, "Лоулендские стрелки", "", 3, 2, 1, skills));
        }
        for (int i = 0; i < 2; i++)
        {
            instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
            instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Shelling, 1));
            instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Stun, 1));
            skills = new Skills(instantSkills);
            cards.Add(new SquadCard(Rarity.Rare, "Чародей клана", "", 2, 1, 1, skills));
        }
        for (int i = 0; i < 2; i++)
        {
            skills = new Skills();
            skills.inspiration = 1;
            cards.Add(new SquadCard(Rarity.Rare, "Волынщик", "", 2, 2, 1, skills));
        }
        skills = new Skills();
        skills.armor = 1;
        skills.pierce = true;
        cards.Add(new SquadCard(Rarity.Epic, "Королевская пограничная стража", "", 2, 2, 3, skills));

        instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
        instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Shelling, 2));
        skills = new Skills(instantSkills);
        skills.pierce = true;
        cards.Add(new SquadCard(Rarity.Epic, "Шотландские фузилеры", "", 2, 3, 2, skills));

        skills = new Skills();
        skills.agility = true;
        skills.inspiration = 1;
        cards.Add(new SquadCard(Rarity.Epic, "Галогласы", "", 3, 2, 1, skills));

        instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
        instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Shelling, 2));
        skills = new Skills(instantSkills);
        skills.agility = true;
        skills.massDamage = 1;
        cards.Add(new SquadCard(Rarity.Epic, "Боевой маг", "", 3, 1, 0, skills));

        instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
        instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Stun, 2));
        skills = new Skills(instantSkills);
        cards.Add(new SquadCard(Rarity.Epic, "\"Громогласные\"", "", 3, 2, 0, skills));

        instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
        instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Shelling, 2));
        instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Sapper, 1));
        skills = new Skills(instantSkills);
        cards.Add(new SquadCard(Rarity.Epic, "Говоящий с камнем", "", 2, 2, 3, skills));

        skills = new Skills();
        skills.agility = true;
        skills.pierce = true;
        skills.inspiration = 1;
        cards.Add(new SquadCard(Rarity.Legendary, "Камерунские налётчиик", "", 4, 3, 0, skills));

        instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
        instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Shelling, 1));
        skills = new Skills(instantSkills);
        skills.armor = 2;
        cards.Add(new SquadCard(Rarity.Legendary, "\"Чёрная стража\"", "", 0, 4, 3, skills));

        skills = new Skills();
        skills.armor = 1;
        skills.inspiration = 2;
        cards.Add(new SquadCard(Rarity.Legendary, "Сэр Уоллес", "", 3, 3, 0, skills));

        instantSkills = new List<Tuple<Skills.SkillCallback, int>>();
        instantSkills.Add(new Tuple<Skills.SkillCallback, int>(Skills.Stun, 2));
        skills = new Skills(instantSkills);
        skills.massDamage = 1;
        cards.Add(new SquadCard(Rarity.Legendary, "Верховный чародей", "", 2, 2, 1, skills));

        for (int i = 0; i < 2; i++)
        {
            cards.Add(new FortificationCard(Rarity.General, "Живой терновник", "", this.LivingBlackThornCallback));
        }

        for (int i = 0; i < 2; i++)
        {
            cards.Add(new FortificationCard(Rarity.Rare, "Знамя клана", "", this.ClanBannerCallback));
        }

        for (int i = 0; i < 3; i++)
        {
            cards.Add(new SupportCard(Rarity.General, "Призыв кланов", "", ViewAction.CLANCALL));
        }
        for (int i = 0; i < 3; i++)
        {
            cards.Add(new SupportCard(Rarity.General, "Кислотный ливень", "", ViewAction.ACIDRAIN));
        }
        for (int i = 0; i < 2; i++)
        {
            cards.Add(new SupportCard(Rarity.Rare, "Боевой клич", "", ViewAction.BATTLECRY));
        }
        cards.Add(new SupportCard(Rarity.Epic, "Дрожь земли", "", ViewAction.TREMBLINGEARTH));
        cards.Add(new SupportCard(Rarity.Epic, "Герои легенд", "", ViewAction.HEROESOFLEGENDS));
        return cards;
    }

    private HashSet<CommandorCard> CreateSecondSetOfCommandors()
    {
        Skills skills;
        HashSet<CommandorCard> commandors = new HashSet<CommandorCard>();
        skills = new Skills();
        skills.supportRevenge = true;
        commandors.Add(new CommandorCard(Rarity.General, "Королевский чародей", "", skills, 1));
        skills = new Skills();
        skills.armor = 1;
        commandors.Add(new CommandorCard(Rarity.General, "Стойкий защитник", "", skills, 1));
        skills = new Skills();
        skills.forceRevenge = true;
        commandors.Add(new CommandorCard(Rarity.Epic, "Мастер муштры", "", skills, 1));
        skills = new Skills();
        skills.forceAgility = true;
        commandors.Add(new CommandorCard(Rarity.Legendary, "Старейшина клана", "", skills, 1));
        return commandors;
    }

    public Game()
    {
        this.cards = new Dictionary<CurrentPlayer, List<AbstractCard>>();
        this.attacks = new Dictionary<CurrentPlayer, Attack>();
        this.fields = new Dictionary<CurrentPlayer, Field>();
        this.freeCommandors2 = this.CreateFirstSetOfCommandors();
        this.freeCommandors1 = this.CreateSecondSetOfCommandors();
        Skills skills;
        List<Tuple<Skills.SkillCallback, int>> activeSkills;
        List<Tuple<Skills.SkillCallback, int>> instantSkills;
        foreach (CurrentPlayer player in Enum.GetValues(typeof(CurrentPlayer)))
        {
            attacks[player] = new Attack();
            fields[player] = new Field();
        }
        cards[CurrentPlayer.SECOND] = this.CreateFirstSetOfCards();
        cards[CurrentPlayer.FIRST] = this.CreateSecondSetOfCards();
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

    public FortificationCard GetFortificationCard(CurrentPlayer player, Flank flank)
    {
        return attacks[player].GetAttackerFortificationCard(flank);
    }

    public void ApplyFortificationCard(CurrentPlayer player, FortificationCard card, Flank flank)
    {
        attacks[player].SetAttackerFortificationCard(card, flank);
        attacks[player == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST].SetDeffenderFortificationCard(card, flank);
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
        fields[player].DropCard(attacks[player].GetDeffenderFortificationCard(flank));
        attacks[player].SetDeffenderFortificationCard(null, flank);
    }

    public void DropCardFromHand(CurrentPlayer player, AbstractCard card)
    {
        fields[player].DropCard(card);
    }

    public void AddCardsToFlank(CurrentPlayer player, HashSet<SquadCard> cards, Flank flank)
    {
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
        foreach (Flank flank in Enum.GetValues(typeof(Flank)))
        {
            attacks[player].ApplySkillsAttacker(flank, fields[player].GetCommandor(flank).skills);
            attacks[player].ApplySkillsDeffender(flank, fields[player == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST].GetCommandor(flank).skills);
        }
        fields[player == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST].Attack(attacks[player].getHeadquartesHurt());
        RefreshSquads();
    }

    public void ApplyAttack(CurrentPlayer player)
    {
        attacks[player].ApplyAttack();
        fields[player == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST].ApplyAttack();
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

    public void HitHeadsquater(CurrentPlayer player, int hp)
    {
        fields[player].headquartesStrength -= hp;
    }

}
