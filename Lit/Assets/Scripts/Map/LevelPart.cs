using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelPart", menuName = "Data/Level Part")]
public class LevelPart : ScriptableObject
{
    public List<Transform> parts;
}
