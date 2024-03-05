public class Gladiator
{
    public float Health { get; }
    public float AttackDmg { get; }
    public float AttackSpd { get; }
    public float RageMeter { get; }

    public Gladiator()
    {
        this.Health = 150; // make this at least 1x higher than prisoner
        this.AttackDmg = 15; // make damage higher
        this.AttackSpd = 1.0f;
        this.RageMeter = 0; // rage meter should increase as time or damage happens
    }
}