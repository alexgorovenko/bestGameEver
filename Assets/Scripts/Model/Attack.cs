using System;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    private Dictionary<Flank, List<SquadCard>> attacker;
    private Dictionary<Flank, List<SquadCard>> deffender;
    private Dictionary<Flank, FortificationCard> attackerFortification;
    private Dictionary<Flank, FortificationCard> deffenderFortification;
    private Dictionary<Flank, Skills> attackerSkills;
    private Dictionary<Flank, Skills> deffenderSkills;

    public Attack()
    {
        this.attacker = new Dictionary<Flank, List<SquadCard>>();
        this.deffender = new Dictionary<Flank, List<SquadCard>>();
        this.attackerFortification = new Dictionary<Flank, FortificationCard>();
        this.deffenderFortification = new Dictionary<Flank, FortificationCard>();
        this.attackerSkills = new Dictionary<Flank, Skills>();
        this.deffenderSkills = new Dictionary<Flank, Skills>();
        foreach (Flank flank in Enum.GetValues(typeof(Flank)))
        {
            attacker[flank] = new List<SquadCard>();
            deffender[flank] = new List<SquadCard>();
            attackerFortification[flank] = null;
            deffenderFortification[flank] = null;
        }
    }

    public void SetAttackerFortificationCard(FortificationCard card, Flank flank)
    {
        attackerFortification[flank] = card;
    }

    public void SetDeffenderFortificationCard(FortificationCard card, Flank flank)
    {
        deffenderFortification[flank] = card;
    }

    public FortificationCard GetAttackerFortificationCard(Flank flank)
    {
        return attackerFortification[flank];
    }

    public FortificationCard GetDeffenderFortificationCard(Flank flank)
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
            if (_card == null) continue;
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
            if (card == null) continue;
            if (card == _card)
            {
                card.stamina -= (int)hp;
            }
        }
    }

    private void applyInspiration(Skills skills, bool isAttacker, Flank flank)
    {
        if (isAttacker)
        {
            foreach (SquadCard card in attacker[flank])
            {
                if (card == null || card.isDeaf) continue;
                if (card.skills != skills)
                {
                    card.addAttack += card.skills.inspiration;
                    card.addProtection += card.skills.inspiration;
                    card.addAttack += card.skills.brotherhood ? 1 : 0;
                    card.addProtection += card.skills.brotherhood ? 1 : 0;
                    card.addStamina += card.skills.brotherhood ? 1 : 0;
                }
            }
        }
        else
        {
            foreach (SquadCard card in deffender[flank])
            {
                if (card == null || card.isDeaf) continue;
                if (card.skills != skills)
                {
                    card.addAttack += card.skills.inspiration;
                    card.addProtection += card.skills.inspiration;
                    card.addAttack += card.skills.brotherhood ? 1 : 0;
                    card.addProtection += card.skills.brotherhood ? 1 : 0;
                    card.addStamina += card.skills.brotherhood ? 1 : 0;
                }
            }
        }
    }

    public int getHeadquartesHurt()
    {
        int totalHurt = 0;
        foreach (Flank flank in Enum.GetValues(typeof(Flank)))
        {
            applyInspiration(this.attackerSkills[flank], true, flank);
            applyInspiration(this.deffenderSkills[flank], false, flank);
            if (this.attackerFortification[flank] != null)
            {
                this.attackerFortification[flank].skill(this.deffender[flank], this.attacker[flank], this.deffenderSkills[flank], this.attackerSkills[flank], true);
            }
            if (this.deffenderFortification[flank] != null)
            {
                this.deffenderFortification[flank].skill(this.attacker[flank], this.deffender[flank], this.attackerSkills[flank], this.deffenderSkills[flank], false);
            }

            int massDamageAttacker = 0;
            foreach (SquadCard card in attacker[flank])
            {
                if (card == null || card.isDeaf) continue;
                massDamageAttacker += card.skills.massDamage;
            }

            foreach (SquadCard card in attacker[flank])
            {
                if (card == null || card.isDeaf) continue;
                applyInspiration(card.skills, true, flank);
            }

            foreach (SquadCard card in deffender[flank])
            {
                if (card == null || card.isDeaf) continue;
                applyInspiration(card.skills, false, flank);
            }

            for (int i = 0; i < 4; i++)
            {
                if (attacker[flank][i] == null) continue;
                if (deffender[flank][i] != null)
                {
                    attacker[flank][i].addStamina -= (int)deffender[flank][i].protection + deffender[flank][i].addProtection;
                    if ((!deffender[flank][i].skills.pierce || deffender[flank][i].isDeaf) && !attacker[flank][i].isDeaf && !attacker[flank][i].isWeak)
                    {
                        attacker[flank][i].addStamina += (int)Math.Min(attacker[flank][i].skills.armor + attackerSkills[flank].armor, attacker[flank][i].addStamina - 1);
                    }
                    deffender[flank][i].addStamina -= (int)attacker[flank][i].attack + attacker[flank][i].addAttack;
                    deffender[flank][i].stamina -= (int)massDamageAttacker;
                    if ((!attacker[flank][i].skills.pierce || attacker[flank][i].isDeaf) && !deffender[flank][i].isDeaf && !deffender[flank][i].isWeak)
                    {
                        deffender[flank][i].addStamina += (int)Math.Min(deffender[flank][i].skills.armor + deffenderSkills[flank].armor, deffender[flank][i].addStamina - 1);
                    }
                }
                else
                {
                    totalHurt += attacker[flank][i].attack + attacker[flank][i].addAttack;
                }
            }
            foreach (SquadCard card in this.attacker[flank])
            {
                if (card == null) continue;
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

    public void ApplyAttack()
    {
        foreach (Flank flank in Enum.GetValues(typeof(Flank)))
        {
            foreach (SquadCard card in this.attacker[flank])
            {
                if (card == null) continue;
                card.stamina += card.addStamina;
                card.addAttack = 0;
                card.addProtection = 0;
                card.addStamina = 0;
                card.isWeak = false;
            }
            foreach (SquadCard card in this.deffender[flank])
            {
                if (card == null) continue;
                card.stamina += card.addStamina;
                card.addAttack = 0;
                card.addProtection = 0;
                card.addStamina = 0;
                card.isWeak = false;
            }
        }
    }
}
