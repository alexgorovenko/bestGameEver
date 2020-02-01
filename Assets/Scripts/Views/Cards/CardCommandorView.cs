using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardCommandorView : Card
{
    [SerializeField]
    GameObject selectButton;
    [SerializeField]
    GameObject activeButton;
    public void Select()
    {
        selectButton.SetActive(false);
        activeButton.SetActive(true);
        player.GetComponent<PlayerController>().SetCommandor(transform, (CommandorCard)card);
    }
}
