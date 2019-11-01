public class Skills
{
    public uint support { get; set; }
    public uint suppression { get; set; }
    public uint armor { get; set; }
    public bool armorPiercing { get; set; }
    public uint shelling { get; set; }
    public uint inspiration { get; set; }
    public uint massDamage { get; set; }
    public uint sapper { get; set; }
    public uint intelligenceService { get; set; }
    public bool breakthrough { get; set; }
    public bool agility { get; set; }
    public uint medic { get; set; }
    public uint block { get; set; }

    public Skills()
    {
        this.support = 0;
        this.suppression = 0;
        this.armor = 0;
        this.armorPiercing = false;
        this.shelling = 0;
        this.inspiration = 0;
        this.massDamage = 0;
        this.sapper = 0;
        this.intelligenceService = 0;
        this.breakthrough = false;
        this.agility = false;
        this.medic = 0;
        this.block = 0;
    }
}
