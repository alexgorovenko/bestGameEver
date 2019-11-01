public class Field
{
    public uint headquartesStrength { get; }
    private CommandorCard commandorLeft { get; set; }
    private CommandorCard commandorRight { get; set; }
    private HashSet<AbstractCard> onHand { get; }
    private HashSet<AbstractCard> reserve { get; }
    private HashSet<AbstractCard> drop { get; }

    public Field()
    {
        this.headquartesStrength = 30;
        this.onHand = new HashSet<>();
        this.reserve = new HashSet<>();
        this.drop = new HashSet<>();
    }

    public void AddToHand(AbstractCard card)
    {
        onHand.Add(card);
    }

    public void MoveToReserve(AbstractCard card)
    {
        if (!onHand.Contains(card)) throw new Exception("No such card on hand");
        if (reserve.Count > 8) throw new Excepetion("No more place on reserve");
        onHand.Remove(card);
        reserve.Add(card);
    }

    public void Drop()
    {
        foreach (AbstractCard card in onHand)
        {
            if (typeof(card) == SquadCard && card.stamina <= 0)
            {
                DropCard(card);
            } 
        }
            
    }

    public void DropCard(AbstractCard card)
    {
        onHand.Remove(card);
        drop.Add(card);
    }
}
