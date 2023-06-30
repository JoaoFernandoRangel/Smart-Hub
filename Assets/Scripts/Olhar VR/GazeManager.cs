using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeManager : MonoBehaviour
{
    public Camera viewCamera;
    public GameObject lastGazedUpon;
    public float tempoParaOlhar = 3f;

    float countup;
    private Ray gazeray_;


    // GazePanel panel;
    private void Update()
    {
        CheckGaze();
    }

    private void CheckGaze()
    {
        if (lastGazedUpon)
        {
            lastGazedUpon.SendMessage("NotGazingUpon", SendMessageOptions.DontRequireReceiver);
        }

        Ray gazeRay = new Ray(viewCamera.transform.position, viewCamera.transform.rotation * Vector3.forward);
        gazeray_ = gazeRay;
        RaycastHit hit;


        if (Physics.Raycast(gazeRay, out hit, Mathf.Infinity))
        {
            if(lastGazedUpon == hit.collider.gameObject)
            {
                countup += Time.deltaTime;
            }
            else
            {
                countup = 0;
            }

            StartCoroutine(Olhando(hit));

            //hit.collider.SendMessage("GazingUpon", SendMessageOptions.DontRequireReceiver);

            lastGazedUpon = hit.collider.gameObject;

            //Debug.Log(hit.collider.name);



        }
       
        
        
    }

    IEnumerator Olhando(RaycastHit hit)
    {
        yield return new WaitUntil(() => countup >= tempoParaOlhar);

        hit.collider.SendMessage("GazingUpon", SendMessageOptions.DontRequireReceiver);
    }

    void OnDrawGizmos()
    {
        Update();
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        //Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
        Gizmos.DrawRay(gazeray_);
    }
}
