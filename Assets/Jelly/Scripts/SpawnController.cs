using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private Camera mainCamera;
    public float spacingPlane = 0.1f;
    public int rows = 3;
    public int columns = 4;
    public static SpawnController instance;
    public GameObject planesPref;
    public Material[] cubeMaterials;
    public GameObject[] spawnPosition = { null, null };
    public GameObject cubePref;
    public List<PlaneInfomation> planes;
    public EGridType currentGridType = EGridType.none;
    public PlaneInfomation currentPlane;
    public GameObject PlaneObj;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        ReturnSpawnPosition();
        // Init();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ReturnSpawnPosition()
    {
        cubePref.transform.position = new Vector3(spawnPosition[1].transform.position.x, cubePref.transform.position.y, spawnPosition[1].transform.position.z);
        ResizeCubeToPlane(spawnPosition[1], cubePref);
        Init();
    }
    public void Init()
    {
        currentGridType = EGridType.Plane3x4;
        InitPlane();
    }
    private void InitPlane()
    {
        if (currentGridType == EGridType.none && currentPlane.type == currentGridType) return;
        if (currentPlane != null)
        {
            Destroy(currentPlane);
            currentPlane = null;
        }
        switch (currentGridType)
        {
            case EGridType.Plane3x4:
                currentPlane = planes.Find(x => x.type == EGridType.Plane3x4);
                PlaneObj = Instantiate(currentPlane.gameObject, this.transform);
                break;
            case EGridType.Plane4x4:
                currentPlane = planes.Find(x => x.type == EGridType.Plane4x4);
                PlaneObj = Instantiate(currentPlane.gameObject, this.transform);
                break;
            case EGridType.Plane4x5:
                currentPlane = planes.Find(x => x.type == EGridType.Plane4x5);
                PlaneObj = Instantiate(currentPlane.gameObject, this.transform);
                break;
        }
    }
    void ResizeCubeToPlane(GameObject plane, GameObject cube)
    {
        float planeWidth = plane.transform.localScale.x * 10;
        float planeHeight = plane.transform.localScale.z * 10; // Plane sử dụng trục z cho chiều dài

        // Thay đổi kích thước của Cube sao cho giống kích thước của Plane
        cube.transform.localScale = new Vector3(planeWidth, cube.transform.localScale.y, planeHeight);
    }


}
[Serializable]
public enum EGridType
{
    none,
    Plane3x4,
    Plane4x4,
    Plane4x5,
}
