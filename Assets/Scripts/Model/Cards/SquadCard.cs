public class SquadCard : AbstractCard
{

    public uint attack { get; set; }
    public uint stamina { get; set; }
    public uint protection { get; set; }
    public Skills skills { get; set; }

    public SquadCard(Rarity rarity, string name, uint attack, uint stamina, uint protection, Skills skills) : base(rarity, name)
    {
        this.attack = attack;
        this.stamina = stamina;
        this.protection = protection;
        this.skills = skills;
    }

}
