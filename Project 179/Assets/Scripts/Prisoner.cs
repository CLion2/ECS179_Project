public class Prisoner
{
    public float Health { get; set; }
    public float AttackDmg { get; set; }
    public float AttackSpd { get; set; }
    public float RageMeter { get; set;  }

    public Prisoner()
    {
        this.Health = 150;
        this.AttackDmg = 15;
        this.AttackSpd = 0f;
        this.RageMeter = 0; // should never increase for prisoner just there to show there is a rage meter
    }
}