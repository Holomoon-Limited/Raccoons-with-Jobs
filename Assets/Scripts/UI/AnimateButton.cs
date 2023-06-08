using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimateButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image image;

    private void Start()
    {
        image.material.SetFloat("_DoodleSpeed", 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.material.SetFloat("_DoodleSpeed", 7.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.material.SetFloat("_DoodleSpeed", 0);
    }

    public void OnSelect(BaseEventData eventData)
    {
        image.material.SetFloat("_DoodleSpeed", 7.5f);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        image.material.SetFloat("_DoodleSpeed", 0);
    }
}
