using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardCommandorView : Card
{
  [SerializeField] GameObject selectButton;
  [SerializeField] GameObject activeButton;
  uint abilityCounter = 0;
  public void Select()
  {
    player.GetComponent<PlayerController>().SetCommandor(transform, (CommandorCard)card);
    selectButton.SetActive(false);
    activeButton.SetActive(true);
  }
  public void Activate()
  {
    if (abilityCounter % ((CommandorCard)card).period == 0)
    {
      player.GetComponent<PlayerController>().ActivateCommandor(gameObject.GetComponent<CommandorCard>());
    }
  }
  public void UpdateCommandor()
  {
    abilityCounter++;
  }
}
