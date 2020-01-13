using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject canvas;
    public Vector3 delta;
    private Vector3 initialPosition;
    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = transform.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        player.GetComponent<PlayerController>().currentDraggableCard = gameObject.GetComponent<Card>();
        float cw = ((RectTransform)canvas.transform).rect.width;
        float ch = ((RectTransform)canvas.transform).rect.height;
        float scale = canvas.transform.localScale.x;
        float pX = (eventData.position.x - 30.0f) / Screen.width;
        float pY = 1.0f - (eventData.position.y - 45.0f) / Screen.height;
        float xT = cw * (pX - 0.5f) * scale;
        float yT = (-ch * (pY - 0.5f) * scale);
        float zT = 0;
        delta = new Vector3(xT, yT, zT);
        transform.position = delta;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = initialPosition;
    }
}
