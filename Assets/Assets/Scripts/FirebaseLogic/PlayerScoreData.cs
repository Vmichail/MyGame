using System;

[Serializable]
public class PlayerScoreData
{
    public string map;
    public string difficulty;
    public int score;
    public string hero;
    public int timeSurvivedSeconds;
    public long timestamp; // when reading, timestamp is a long

    // Required empty constructor for Firebase
    public PlayerScoreData() { }

    public PlayerScoreData(
        string map,
        string difficulty,
        int score,
        string hero,
        int timeSurvivedSeconds,
        long timestamp)
    {
        this.map = map;
        this.difficulty = difficulty;
        this.score = score;
        this.hero = hero;
        this.timeSurvivedSeconds = timeSurvivedSeconds;
        this.timestamp = timestamp;
    }
}