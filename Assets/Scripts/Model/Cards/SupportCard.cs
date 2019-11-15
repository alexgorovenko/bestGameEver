public class SupportCard : AbstractCard
{
    public ViewAction action { get; set; }

    public SupportCard(Rarity rarity, string name, string tag, ViewAction action) : base(rarity, name, tag)
    {
        this.action = action;
    }
}
