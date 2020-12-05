using System.Collections.Generic;

public class Quest {
    public string id;
    public string name;
    public List<QuestNode> nodes;
}

public class QuestNode {
    public string description;
    public List<QuestNode> childrenNodes;
}

public class CounterQuestNode : QuestNode {
    public string text;
    public int targetAmount;
    public int progress;
}