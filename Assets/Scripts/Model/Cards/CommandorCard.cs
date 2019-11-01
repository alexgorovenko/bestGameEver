public class CommandorCard : AbstractCard
{
    public Skills skills { get; set; }
    public uint period { get; set; }

    public CommandorCard(Rarity rarity, string name, Skills skills, uint period) : base(rarity, name)
    {
        this.skills = skills;
        this.period = period;
    }
}
