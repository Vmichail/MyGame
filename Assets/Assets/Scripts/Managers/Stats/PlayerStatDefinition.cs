public class PlayerStatDefinition
{
    public PlayerStatType type;
    public PlayerStatCategory category;
    public string displayName;
    public bool realStat = true;

    public PlayerStatDefinition(
        PlayerStatType type,
        PlayerStatCategory category,
        string displayName)
    {
        this.type = type;
        this.category = category;
        this.displayName = displayName;
        this.realStat = true;
    }

    public PlayerStatDefinition(
        PlayerStatType type,
        PlayerStatCategory category,
        string displayName,
        bool realStat)
    {
        this.type = type;
        this.category = category;
        this.displayName = displayName;
        this.realStat = realStat;
    }

}
