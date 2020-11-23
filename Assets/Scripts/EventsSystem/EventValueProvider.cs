
public enum ProviderType {
    items_in_inventory,
    player_level,
}

public class EventValueProvider {
    static CollectingHero currentHero;

    public ProviderType type;
    public string val;

    public EventValueProvider() {}

    public static void Init(CollectingHero hero) {
        currentHero = hero;
    }

    public int Provide() {
        switch(type) {
            case ProviderType.items_in_inventory:
            if(currentHero.items.ContainsKey(val)) {
                return currentHero.items[val];
            }
            return 0;
        }
        return 0;
    }
}