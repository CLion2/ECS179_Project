public class Prisoner
{
    public float Health { get; set; }
    public float AttackDmg { get; set; }
    public float AttackSpd { get; set; }
    public float RageMeter { get; set; }

    public Prisoner()
    {
        this.Health = 150f;
        this.AttackDmg = 15f;
        this.AttackSpd = 1.0f;
        this.RageMeter = 0f; // should never increase for prisoner just there to show there is a rage meter
    }
}