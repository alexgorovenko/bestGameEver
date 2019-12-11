using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardContainerItem
{
  string Name { get; }
  Sprite Image { get; }
  void onAdd();
  void onPlay();
  void onRemove();
}

public class CardContainerEventArgs : EventArgs
{
  public ICardContainerItem Item;
  public CardContainerEventArgs(ICardContainerItem item)
  {
    Item = item;
  }
}
