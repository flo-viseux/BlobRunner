[System.Serializable]
public class LevelData
{
    public LevelData(int id, int score, int maxScore)
    {
        this.id = id;
        this.score = score;
        this.maxScore = score;
    }

    public int id;

    public int score;

    public int maxScore;
    
}
