using UnityEngine;

public abstract class AbstractContainer : MonoBehaviour
{
  public abstract void AddCard(ICardContainerItem item);
  public abstract void RemoveCard(ICardContainerItem item);
}
