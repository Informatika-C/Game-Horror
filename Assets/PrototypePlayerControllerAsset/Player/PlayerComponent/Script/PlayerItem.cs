using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [SerializeField]
    Transform placeHolderFlashlight;

    [SerializeField]
    Item flashlight;

    [SerializeField]
    List<Item> items = new();

    public Item GetFlashlight()
    {
        return flashlight;
    }

    public void SetUpFlashlight()
    {
        if(flashlight == null) return;
        flashlight = Instantiate(flashlight, placeHolderFlashlight.position, placeHolderFlashlight.rotation, placeHolderFlashlight);
        flashlight.transform.localPosition = flashlight.relativePosition;
        flashlight.transform.localEulerAngles = flashlight.relativeRotation;
    }

    void Start()
    {
        SetUpFlashlight();
    }
}
