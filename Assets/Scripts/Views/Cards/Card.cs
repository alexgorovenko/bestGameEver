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
  public bool isSelectable = true;
  public bool isDraggable = true;
  public bool isHighlighted = true;
  public Card() : base()
  {
    rarityColors[Rarity.General] = Color.gray;
    rarityColors[Rarity.Rare] = Color.blue;
    rarityColors[Rarity.Epic] = Color.magenta;
    rarityColors[Rarity.Legendary] = Color.yellow;
  }
  public void OnAdd() { }
  public void OnPlay() { }
  public void OnRemove() { }
  public void Highlight(bool state)
  {
    ColorBlock colors = border.GetComponent<Button>().colors;
    if (state)
    {
      colors.normalColor = Color.green;
    }
    else
    {
      colors.normalColor = rarityColors[card.rarity];
    }
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
    script.SelectCard(gameObject);
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
        }
        break;
      case CommandorCard c:
        break;
      default:
        break;
    }
  }
}
