﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
  [SerializeField] GameObject player;
  public Vector3 delta;
  private Vector3 initialPosition;
  public void OnBeginDrag(PointerEventData eventData)
  {
    initialPosition = transform.position;
  }
  public void OnDrag(PointerEventData eventData)
  {
    player.GetComponent<PlayerController>().currentDraggableCard = gameObject;
    // Debug.Log(eventData.position);
    float xT = eventData.position.x / 2100 * 1280 - 640;
    float yT = eventData.position.y / 1170 * 720 - 360;
    float zT = 0;
    delta = new Vector3(xT, yT, zT);
    transform.position = delta;
  }
  public void OnEndDrag(PointerEventData eventData)
  {
    transform.position = initialPosition;
  }
}