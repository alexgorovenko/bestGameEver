public class FortificationCard : AbstractCard
{
    public CallbackFortification skill { get; set; }

    public FortificationCard(Rarity rarity, string name, string tag, CallbackFortification skill) : base(rarity, name, tag)
    {
        this.skill = skill;
    }
}
