using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class Skills
{
  public delegate void SkillCallback(int power);
  public List<Tuple<SkillCallback, int>> activeSkills { get; set; }
  public List<Tuple<SkillCallback, int>> instantSkills { get; set; }

  // passive skills
  public bool pierce { get; set; }
  public int armor { get; set; }
  public int inspiration { get; set; }
  public int massDamage { get; set; }
  public bool breakthrough { get; set; }
  public bool agility { get; set; }

  public Skills(
    List<Tuple<SkillCallback, int>> activeSkills,
    List<Tuple<SkillCallback, int>> instantSkills
  )
  {
    this.activeSkills = activeSkills;
    this.instantSkills = instantSkills;

    this.pierce = false;
    this.armor = 0;
    this.inspiration = 0;
    this.massDamage = 0;
    this.breakthrough = false;

    this.agility = false;
  }

  public Skills()
  {
    this.activeSkills = null;
    this.instantSkills = null;

    this.pierce = false;
    this.armor = 0;
    this.inspiration = 0;
    this.massDamage = 0;
    this.breakthrough = false;

    this.agility = false;
  }

  public static void Block(int power)
  {

  }
  public static void Suppression(int power)
  {

  }
  public static void Support(int power)
  {

  }

  public static void Medicine(int power)
  {

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
}
