

[System.Serializable]
public class Quest {
    public string name;
    public string description;

    public QuestNode head;
}

[System.Serializable]
public class QuestNode {
    public string description;
    public string type;
    public int targetItem;
    public int targetAmount;
    public int progress = 0;    

    // TODO init more nodes (after json)
    // TODO reward
    public QuestNode nextNode;
}