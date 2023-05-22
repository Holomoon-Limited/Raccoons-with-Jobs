using System.Collections;
using System.Collections.Generic;
using Holo.Racc.Game;
using UnityEngine;

namespace Holo.Racc.UI
{
    public class GameEndCanvasCheck : MonoBehaviour
    {
        [SerializeField] private GameObject[] canvases;
        [SerializeField] private ScoreManager scoreManager;
        
        void Start()
        {
            for (int i = 0; i < canvases.Length; i++)
            {
                canvases[i].SetActive(false);
            }

            if (scoreManager.PlayerScore > scoreManager.EnemyScore)
            {
                canvases[0].SetActive(true);
            }

            else
            {
                canvases[1].SetActive(true);
            }
        }
    }
}
