using System.Collections;
using UnityEngine;

public class GarraRotationController : MonoBehaviour
{
    /*
    [SerializeField]
    TrackingScript trackingScript;
    [SerializeField]
    GameObject parteFinalDaGarra;
    [SerializeField]
    Animator animator; // Adicionei uma referência para o Animator

    private void Start()
    {
        trackingScript.GarraValueChanged += TrackingScript_GarraValueChanged;
    }

    private void TrackingScript_GarraValueChanged(string arg1, string arg2, string arg3, string arg4)
    {
        float.TryParse(arg4, out float toma);
        if (toma > 0)
        {
            toma += 90;
            print(toma);
        }
        else if (toma == 0)
        {
            toma = 180;
            print(toma);
        }
        else if (toma < 0)
        {
            toma -= 90;
            print(toma);
        }

        // Desative o IK
        //DesativarIK();
        //MainThreadDispatcher.Instance.Enqueue(() => GirarGarra(toma));

    }*/
    /*
    private void GirarGarra(float toma)
    {
        MainThreadDispatcher.Instance.Enqueue(() => parteFinalDaGarra.transform.Rotate(0, toma, 0));

        // Aguarde um momento (você pode ajustar o tempo conforme necessário)
        //StartCoroutine(ReativarIKAfterDelay(0.5f));
    }

    private void DesativarIK()
    {
        // Desative o IK ou faça qualquer manipulação necessária aqui
        if (animator != null)
        {
            MainThreadDispatcher.Instance.Enqueue(() => animator.enabled = false);
        }
    }

    private IEnumerator ReativarIKAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reative o IK ou faça qualquer manipulação necessária aqui
        if (animator != null)
        {
            MainThreadDispatcher.Instance.Enqueue(() => animator.enabled = true);
        }
    }*/
}
