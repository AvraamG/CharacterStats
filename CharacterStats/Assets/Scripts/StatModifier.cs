public enum StatModType
{
    FlatAmmount,
    Percentage
}

public class StatModifier
{
    public readonly StatModType ModifierType;
    public readonly float Value;

    public StatModifier(StatModType p_modifierType, float p_value)
    {
        ModifierType = p_modifierType;
        Value = p_value;
    }
}
