using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewGameLevelsData", menuName = "Data/Game Levels Data")]
public class GameLevelsData : ScriptableObject
{
    public List<Level> levels = new List<Level>();
}
