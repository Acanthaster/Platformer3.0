using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALR_DebugCheckPoints : MonoBehaviour
{

    Transform[] allChildren = new Transform[11];

    public Camera mainCamera;
    public GameObject avatar;

    private int pointer = 1;
    // Start is called before the first frame update
    void Start()
    {
        allChildren = GetComponentsInChildren<Transform>();
    }

    public void NextCheckPoint ()
    {
        if(pointer < 11)
        {

            mainCamera.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            avatar.transform.position = allChildren[pointer + 1].transform.position;
            mainCamera.transform.position = allChildren[pointer + 1].transform.position;
            mainCamera.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            pointer++;
        }
    }

    public void PreviousCheckPoint()
    {
        if (pointer > 1)
        {
            mainCamera.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            avatar.transform.position = allChildren[pointer - 1].transform.position;
            mainCamera.transform.position = allChildren[pointer - 1].transform.position;
            mainCamera.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            pointer--;
        }
    }
}
