public class SupportCard : AbstractCard
{
    public Callback skill { get; set; }

    public SupportCard(Rarity rarity, string name, string tag, Callback skill) : base(rarity, name, tag)
    {
        this.skill = skill;
    }
}
