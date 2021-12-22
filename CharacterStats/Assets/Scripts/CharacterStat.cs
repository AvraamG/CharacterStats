using System;
using System.Collections.Generic;

public class CharacterStat
{
	public float BaseValue;
	public float Value
	{
		get
		{
			//No need to calculate the final value if there is no change from the previous time it was calculated. 
			if (isDirty)
			{
				_value = CalculateFinalValue();
			}

			return _value;
		}
	}
	private bool isDirty = true;
	private float _value;
	private readonly List<StatModifier> _statModifiers;



	public CharacterStat(float p_baseValue)
	{
		BaseValue = p_baseValue;
		_statModifiers = new List<StatModifier>();
	}

	public void AddModifier(StatModifier p_statModifier)
	{
		isDirty = true;
		_statModifiers.Add(p_statModifier);

		//By default Sort does not know how I want the things inside it sorted.
		//So I can pass a comparison method to let it know how to sort.
		_statModifiers.Sort(CompareModifierOrder);

	}

	private int CompareModifierOrder(StatModifier p_statModifierA, StatModifier p_statModifierB)
	{

		if (p_statModifierA.OrderOfExecution < p_statModifierB.OrderOfExecution)
		{
			return -1;
		}
		else if (p_statModifierA.OrderOfExecution > p_statModifierB.OrderOfExecution)
		{
			return 1;
		}

		return 0;
	}

	public bool RemoveModifier(StatModifier p_statModifier)
	{
		isDirty = true;
		return _statModifiers.Remove(p_statModifier);
	}

	private float CalculateFinalValue()
	{
		float finalValue = BaseValue;

		foreach (StatModifier statmodifer in _statModifiers)
		{
			switch (statmodifer.ModifierType)
			{
				case StatModType.FlatAmmount:
					finalValue += statmodifer.Value;
					break;
				case StatModType.Percentage:
					finalValue *= 1 + statmodifer.Value;
					break;
				default:
					break;
			}

			//interesting choice to make. The order of bonuses applies depending on the order of the stat modifier in the list.
			//e.g +5 +2 +1 +20% +5 the last +5 wont be getting the 20% bonus. 

		}

		float roundedFinalValue = RoundFloatWithFourDecilams(finalValue);
		return roundedFinalValue;
	}

	private float RoundFloatWithFourDecilams(float p_originalValue)
	{
		const int numberOfDecimals = 4;
		//12.001f != 12f
		return (float)Math.Round(p_originalValue, numberOfDecimals);
	}
}
