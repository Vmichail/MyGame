using UnityEngine;

public static class SaveSystem
{
    private const string SAVE_KEY = "SAVE_DATA";

    private static SaveData cachedData;

    public static SaveData Data
    {
        get
        {
            if (cachedData == null)
                LoadInternal();

            return cachedData;
        }
    }

    private static void LoadInternal()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
        {
            cachedData = new SaveData();
            return;
        }

        string json = PlayerPrefs.GetString(SAVE_KEY);
        cachedData = JsonUtility.FromJson<SaveData>(json);
    }

    public static void Save()
    {
        string json = JsonUtility.ToJson(cachedData);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public static void Clear()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        cachedData = new SaveData();
    }
}