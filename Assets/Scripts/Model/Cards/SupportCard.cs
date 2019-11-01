public class SupportCard : AbstractCard
{
    public Callback skill { get; set; }

    public SupportCard(Rarity rarity, string name, Callback skill) : base(rarity, name)
    {
        this.skill = skill;
    }
}
