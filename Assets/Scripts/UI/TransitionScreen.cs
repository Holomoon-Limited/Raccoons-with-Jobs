using System.Collections;
using Holo.Racc.Game;
using UnityEngine;

namespace Holo.Racc.UI
{
    public class TransitionScreen : MonoBehaviour
    {
        [Header("Asset References")] [SerializeField]
        private TransitionHandler transitionHandler;

        [Header("Component References")] [SerializeField]
        private GameObject art;

        private float transitionTime => transitionHandler.TransitionTime;

        private void Start()
        {
            art.SetActive(true);

            StartCoroutine(Co_TransitionTimer());
        }

        // wait then notify transition handler when finished 
        private IEnumerator Co_TransitionTimer()
        {
            yield return new WaitForSeconds(transitionTime);
            transitionHandler.TransitionOver();
            art.SetActive(false);
        }
    }
}
