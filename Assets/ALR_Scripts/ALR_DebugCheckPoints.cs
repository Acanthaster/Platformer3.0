using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALR_DebugCheckPoints : MonoBehaviour
{

    [SerializeField] Transform[] allChildren;

    public Camera mainCamera;
    public GameObject avatar;

    private int pointer = 0;
    // Start is called before the first frame update
    void Start()
    {
        //allChildren = GetComponentsInChildren<Transform>();
    }

    public void NextCheckPoint ()
    {
        if(pointer < 10)
        {

            mainCamera.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            avatar.transform.position = allChildren[pointer + 1].transform.position;
            mainCamera.transform.position = allChildren[pointer + 1].transform.position;
            mainCamera.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            pointer++;

            //Debug.Log("NEXT !" + avatar.transform.position);
        }
    }

    public void PreviousCheckPoint()
    {
        if (pointer > 0)
        {
            mainCamera.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            avatar.transform.position = allChildren[pointer - 1].transform.position;
            mainCamera.transform.position = allChildren[pointer - 1].transform.position;
            mainCamera.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            pointer--;

            //Debug.Log("PREVIOUS !" + avatar.transform.position);
        }
    }
}
