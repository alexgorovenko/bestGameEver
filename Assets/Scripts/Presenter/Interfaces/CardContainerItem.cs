using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardContainerItem
{
    [SerializeField]
    Sprite Image { get; }
    void OnAdd();
    void OnPlay();
    void OnRemove();
    void SetActiveSelectHandler(bool state);
    void SetCard(AbstractCard card);
}
public class CardContainerEventArgs : EventArgs
{
    public ICardContainerItem Item;
    public CardContainerEventArgs(ICardContainerItem item)
    {
        Item = item;
    }
}
