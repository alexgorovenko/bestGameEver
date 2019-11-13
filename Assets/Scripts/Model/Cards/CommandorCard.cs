public class CommandorCard : AbstractCard
{
    public Skills skills { get; set; }
    public uint period { get; set; }

    public CommandorCard(Rarity rarity, string name, string tag, Skills skills, uint period) : base(rarity, name, tag)
    {
        this.skills = skills;
        this.period = period;
    }
}
