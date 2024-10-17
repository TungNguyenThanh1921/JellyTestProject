using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    public Button restart, next, pre;
    void Start()
    {
        restart.onClick.AddListener(RestartLevel);
        next.onClick.AddListener(NextLevel);
        pre.onClick.AddListener(Prelevel);
    }
    void NextLevel()
    {

        switch (SpawnController.instance.currentGridType)
        {
            case EGridType.none:
                break;
            case EGridType.Plane3x4:
                SpawnController.instance.SetLevel(EGridType.Plane4x4);
                SpawnController.instance.RestartLevel();

                break;
            case EGridType.Plane4x4:
                SpawnController.instance.SetLevel(EGridType.Plane4x5);
                SpawnController.instance.RestartLevel();

                break;
            case EGridType.Plane4x5:
                break;
        }
    }
    void Prelevel()
    {
        switch (SpawnController.instance.currentGridType)
        {
            case EGridType.none:
                break;
            case EGridType.Plane3x4:
                break;
            case EGridType.Plane4x4:
                SpawnController.instance.SetLevel(EGridType.Plane3x4);
                SpawnController.instance.RestartLevel();
                break;
            case EGridType.Plane4x5:
                SpawnController.instance.SetLevel(EGridType.Plane4x4);
                SpawnController.instance.RestartLevel();
                break;
        }
    }
    void RestartLevel()
    {
        SpawnController.instance.RestartLevel();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
