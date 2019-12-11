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

  public void OnPointerClick()
  {
    // Debug.Log(card.GetType());
    PlayerController script = player.GetComponent<PlayerController>();
    if (2 == 2)
    {
      switch (card)
      {
        case SquadCard s:
          break;
        case FortificationCard f:
          break;
        case SupportCard s:
          switch (s.action)
          {
            case ViewAction.TACTICAL_MOVE:
              script.SupportTactical();
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
}
