using System;
using System.Collections.Generic;

public class Attack
{
    private Dictionary<Flank, List<SquadCard>> attacker;
    private Dictionary<Flank, List<SquadCard>> deffender;
    private Dictionary<Flank, FortificationCard> deffenderFortification;
    private Dictionary<Flank, Skills> attackerSkills;
    private Dictionary<Flank, Skills> deffenderSkills;

    public Attack()
    {
        this.attacker = new Dictionary<Flank, List<SquadCard>>();
        this.deffender = new Dictionary<Flank, List<SquadCard>>();
        this.deffenderFortification = new Dictionary<Flank, FortificationCard>();
        foreach (Flank flank in Enum.GetValues(typeof(Flank)))
        {
            attacker[flank] = new List<SquadCard>();
            deffender[flank] = new List<SquadCard>();
        }
    }

    public void SetFortificationCard(FortificationCard card, Flank flank)
    {
        deffenderFortification[flank] = card;
    }

    public FortificationCard GetFortificationCard(Flank flank)
    {
        return deffenderFortification[flank];
    }

    public void AddAttackers(List<SquadCard> squad, Flank flank)
    {
        attacker[flank] = squad;
    }

    public void AddDeffenders(List<SquadCard> squad, Flank flank)
    {
        deffender[flank] = squad;
    }

    public void ApplySkillsAttacker(Flank flank, Skills skills)
    {
        this.attackerSkills[flank] = skills;
    }

    public void ApplySkillsDeffender(Flank flank, Skills skills)
    {
        this.deffenderSkills[flank] = skills;
    }

    public void hitToAttacker(uint hp, Flank flank, SquadCard card)
    {
        foreach (SquadCard _card in attacker[flank])
        {
            if (card == _card)
            {
                card.stamina -= (int)hp;
            }
        }
    }

    public void hitToDeffender(uint hp, Flank flank, SquadCard card)
    {
        foreach (SquadCard _card in deffender[flank])
        {
            if (card == _card)
            {
                card.stamina -= (int)hp;
            }
        }
    }

    private void applySkills(Skills skills, bool isAttacker, bool inspirated, Flank flank)
    {
        if (isAttacker)
        {
            foreach (SquadCard card in deffender[flank])
            {
                if (card != null) card.protection = Math.Min(1, card.protection - attackerSkills[flank].support);
            }
            
            foreach (SquadCard card in attacker[flank])
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
            foreach (SquadCard card in attacker[flank])
            {
                if (card != null) card.attack = Math.Min(1, card.attack - deffenderSkills[flank].suppression);
            }
            
            foreach (SquadCard card in attacker[flank])
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

    public int getHeadquartesHurt()
    {
        int totalHurt = 0;
        foreach (Flank flank in Enum.GetValues(typeof(Flank)))
        {
            applySkills(this.attackerSkills[flank], true, false, flank);
            applySkills(this.deffenderSkills[flank], false, false, flank);
            if (this.deffenderFortification.ContainsKey(flank))
            {
                this.deffenderFortification[flank].skill(this.attacker[flank], this.deffender[flank]);
            }

            uint remainInspiration = 1;
            int massDamageAttacker = 0;
            foreach (SquadCard card in attacker[flank])
            {
                if (card.skills.inspiration > 0)
                {
                    remainInspiration -= 1;
                }

                massDamageAttacker += card.skills.massDamage;
            }

            foreach (SquadCard card in attacker[flank])
            {
                applySkills(card.skills, true, remainInspiration < 0, flank);
            }

            remainInspiration = 1;
            int massDamageDefender = 0;
            foreach (SquadCard card in deffender[flank])
            {
                if (card.skills.inspiration > 0)
                {
                    remainInspiration -= 1;
                }

                massDamageDefender += card.skills.massDamage;
            }

            foreach (SquadCard card in deffender[flank])
            {
                applySkills(card.skills, false, remainInspiration < 0, flank);
            }

            for (int i = 0; i < attacker[flank].Count; i++)
            {
                if (deffender[flank][i] != null)
                {
                    attacker[flank][i].stamina -= (int)deffender[flank][i].protection;
                    if (!deffender[flank][i].skills.armorPiercing)
                    {
                        attacker[flank][i].stamina += (int)Math.Min(attacker[flank][i].skills.armor, deffender[flank][i].protection - 1);
                    }

                    attacker[flank][i].stamina -= (int)massDamageDefender;
                    deffender[flank][i].stamina -= (int)attacker[flank][i].attack;
                    if (!attacker[flank][i].skills.armorPiercing)
                    {
                        deffender[flank][i].stamina += (int)Math.Min(deffender[flank][i].skills.armor, attacker[flank][i].protection - 1);
                    }

                    deffender[flank][i].stamina -= (int)massDamageAttacker;
                    if (deffender[flank][i].stamina < 0 && attacker[flank][i].skills.breakthrough)
                    {
                        totalHurt += -deffender[flank][i].stamina;
                    }
                }
                else
                {
                    totalHurt += attacker[flank][i].attack;
                }
            }
            foreach (SquadCard card in this.attacker[flank])
            {
                card.active = false;
            }
            foreach (SquadCard card in this.deffender[flank])
            {
                if (card != null)
                {
                    card.active = false;
                }
            }
        }
        return totalHurt;
    }
}
