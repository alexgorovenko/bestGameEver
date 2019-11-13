public class SquadCard : AbstractCard
{

    public uint attack { get; set; }
    public int stamina { get; set; }
    public uint protection { get; set; }
    public Skills skills { get; set; }

    public SquadCard(Rarity rarity, string name, string tag, uint attack, int stamina, uint protection, Skills skills) : base(rarity, name, tag)
    {
        this.attack = attack;
        this.stamina = stamina;
        this.protection = protection;
        this.skills = skills;
    }

}
