public class FortificationCard : AbstractCard
{
    public Callback skill { get; set; }

    public FortificationCard(Rarity rarity, string name, Callback skill) : base(rarity, name)
    {
        this.skill = skill;
    }
}
