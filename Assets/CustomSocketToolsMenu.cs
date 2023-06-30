using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSocketToolsMenu : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<MenuRingItem>(out MenuRingItem menuItem);

        if (menuItem)
        {

        }
    }
}
