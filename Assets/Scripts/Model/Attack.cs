public class Attack
{
    private HashMap<Field, LinkedList<SquadCard>> attacker;
    private HashMap<Field, LinkedList<SquadCard>> deffender;
    private Skills attackerSkills;
    private Skills deffenderSkills;
    private uint[] attack;
    private uint[] protection;
    private uint[] stamina;

    public Attack()
    {
        foreach (Flang flang in Enum.GetValues(typeof(Flang)))
        {
            attacker.add(flang, new LinkedList<>());
            deffender.add(flang, new LinkedList<>());
        }
    }

    public void AddAttacker(SquadCard squad, Flang flang)
    {
        attacker[flang].add(squad);
    }

    public void AddDeffender(SquadCard squad, Flang flang)
    {
        deffender[flang].add(squad);
    }

    public void ApplySkillsAttacker(Skills skills)
    {
        this.attackerSkills = skills;
    }

    public void ApplySkillsDeffender(Skills skills)
    {
        this.deffenderSkills = skills;
    }

    public void hitToAttacker(uint hp, Flang flang, SquadCard card)
    {
        foreach (SquadCard _card in attacker[flang])
        {
            if (card == _card)
            {
                card.stamina -= hp;
            }
        }
    }

    public void hitToDeffender(uint hp, Flang flang, SquadCard card)
    {
        foreach (SquadCard _card in deffender[flang])
        {
            if (card == _card)
            {
                card.stamina -= hp;
            }
        }
    }

    private void applySkills(Skills skills, bool attacker, bool inspirated, Flang flang)
    {
        if (attacker)
        {
            foreach (SquadCard card in deffender[flang])
            {
                if (card != null) card.protection = Min(1, card.protection - attackerSkills.support);
            }
            
            foreach (SquadCard card in attacker[flang])
            {
                if (inspirated && card.skills != skills)
                {
                    card.attack += card.skills.inspiration;
                    card.protection += card.skills.inspiration;
                }

                if (skills.block)
                {
                    card.skills.armor++;
                }
            }
        }
        else
        {
            foreach (SquadCard card in attacker[flang])
            {
                if (card != null) card.attack = Min(1, card.attack - deffenderSkills.suppression);
            }
            
            foreach (SquadCard card in attacker[flang])
            {
                if (inspirated && card.skills != skills)
                {
                    card.attack += card.skills.inspiration;
                    card.protection += card.skills.inspiration;
                }

                if (skills.block)
                {
                    card.skills.armor++;
                }
            }
        }
    }

    public uint getHeadquartesHurt()
    {
        uint totalHurt = 0;
        foreach (Flang flang in Enum.GetValues(typeof(Flang)))
        {
            apply(this.attackerSkills, true, false, flang);
            apply(this.deffenderSkills, false, false, flang);

            uint remainInspiration = 1;
            uint massDamageAttacker = 0;
            foreach (SquadCard card in attacker[flang])
            {
                if (card.skills.inspiration > 0)
                {
                    remainInspiration -= 1;
                }

                massDamageAttacker += card.skills.massDamage;
            }

            foreach (SquadCard card in attacker[flang])
            {
                apply(card.skills, true, remainInspiration < 0, flang);
            }

            remainInspiration = 1;
            uint massDamageDefender = 0;
            foreach (SquadCard card in deffender[flang])
            {
                if (card.skills.inspiration > 0)
                {
                    remainInspiration -= 1;
                }

                massDamageDefender += card.skills.massDamage;
            }

            foreach (SquadCard card in deffender[flang])
            {
                apply(card.skills, false, remainInspiration < 0, flang);
            }

            for (uint i = 0; i < attacker[flang].Length; i++)
            {
                if (deffender[flang][i] != null)
                {
                    attacker[flang][i].stamina -= deffender[flang][i].protection;
                    if (!deffender[flang][i].skills.armorPiercing)
                    {
                        attacker[flang][i].skills.stamina += Min(attacker[flang][i].skills.armor, deffender[flang][i].protection - 1);
                    }

                    attacker[flang][i].stamina -= massDamageDefender;
                    deffender[flang][i].stamina -= attacker[flang][i].attack;
                    if (!attacker[flang][i].skills.armorPiercing)
                    {
                        deffender[flang][i].skills.stamina += Min(deffender[flang][i].skills.armor, attacker[flang][i].protection - 1);
                    }

                    deffender[flang][i].stamina -= massDamageAttacker;
                    if (deffender[flang][i].stamina < 0 && attacker[flang][i].skills.breakthrough)
                    {
                        totalHurt += -deffender[flang][i].stamina;
                    }
                }
                else
                {
                    totalHurt += attacker[flang][i].attack;
                }
            }
        }
        return totatlHurt;
    }
}
