
public enum QuestType { FETCH }; 

[System.Serializable]
public class Quest
{
    public string name;
    public string description;

    public  QuestType type;
    public int targetAmount;
}
