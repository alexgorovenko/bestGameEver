public abstract class AbstractCard
{
  public Rarity rarity { get; set; }
  public string name { get; set; }
  public string tag { get; set; }
  public bool active { get; set; }
  public AbstractCard(Rarity rarity, string name, string tag)
  {
    this.rarity = rarity;
    this.name = name;
    this.tag = tag;
    this.active = false;
  }
}
