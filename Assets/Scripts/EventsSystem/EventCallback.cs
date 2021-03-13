using System;
using UnityEngine;


public class BaseEventCallback {
    protected static QuestDoingHero questHero;

    public static void Init(QuestDoingHero hero) {
        questHero = hero;
    }

    public virtual void Call() {
        throw new Exception("not implementerd");
    }

    public void devPing() {
        Debug.Log($"PING CALLBACK {this.GetType()}");
    }
}

public class IntEventCallback : BaseEventCallback {
    public int intArg;

    public int getIntArg() {
        return intArg;
    }
}

public class StrEventCallback : BaseEventCallback {
    public string strArg;
}

public class SetFlagCallback: StrEventCallback {
    public override void Call() {
        questHero.SetFlag(strArg);
    }
} 

public class UnsetFlagCallback: StrEventCallback {
    public override void Call() {
        questHero.UnsetFlag(strArg);
    }    
} 

public class StartListenerCallback: IntEventCallback {}
public class StopListenerCallback: IntEventCallback {}

public class StartQuestCallback: IntEventCallback {}
public class ProgressQuestNodeCallback: IntEventCallback {}
public class SwitchQuestNodeCallback: IntEventCallback {}
public class FinishQuestCallback: IntEventCallback {}

public class ChangeDialogueInitialNodeCallback: BaseEventCallback {}
public class ChangeDialogeConnectionCallback: BaseEventCallback {}

public class GetItemCallback: BaseEventCallback {}
public class RemoveItemCallback: BaseEventCallback {}

public class LoadLevelCallback: BaseEventCallback {
    public override void Call() {
        Debug.Log("!");
    }
}
