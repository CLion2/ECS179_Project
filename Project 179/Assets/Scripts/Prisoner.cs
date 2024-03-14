public class Prisoner
{
    public float Health { get; }
    public float AttackDmg { get; }
    public float AttackSpd { get; }
    public float RageMeter { get; }

    public Prisoner()
    {
        this.Health = 150;
        this.AttackDmg = 15;
        this.AttackSpd = 1.0f;
        this.RageMeter = 0; // should never increase for prisoner just there to show there is a rage meter
    }
}