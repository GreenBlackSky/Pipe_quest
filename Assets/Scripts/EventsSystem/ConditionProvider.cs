
public enum ConditionProviderType {
    items_in_inventory,
    player_level,
}

public class ConditionProvider {
    public ConditionProviderType type;

    public ConditionProvider() {}

    public static void Init() {

    }

    public int Provide() {
        return 0;
    }
}