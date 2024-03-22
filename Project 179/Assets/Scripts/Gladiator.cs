public class Gladiator
{
    public float Health { get; set; }
    public float AttackDmg { get; set; }
    public float AttackSpd { get; set; }
    public float RageMeter { get; set; }

    public Gladiator()
    {
        this.Health = 1000.0f; // make this at least 1x higher than prisoner
        this.AttackDmg = 15f; // make damage higher
        this.AttackSpd = 2.0f;
        this.RageMeter = 0f; // rage meter should increase as time or damage happens
    }
}