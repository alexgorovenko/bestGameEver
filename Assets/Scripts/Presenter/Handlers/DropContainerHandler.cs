using UnityEngine;
using UnityEngine.EventSystems;

public class DropContainerHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    GameObject drop;
    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().ShowDrop(drop.GetComponent<ContainerDrop>());
        }
    }
}