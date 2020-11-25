using System;

public class  BaseEventValueProvider {
    protected static CollectingHero currentHero;
    public string val;
    
    public static void Init(CollectingHero hero) {
        currentHero = hero;
    }

    public BaseEventValueProvider() {}

    public virtual int Provide() {
        throw new Exception("not implementerd");
    }
}

public class ItemsInInventoryProvider : BaseEventValueProvider {

    public override int Provide() {
        if(currentHero.items.ContainsKey(val)) {
            return currentHero.items[val];
        }
        return 0;
    }
}