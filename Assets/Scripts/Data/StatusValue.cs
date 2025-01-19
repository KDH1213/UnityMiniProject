using UnityEngine;

[System.Serializable]
public class StatusValue
{
    [SerializeField]
    private StatType statusInfoType;

    public StatType StatusInfoType { get { return statusInfoType; } }
    [SerializeField]
    private float value;
    public float Value { get { return value; } }

    [SerializeField]
    private float maxValue;
    public float MaxValue { get { return maxValue; } }

    [SerializeField]    
    private float minValue;
    public float MinValue { get { return minValue; } }

    public void ValueCopy(StatusValue statusValue)
    {
        this.maxValue = statusValue.MaxValue;
        this.value = statusValue.Value > this.maxValue ? this.maxValue : statusValue.Value;
    }

    public float AddValue(float addValue)
    {
        this.value += addValue;
        this.value = System.Math.Max(minValue, value);

        return this.value;
    }

    public float AddMaxValue(float addValue, bool useAddValue = true)
    {
        this.maxValue += addValue;

        if(useAddValue)
            AddValue(addValue);

        return this.value;
    }

    public float SetMaxValue(float value)
    {
        maxValue = value;

        return maxValue;
    }

    public void OnValueincrease(StatusValue statusValue)
    {
        OnValueincrease(statusValue.MaxValue);
    }

    public void OnValueincrease(float value)
    {
        this.maxValue += value;
        this.value = this.maxValue;
    }

    public void SetValue(float _value)
    {
        this.value = System.Math.Max(_value, maxValue);
    }

    public StatusValue(StatType statusInfoType, float value = 0f, float maxValue = 0f, float minValue = 0f)
    {
        this.statusInfoType = statusInfoType;
        this.value = value;
        this.maxValue = maxValue;
        this.minValue = minValue;
    }
}
