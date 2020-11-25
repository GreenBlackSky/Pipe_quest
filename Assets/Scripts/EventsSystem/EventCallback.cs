using System;
using System.Collections;
using System.Collections.Generic;

// TODO create common base classes
public class BaseEventCallback {

    public static void Init() {

    }

    public virtual void Call() {
        throw new Exception("not implementerd");
    }
}

    public class SetFlagCallback: BaseEventCallback {} 
    public class UnsetFlagCallback: BaseEventCallback {} 

    public class StartListenerCallback: BaseEventCallback {
        public int intArg;

        public int getIntArg() {
            return intArg;
        }
    } 
    public class StopListenerCallback: BaseEventCallback {
        public int intArg;

        public int getIntArg() {
            return intArg;
        }
    } 

    public class StartQuestCallback: BaseEventCallback {
        public int intArg;

    } 
    public class ProgressQuestCallback: BaseEventCallback {} 

    public class ChangeDialogueInitialNodeCallback: BaseEventCallback {} 
    public class ChangeDialogeConnectionCallback: BaseEventCallback {} 

    public class GetItemCallback: BaseEventCallback {} 
    public class RemoveItemCallback: BaseEventCallback {} 

    public class LoadLevelCallback: BaseEventCallback {} 

