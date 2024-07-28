using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [SerializeField]
    Transform placeHolderFlashlight;
    [SerializeField]
    Item flashlightPrefab;

    GameObject flashlight;

    void Start()
    {
        flashlight = Instantiate(flashlightPrefab.gameObject, placeHolderFlashlight.position, placeHolderFlashlight.rotation, placeHolderFlashlight);
        flashlight.transform.localPosition = flashlightPrefab.relativePosition;
        flashlight.transform.localEulerAngles = flashlightPrefab.relativeRotation;
    }
}
