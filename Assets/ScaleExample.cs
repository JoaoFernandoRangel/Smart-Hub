using UnityEngine;

public class ScaleExample : MonoBehaviour
{
    [SerializeField]
    float inGameValue = 4f;
    [SerializeField]

    float inGameMin = 0f;
    [SerializeField]

    float inGameMax = 5f;
    [SerializeField]

    float realMin = 0f;
    [SerializeField]

    float realMax = 1.23f;
    private float ReverseScale(float inGameValue, float inGameMin, float inGameMax, float realMin, float realMax)
    {
        float realValue = ((inGameValue - inGameMin) / (inGameMax - inGameMin)) * (realMax - realMin) + realMin;
        return realValue;
    }

    private void Update()
    {
    

        float realValue = ReverseScale(inGameValue, inGameMin, inGameMax, realMin, realMax);
        Debug.Log("Real Value: " + realValue);
    }
}