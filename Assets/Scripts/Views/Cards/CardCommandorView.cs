using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardCommandorView : CardView
{
  public void Click()
  {
    player.GetComponent<PlayerController>().SetCommandor(transform, (CommandorCard)card);
  }
}
