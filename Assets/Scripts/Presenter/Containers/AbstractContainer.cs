using UnityEngine;

public abstract class AbstractContainer : MonoBehaviour
{
    public abstract void addCard(ICardContainerItem item);
	public abstract void removeCard(ICardContainerItem item);
}
