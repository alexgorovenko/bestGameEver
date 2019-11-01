public class AbstractCard
{
    public Rarity rarity { get; set; }
    public string name { get; set; }

    public AbstractCard(Rarity rarity, string name)
    {
        this.rarity = rarity;
        this.name = name;
    }
}
