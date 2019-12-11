using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class CardView : MonoBehaviour, IPointerClickHandler
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


  public void OnPointerClick(PointerEventData eventData)
  {
    // Debug.Log(card.GetType());
    Game game = player.GetComponent<PlayerController>().game;
    if (eventData.clickCount == 2)
    {
      switch (card)
      {
        case SquadCard s:
          break;
        case FortificationCard f:
          break;
        case SupportCard s:
          game.
          break;
        case CommandorCard c:
          break;
        default:
          break;
      }
    }
  }
}
