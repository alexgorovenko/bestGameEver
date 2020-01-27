using UnityEngine;
using UnityEngine.EventSystems;

public class DropContainerHandler : MonoBehaviour, IPointerClickHandler
{
    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().ShowDrop(GetComponent<ContainerDrop>());
        }
    }
}