using Mirror;

[System.Serializable]
public class LevelData
{
    public string playerName;

    public LevelData(LevelManager level)
    {
        playerName = MainMenu.playerName;   
    }

   
}
