public enum UnlockKey
{
    None,
    RegisterAccount,
    DifficultyNormal,
    DifficultyHard,
    DifficultyInsane,
}


public interface IUnlockCondition
{
    bool IsUnlocked();
}