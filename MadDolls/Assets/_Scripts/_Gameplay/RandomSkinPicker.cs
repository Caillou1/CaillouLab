using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkinPicker : MonoBehaviour
{
    public GameObject[] AvailableSkins;

    private void Awake() {
        foreach(var go in AvailableSkins) {
            go.SetActive(false);
        }

        AvailableSkins[Random.Range(0, AvailableSkins.Length)].SetActive(true);
    }
}
