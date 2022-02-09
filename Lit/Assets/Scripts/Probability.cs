using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Probability<T>
{
    /// <summary>
    /// 
    /// </summary>
    public Dictionary<T, int> probabilityPair = new Dictionary<T, int>();
    /// <summary>
    /// A list of objects to obtain a result based on the defined probability.
    /// </summary>
    public List<T> array = new List<T>();
    /// <summary>
    /// The main probability. Object where the probability percentages are stored.
    /// </summary>
    public AnimationCurve probability;

    /// <summary>
    /// Determine if has found an object based on probaiblity in a search.
    /// </summary>
    bool hasFoundProbableObject = false;

    public int maximumProbabilityRange { get; private set; }

    /// <summary>
    /// Initialize the probability object.
    /// </summary>
    /// <param name="probability">The probability element.</param>
    public Probability(AnimationCurve probability)
    {
        this.probability = probability;
    }
    /// <summary>
    /// Initialize the probable elements according to their positions in the list. This must be called before using any other method in this class.
    /// </summary>
    /// <param name="array">The list of probable elements. The position of the elements in the list is HIGHLY important.</param>
    public void InitDictionary(List<T> array)
    {
        int position = 1;

        for (int i = 0; i < array.Count; i++)
        {
            probabilityPair.Add(array[i], position);
            position++;
        }
        this.array = array;
        SetMaximumProbabilityRange(array);
    }
    
    /// <summary>
    /// The main probability distrubutor.
    /// </summary>
    /// <returns>An element based on the probability.</returns>
    public T ProbabilityGenerator()
    {
        T item = default;
        float random;
        if (maximumProbabilityRange <= 1 && array.Count == 2)
        {
            random = Random.value;
        }
        else
        {
            random = Random.Range(0, maximumProbabilityRange);
        }
        Debug.Log(maximumProbabilityRange);
        Debug.Log($"The random int is: {random}");

        for (int i = 0; i < array.Count; i++)
        {
            var newItem = IsWithinRange(i, random);
            if (hasFoundProbableObject)
            {
                hasFoundProbableObject = false;
                item = newItem;
                return item;
            }
        }
        return item;
    }
    private T IsWithinRange(int currentIteration, float random)
    {
        Debug.Log(GetRange(array[currentIteration]).start);
        Debug.Log(GetRange(array[currentIteration]).length);
        Debug.Log(GetRange(array[currentIteration]).end);

        var range = GetRange(array[currentIteration]);
        if (Within(random, range.start, range.end))
        {
            hasFoundProbableObject = true;
            return array[currentIteration];
        }
        
        return default;
    }
    private bool Within(float value, int min, int max)
    {
        if(value >= min && value < max)
        {
            return true;
        }
        return false;
    }
    private void SetMaximumProbabilityRange(List<T> array)
    {
        int totalRange = 0;
        for (int i = 0; i < array.Count; i++)
        {
            var range = GetRange(array[i]).length;
            totalRange += range;
        }
        maximumProbabilityRange = totalRange;
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
