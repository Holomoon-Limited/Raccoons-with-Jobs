using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Holo.Input
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float raycastRadius;

        private IRaycastable highlightedObject = null;

        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            RaycastHit[] hits = RaycastAllSorted();
            IRaycastable raycastable = null;
            foreach (RaycastHit hit in hits)
            {
                raycastable = hit.collider.GetComponent<IRaycastable>();
                break;
            }
            if (raycastable == null)
            {
                if (highlightedObject != null)
                {
                    highlightedObject.OnEndHover();
                    highlightedObject = null;
                }
                return;
            }
            if (raycastable == highlightedObject) return;
            if (highlightedObject != null)
            {
                highlightedObject.OnEndHover();
                highlightedObject = null;
            }
            highlightedObject = raycastable;
            highlightedObject.OnHover();
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private Ray GetMouseRay()
        {
            return cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        }
    }
}
