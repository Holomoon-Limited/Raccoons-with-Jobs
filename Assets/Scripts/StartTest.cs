using System.Collections;
using System.Collections.Generic;
using Holo.Racc.Game;
using UnityEngine;

public class StartTest : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField] private PhaseHandler phaseHandler;

    public void StartGame()
    {
        phaseHandler.StartGame();
    }
        
}
