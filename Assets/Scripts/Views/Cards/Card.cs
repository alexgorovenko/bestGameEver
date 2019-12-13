using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using TMPro;

public class Card : MonoBehaviour, ICardContainerItem
{
  public AbstractCard card;
  [SerializeField] public GameObject player;
  [SerializeField] public Sprite Image { get; }
  protected TextMeshProUGUI _fieldName;
  public bool isSelectable = true;
  public bool isDraggable = true;
  public void OnAdd() { }
  public void OnPlay() { }
  public void OnRemove() { }
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
        script.ResetSelectionCards();
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
