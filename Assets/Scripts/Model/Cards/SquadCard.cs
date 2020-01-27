public class SquadCard : AbstractCard
{

    public int attack { get; set; }
    public int stamina { get; set; }
    public int protection { get; set; }
    public Skills skills { get; set; }
    public bool isActive { get; set; }
    public bool isDeaf { get; set; }
    public bool isWeak { get; set; }
    public int addAttack { get; set; }
    public int addStamina { get; set; }
    public int addProtection { get; set; }

    public SquadCard(Rarity rarity, string name, string tag, int attack, int stamina, int protection, Skills skills) : base(rarity, name, tag)
    {
        this.attack = attack;
        this.stamina = stamina;
        this.protection = protection;
        this.skills = skills;
        this.isActive = skills.agility;
        this.isDeaf = false;
        this.isWeak = false;
    }

}
