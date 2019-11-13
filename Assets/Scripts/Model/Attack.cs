using System;
using System.Collections.Generic;

public class Attack
{
    private Dictionary<Flang, List<SquadCard>> attacker;
    private Dictionary<Flang, List<SquadCard>> deffender;
    private Skills attackerSkills;
    private Skills deffenderSkills;
    private uint[] attack;
    private uint[] protection;
    private uint[] stamina;

    public Attack()
    {
        foreach (Flang flang in Enum.GetValues(typeof(Flang)))
        {
            attacker[flang] = new List<SquadCard>();
            deffender[flang] = new List<SquadCard>();
        }
    }

    public void AddAttacker(SquadCard squad, Flang flang)
    {
        attacker[flang].Add(squad);
    }

    public void AddDeffender(SquadCard squad, Flang flang)
    {
        deffender[flang].Add(squad);
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
                card.stamina -= (int)hp;
            }
        }
    }

    public void hitToDeffender(uint hp, Flang flang, SquadCard card)
    {
        foreach (SquadCard _card in deffender[flang])
        {
            if (card == _card)
            {
                card.stamina -= (int)hp;
            }
        }
    }

    private void applySkills(Skills skills, bool isAttacker, bool inspirated, Flang flang)
    {
        if (isAttacker)
        {
            foreach (SquadCard card in deffender[flang])
            {
                if (card != null) card.protection = Math.Min(1, card.protection - attackerSkills.support);
            }
            
            foreach (SquadCard card in attacker[flang])
            {
                if (inspirated && card.skills != skills)
                {
                    card.attack += card.skills.inspiration;
                    card.protection += card.skills.inspiration;
                }

                if (skills.block > 0)
                {
                    card.skills.armor++;
                }
            }
        }
        else
        {
            foreach (SquadCard card in attacker[flang])
            {
                if (card != null) card.attack = Math.Min(1, card.attack - deffenderSkills.suppression);
            }
            
            foreach (SquadCard card in attacker[flang])
            {
                if (inspirated && card.skills != skills)
                {
                    card.attack += card.skills.inspiration;
                    card.protection += card.skills.inspiration;
                }

                if (skills.block > 0)
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
            applySkills(this.attackerSkills, true, false, flang);
            applySkills(this.deffenderSkills, false, false, flang);

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
                applySkills(card.skills, true, remainInspiration < 0, flang);
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
                applySkills(card.skills, false, remainInspiration < 0, flang);
            }

            for (int i = 0; i < attacker[flang].Count; i++)
            {
                if (deffender[flang][i] != null)
                {
                    attacker[flang][i].stamina -= (int)deffender[flang][i].protection;
                    if (!deffender[flang][i].skills.armorPiercing)
                    {
                        attacker[flang][i].stamina += (int)Math.Min(attacker[flang][i].skills.armor, deffender[flang][i].protection - 1);
                    }

                    attacker[flang][i].stamina -= (int)massDamageDefender;
                    deffender[flang][i].stamina -= (int)attacker[flang][i].attack;
                    if (!attacker[flang][i].skills.armorPiercing)
                    {
                        deffender[flang][i].stamina += (int)Math.Min(deffender[flang][i].skills.armor, attacker[flang][i].protection - 1);
                    }

                    deffender[flang][i].stamina -= (int)massDamageAttacker;
                    if (deffender[flang][i].stamina < 0 && attacker[flang][i].skills.breakthrough)
                    {
                        totalHurt += (uint)-deffender[flang][i].stamina;
                    }
                }
                else
                {
                    totalHurt += attacker[flang][i].attack;
                }
            }
        }
        return totalHurt;
    }
}
