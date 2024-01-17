using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerGameDataSO", menuName = "Scriptable Object/PlayerGameDataSO")]
public class PlayerGameDataSO : ScriptableObject
{
    [Header("Character Data")]
    public int characterCode;
    public int maxHp;
    public int curHP;
    public int gold;

    [Header("Map and Chamber Data")]
    public int currentMapNumber;

}
