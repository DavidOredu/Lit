using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Probability<T>
{
    public Dictionary<T, int> probabilityPair;
    public List<T> array = new List<T>();
    public List<bool> withinRange = new List<bool>();
    public AnimationCurve probability;

    bool hasFound = false;

    public int maximumProbabilityRange { get; private set; }

    public Probability(AnimationCurve probability)
    {
        probabilityPair = new Dictionary<T, int>();
        this.probability = probability;
    }
    public void InitDictionary(List<T> array)
    {
        int position = 1;

        for (int i = 0; i < array.Count; i++)
        {
            probabilityPair.Add(array[i], position);
            withinRange.Add(false);
            position++;
        }
        this.array = array;
        GetMaxProbabilityRange(array);
    }
    //
    // Summary: 
    //      Render.
    public T ProbabilityGenerator()
    {
        T item = default;

        var random = Random.Range(0, maximumProbabilityRange - 1);
        Debug.Log($"The random int is: {random}");

        for (int i = 0; i < array.Count; i++)
        {
            var newItem = IsWithinRange(i, random);
            if (hasFound)
            {
                hasFound = false;
                item = newItem;
                return item;
            }
        }
        return item;
    }
    private T IsWithinRange(int currentIteration, int random)
    {
        Debug.Log(GetRange(array[currentIteration]).start);
        Debug.Log(GetRange(array[currentIteration]).length);
        Debug.Log(GetRange(array[currentIteration]).end);

        var range = GetRange(array[currentIteration]);
        if (Within(random, range.start, range.end))
        {
            hasFound = true;
            return array[currentIteration];
        }
        
        return default;
    }
    private bool Within(int value, int min, int max)
    {
        if(value >= min && value < max)
        {
            return true;
        }
        return false;
    }
    private void GetMaxProbabilityRange(List<T> array)
    {
        float totalRange = 0;
        for (int i = 0; i < array.Count; i++)
        {
            var range = GetRange(array[i]).length;
            totalRange += range;
        }
        maximumProbabilityRange = (int)totalRange;
    }
    public T HighestProbability()
    {
        List<RangeInt> ranges = new List<RangeInt>();
        int highestRange = 0;
        T itemWithHighestRange = default;

        for (int i = 0; i < array.Count; i++)
        {
            var range = GetRange(array[i]);
            ranges.Add(range);
        }
        foreach (var range in ranges)
        {
            if (range.length > highestRange)
            {
                highestRange = range.length;
                var index = ranges.IndexOf(range);
                T item = array[index];
                itemWithHighestRange = item;
            }
        }
        return itemWithHighestRange;
    }
    public RangeInt GetRange(T key)
    {

        var index = array.IndexOf(key);
        var previousIndex = index - 1;
        T previousKey;
        if (previousIndex >= 0)
            previousKey = array[previousIndex];
        else
            previousKey = default;
        int minRange;
        int maxRange;

        maxRange = (int)probability.Evaluate(probabilityPair[key]);

        if (probabilityPair[key] == 1)
            minRange = (int)probability.Evaluate(0);
        else
            minRange = (int)probability.Evaluate(probabilityPair[previousKey]);

        var length = maxRange - minRange;

        RangeInt rangeInt = new RangeInt(minRange, length);
        return rangeInt;
    }
}
