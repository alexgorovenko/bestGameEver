public class FortificationCard : AbstractCard
{
    public Callback skill { get; set; }

    public FortificationCard(Rarity rarity, string name, string tag, Callback skill) : base(rarity, name, tag)
    {
        this.skill = skill;
    }
}
