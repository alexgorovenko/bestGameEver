using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class Card : MonoBehaviour, ICardContainerItem
{
  static Dictionary<Rarity, Color> rarityColors = new Dictionary<Rarity, Color>();
  public AbstractCard card;
  [SerializeField] public GameObject player;
  [SerializeField] public GameObject border;
  [SerializeField] public Sprite Image { get; }
  protected TextMeshProUGUI _fieldName;
  public bool isSelectable = false;
  public bool isDraggable = true;
  public bool isHighlighted = true;
  public int position = -1;
  public Card() : base()
  {
    rarityColors[Rarity.General] = new Color(0.8f, 0.8f, 0.8f);
    rarityColors[Rarity.Rare] = new Color(24.0f / 255.0f, 97.0f / 255.0f, 242.0f / 255.0f);
    rarityColors[Rarity.Epic] = new Color(97.0f / 255.0f, 24.0f / 255.0f, 242.0f / 255.0f);
    rarityColors[Rarity.Legendary] = new Color(242.0f / 255.0f, 170.0f / 255.0f, 24.0f / 255.0f);
  }
  public void OnAdd() { }
  public void OnPlay() { }
  public void OnRemove() { }
  public void Highlight(bool state)
  {
    Debug.Log("in highlight");
    ColorBlock colors = border.GetComponent<Button>().colors;
    if (state)
    {
      colors.normalColor = new Color(140.0f / 255.0f, 242.0f / 255.0f, 140.0f / 255.0f);
      colors.selectedColor = new Color(140.0f / 255.0f, 242.0f / 255.0f, 140.0f / 255.0f);
    }
    else
    {
      colors.normalColor = rarityColors[card.rarity];
    }
    border.GetComponent<Button>().colors = colors;
  }
  public void SetCard(AbstractCard card)
  {
    if (card.GetType() == typeof(SupportCard)) isDraggable = false;
    this.card = card;
    _fieldName = GetComponentInChildren<TextMeshProUGUI>();
    _fieldName.SetText(card.name);
  }
  public void SetActiveSelectHandler(bool state)
  {

  }
  public void OnPointerClick()
  {
    // Debug.Log(card.GetType());
    PlayerController script = player.GetComponent<PlayerController>();
    if (isSelectable) script.SelectCard(gameObject, position);
    Skills skills;
    switch (card)
    {
      case SquadCard s:
        break;
      case FortificationCard f:
        break;
      case SupportCard s:
        script.DropCardToDrop(this, false);
        switch (s.action)
        {
          case ViewAction.TACTICAL_MOVE:
            script.SupportTactical();
            break;
          case ViewAction.MOBILIZAZATION:
            script.SupportMobilization();
            break;
          case ViewAction.SNIPER:
            skills = new Skills();
            skills.shelling = 3;
            script.SupportSniper(skills);
            break;
          case ViewAction.REAR_RAID:
            script.RearRaid_Start();
            break;
          case ViewAction.FIELD_MEDICINE:
            skills = new Skills();
            skills.medic = 2;
            script.SupportMedic_Start(skills);
            break;
        }
        break;
      case CommandorCard c:
        break;
      default:
        break;
    }
  }
}
