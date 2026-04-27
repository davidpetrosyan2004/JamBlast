using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action CheckIfShapeCanBePlaced;
    public static Action CheckIfShapeCanBePlacedInBuffer;
    public static Action GameOver;
    public static Action GameWin;
    public static Action ShowMessage;
    public static Action ComboLinesCompleted;
}
