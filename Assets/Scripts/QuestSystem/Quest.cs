using System.Collections.Generic;

public class Quest {
    public int id;
    public string name;
    public string description;
    public List<QuestNode> nodes;
}

public class QuestNode {
    public string description;
    public int targetItem;
    public int targetAmount;
    public int progress = 0;
}