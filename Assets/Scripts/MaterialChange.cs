using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{
    [SerializeField] Material myMat;
    [SerializeField] Texture2D myTexture;

    private void Start() 

    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        myMat.SetTexture("Albedo", myTexture);
    }


}



