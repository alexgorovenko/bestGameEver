using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardView : MonoBehaviour
{
  [SerializeField] protected GameObject player;
  protected TextMeshProUGUI _fieldName;
  public AbstractCard card;
  public bool isDraggable = true;
  public void SetCard(AbstractCard card)
  {
    if (card.GetType() == typeof(SupportCard)) isDraggable = false;
    this.card = card;
    _fieldName = GetComponentInChildren<TextMeshProUGUI>();
    _fieldName.SetText(card.name);
  }
  public void Click()
  {
    Debug.Log(card.GetType());
    Component script = player.GetComponent<PlayerController>();
    switch (card)
    {
      case SquadCard s:
        // play squad card to flank
        // script.PlaySquad(s);
        Debug.Log("Played SquadCard");
        break;
      case FortificationCard f:
        // script.ApplyFortificationCard(f);
        break;
      case SupportCard s:
        // play support card
        break;
      case CommandorCard c:
        // if commandor has skill play it
        Debug.Log("Played CommandorCard");
        break;
      default:
        break;
    }
  }
}
