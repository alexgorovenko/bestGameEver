using UnityEngine;

public class AbstractContainer : MonoBehaviour
{
    private List<ICardContainerItem> mCards = new List<ICardContainerItem>();
    public event EventHandler<CardContainerEventArgs> CardAdded;
    public void addCard(ICardContainerItem item)
    {
        mCards.Add(item);
        item.onAdd();

        if (CardAdded != null)
        {
            CardAdded(this, new CardContainerEventArgs(item));
        }
    };
	public void removeCard(ICardContainerItem item)
    {
        mCards.Remove(item);
        item.onRemove();
    };
    public void SetCardHandler(bool isActive)
    {
        foreach(var card in mCards) {
            card.SetActiveSelectHandler(isActive);
        }
    }
}
