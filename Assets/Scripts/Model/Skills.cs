public class Skills
{
    public int support { get; set; }
    public int suppression { get; set; }
    public int armor { get; set; }
    public bool armorPiercing { get; set; }
    public int shelling { get; set; }
    public int inspiration { get; set; }
    public int massDamage { get; set; }
    public int sapper { get; set; }
    public int intelligenceService { get; set; }
    public bool breakthrough { get; set; }
    public bool agility { get; set; }
    public int medic { get; set; }
    public int block { get; set; }

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
