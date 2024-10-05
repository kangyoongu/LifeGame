using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayData/PlayData", order = 0)]
public class PlayData : ScriptableObject
{
    public Dificult dificult;
    public Tinfo Tdificult;
    public Thinfo Thdificult;
    public Finfo Fdificult;
    public Fiinfo Fidificult;

    public int talkNum;
}
