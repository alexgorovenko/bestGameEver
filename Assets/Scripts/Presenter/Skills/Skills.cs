using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class Skills
{
    public delegate void SkillCallback(int power);
    public List<Tuple<SkillCallback, int>> instantSkills { get; set; }

    // passive skills
    public bool pierce { get; set; }
    public int armor { get; set; }
    public int inspiration { get; set; }
    public int massDamage { get; set; }
    public bool agility { get; set; }
    public bool brotherhood { get; set; }
    public bool revenge { get; set; } // для лидера сопротивления
    public bool forceShelling { get; set; } // для координатора
    public bool supportRevenge { get; set; }
    public bool forceRevenge { get; set; }
    public bool forceAgility { get; set; }

    public Skills(
      List<Tuple<SkillCallback, int>> instantSkills
    ) : base()
    {
        this.instantSkills = instantSkills;
    }

    public Skills()
    {
        this.instantSkills = null;

        this.pierce = false;
        this.armor = 0;
        this.inspiration = 0;
        this.massDamage = 0;
        this.agility = false;
        this.brotherhood = false;
        this.revenge = false;
        this.forceShelling = false;
        this.supportRevenge = false;
        this.forceRevenge = false;
        this.forceAgility = false;
    }

    public static void Shelling(int power)
    {

    }
    public static void Scouting(int power)
    {

    }
    public static void Sapper(int power)
    {

    }
    public static void Stun(int power)
    {

    }
}
