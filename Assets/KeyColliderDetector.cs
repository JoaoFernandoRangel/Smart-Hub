using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyColliderDetector : MonoBehaviour
{
    [SerializeField]
    KeyUnlockScript keyUnlockScript;
    [SerializeField]
    string tag;
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "ChaveQuadrada")
        {
            keyUnlockScript.ChangeLockedState(false);
        }
    }

}
