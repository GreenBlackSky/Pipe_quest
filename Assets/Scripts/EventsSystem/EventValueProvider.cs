using System;
using UnityEngine;


public class  BaseEventValueProvider {
    protected static CollectingHero currentHero;
    public string strArg;
    
    public static void Init(CollectingHero hero) {
        currentHero = hero;
    }

    public BaseEventValueProvider() {}

    public virtual int Provide() {
        throw new Exception("not implementerd");
    }

    public void devPing() {
        Debug.Log($"PING VALUE PROVIDER {this.GetType()}");
    }
}

public class ItemsInInventoryProvider : BaseEventValueProvider {

    public override int Provide() {
        if(currentHero.items.ContainsKey(strArg)) {
            return currentHero.items[strArg];
        }
        return 0;
    }
}