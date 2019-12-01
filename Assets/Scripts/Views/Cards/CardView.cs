using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardView : MonoBehaviour
{
  [SerializeField] protected GameObject player;
  protected TextMeshProUGUI _fieldName;
  protected AbstractCard card;
  public void SetCard(AbstractCard card)
  {
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
        break;
      case FortificationCard f:
        // play squad card to flank
        break;
      case SupportCard s:
        // play support card
        break;
      case CommandorCard c:
        // if commandor has skill play it
        break;
      default:
        break;
    }
  }
}
