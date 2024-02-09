using System.Collections;
using UnityEngine;

public class SensorScript : MonoBehaviour
{
    public Transform modeloDoSensor;
    private MainThreadDispatcher mainThreadDispatcher;
    private bool isModeloDoSensorEmMovimento = false;
    private bool descendoModeloDoSensor = false;
    private bool subindoModeloDoSensor = false;
    private float velocidadeDescida = 1.0f;
    [SerializeField]
    private float alturaDescida = -0.008f;
    private Vector3 posicaoAntiga;

    [SerializeField]
    private TrackingScript trackingScript;

    private void Start()
    {
        mainThreadDispatcher = MainThreadDispatcher.Instance;
        posicaoAntiga = modeloDoSensor.position;

        trackingScript.PistaoValueChanged += OnPistaoValueChanged;
    }

    private void OnPistaoValueChanged(char pistaoName, string pistaoValue)
    {
        
        if (trackingScript.PistaoC == "01" && !descendoModeloDoSensor)
        {
            mainThreadDispatcher.Enqueue(StartDescidaModeloDoSensor);
        }
        else if (trackingScript.PistaoC == "11" && !subindoModeloDoSensor)
        {
            mainThreadDispatcher.Enqueue(StartSubidaModeloDoSensor);
        }
    }

    [ContextMenu("StartDescidaModeloDoSensor")]
    private void StartDescidaModeloDoSensor()
    {
        if (!isModeloDoSensorEmMovimento && !descendoModeloDoSensor)
        {
            StartCoroutine(MoveModeloDoSensor(posicaoAntiga + Vector3.up * alturaDescida, () => descendoModeloDoSensor = false));
            descendoModeloDoSensor = true;
        }
    }

    [ContextMenu("StartSubidaModeloDoSensor")]
    private void StartSubidaModeloDoSensor()
    {
        if (!isModeloDoSensorEmMovimento && !subindoModeloDoSensor)
        {
            StartCoroutine(MoveModeloDoSensor(posicaoAntiga, () => subindoModeloDoSensor = false));
            subindoModeloDoSensor = true;
        }
    }

    private IEnumerator MoveModeloDoSensor(Vector3 targetPosition, System.Action onComplete)
    {
        isModeloDoSensorEmMovimento = true;

        float elapsedTime = 0f;
        Vector3 startPosition = modeloDoSensor.position;

        while (elapsedTime < 1f)
        {
            modeloDoSensor.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * velocidadeDescida;
            yield return null;
        }

        modeloDoSensor.position = targetPosition;
        isModeloDoSensorEmMovimento = false;
        onComplete?.Invoke();
    }

    private void Update()
    {
        // Lógica para impedir que suba novamente enquanto já está subindo
        if (subindoModeloDoSensor && isModeloDoSensorEmMovimento)
        {
            // Se já está subindo, não permita outra subida
            mainThreadDispatcher.Enqueue(() => subindoModeloDoSensor = false);
        }

        // Lógica para impedir que desça novamente enquanto já está descendo
        if (descendoModeloDoSensor && isModeloDoSensorEmMovimento)
        {
            // Se já está descendo, não permita outra descida
            mainThreadDispatcher.Enqueue(() => descendoModeloDoSensor = false);
        }
    }
}
