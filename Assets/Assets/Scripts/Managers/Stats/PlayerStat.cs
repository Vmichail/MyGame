[System.Serializable]
public class PlayerStat
{
    public float baseValue;
    public float permanentUpgrade = 0;
    public float flatBonus;
    public float levelBonus;
    public float multiplier = 1f;

    public float Value => ((baseValue + permanentUpgrade) * multiplier) + flatBonus + levelBonus;

    public PlayerStat(float baseValue)
    {
        this.baseValue = baseValue + permanentUpgrade;
    }

    public PlayerStat(PlayerStat other)
    {
        baseValue = other.baseValue + other.permanentUpgrade;
        flatBonus = other.flatBonus;
        multiplier = other.multiplier;
        levelBonus = other.levelBonus;
    }
}