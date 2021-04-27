using UnityEngine;

public class MinMaxAttribute : PropertyAttribute
{
    public int MinLimit = 0;
    public int MaxLimit = 1;
    public bool ShowEditRange;
    public bool ShowDebugValues;

    public MinMaxAttribute(int min, int max)
    {
        MinLimit = min;
        MaxLimit = max;
    }
}