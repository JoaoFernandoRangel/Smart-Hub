using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class RingMenu : MonoBehaviour
{
    [SerializeField]
    List<GameObject> ringMenuTools = new List<GameObject>();
    [SerializeField]
    List<GameObject> spawnPoints = new List<GameObject>();
    [SerializeField]
    InputDevice targetDevice;
    private void Start()
    {
        foreach (GameObject toolsGameObjects in GameObject.FindGameObjectsWithTag("Tool"))
        {

            ringMenuTools.Add(toolsGameObjects);
        }

        for (int i = 0; i < ringMenuTools.Count; i++)
        {

            GameObject obj = Instantiate(ringMenuTools[i], spawnPoints[i].transform.position, spawnPoints[i].transform.rotation, spawnPoints[i].transform);
            //obj.transform.localScale = new Vector3(1,1,1);
            if (obj.GetComponent<Rigidbody>())
            {
                obj.GetComponent<Rigidbody>().useGravity = false;
            }
            if (!obj.GetComponent<RingMenuItem>())
            {
                obj.AddComponent<RingMenuItem>();
            }
            else
            {
                obj.GetComponent<RingMenuItem>().onMenuRing = true;
            }
        }
    }
    /*
    public void RingMenuController(bool button)
    {

            if (button)
            {
                print("sdasadsdsa");
                this.gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        
    }*/
}
