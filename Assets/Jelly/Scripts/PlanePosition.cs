using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePosition : MonoBehaviour
{
    public bool isHasCube = false;
    public GameObject currentCube = null;
    public void InitPlanePosition(GameObject _obj)
    {
        currentCube = _obj;
        isHasCube = true;
    }
   
}
