using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SpawnController : MonoBehaviour
{
    private Camera mainCamera;
    public float spacingPlane = 0.1f;
    public int rows = 3;
    public int columns = 4;
    public static SpawnController instance;
    public GameObject planesPref;
    public GameObject[] spawnPosition = { null, null };
    public List<GameObject> cubePrefs;
    public List<PlaneInfomation> planes;
    public EGridType currentGridType = EGridType.none;
    public GameObject currentPlane;
    public GameObject[] cubeSlot = { null, null };
    public UnityAction<GameObject> isCubeAttachPlane;
    public GameObject isConfigObj = null;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        SetLevel(EGridType.Plane3x4);
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetLevel(EGridType type)
    {
        currentGridType = type;
    }
    public void RestartLevel()
    {
        GameObject[] cubeTemp = GameObject.FindGameObjectsWithTag("Cube");
        foreach (GameObject a in cubeTemp)
            Destroy(a);
        Init();
    }
    public void RemoveCubeSlot(GameObject obj)
    {
        if (cubeSlot[0].name.Equals(obj.name))
        {
            cubeSlot[0] = null;
            cubeSlot[0] = Instantiate(cubePrefs[UnityEngine.Random.Range(0, cubePrefs.Count - 1)]);
            cubeSlot[0].AddComponent<BoxCollider>().isTrigger = true;
            cubeSlot[0].transform.position = new Vector3(spawnPosition[0].transform.position.x, 13.8f, spawnPosition[0].transform.position.z);
            ResizeCubeToPlane(spawnPosition[0], cubeSlot[0]);
        }
        else
        {
            cubeSlot[1] = null;
            cubeSlot[1] = Instantiate(cubePrefs[UnityEngine.Random.Range(0, cubePrefs.Count - 1)]);
            cubeSlot[1].AddComponent<BoxCollider>().isTrigger = true;
            cubeSlot[1].transform.position = new Vector3(spawnPosition[1].transform.position.x, 13.8f, spawnPosition[1].transform.position.z);
            ResizeCubeToPlane(spawnPosition[1], cubeSlot[1]);
        }
    }
    public void ReturnSpawnPosition(GameObject obj = null)
    {
        if (isCubeAttachPlane != null)
        {
            isCubeAttachPlane.Invoke(obj);
        }
        else
        {
            if (cubeSlot[0] != null)
            {
                cubeSlot[0].transform.position = new Vector3(spawnPosition[0].transform.position.x, 13.8f, spawnPosition[0].transform.position.z);
                ResizeCubeToPlane(spawnPosition[0], cubeSlot[0]);
            }
            if (cubeSlot[1] != null)
            {
                cubeSlot[1].transform.position = new Vector3(spawnPosition[1].transform.position.x, 13.8f, spawnPosition[1].transform.position.z);
                ResizeCubeToPlane(spawnPosition[1], cubeSlot[1]);
            }

        }
    }
    public void Init()
    {
        // init cube 
        cubeSlot[0] = Instantiate(cubePrefs[UnityEngine.Random.Range(0, cubePrefs.Count - 1)]);
        cubeSlot[0].AddComponent<BoxCollider>().isTrigger = true;
        cubeSlot[1] = Instantiate(cubePrefs[UnityEngine.Random.Range(0, cubePrefs.Count - 1)]);
        cubeSlot[1].AddComponent<BoxCollider>().isTrigger = true;
        InitPlane();
        ReturnSpawnPosition();
    }
    public List<int> GetSurroundingIndexes(int index)
    {
        List<int> surroundingIndexes = new List<int>();

        // Tính toán hàng và cột của cube hiện tại
        int row = index / columns;
        int col = index % columns;

        // Tính toán các index xung quanh
        int topIndex = (row - 1 >= 0) ? index - columns : -1;        // Phía trên
        int bottomIndex = (row + 1 < rows) ? index + columns : -1;    // Phía dưới
        int leftIndex = (col - 1 >= 0) ? index - 1 : -1;              // Bên trái
        int rightIndex = (col + 1 < columns) ? index + 1 : -1;        // Bên phải

        // Thêm các index vào danh sách
        surroundingIndexes.Add(topIndex);    // Thêm vị trí trên
        surroundingIndexes.Add(bottomIndex); // Thêm vị trí dưới
        surroundingIndexes.Add(leftIndex);   // Thêm vị trí trái
        surroundingIndexes.Add(rightIndex);  // Thêm vị trí phải

        return surroundingIndexes;
    }
    public void CheckNearCube(int index)
    {
        List<int> surroundingIndexes = GetSurroundingIndexes(index);
        if (surroundingIndexes.Count < 0)
            return;
        PlanePosition currentCubeStand = currentPlane.GetComponent<PlaneInfomation>().planes[index].GetComponent<PlanePosition>();
        List<int> sameCube = new List<int>();
        for (int i = 0; i < surroundingIndexes.Count; i++)
        {
            if (surroundingIndexes[i] == -1)
                continue;

            PlanePosition plane = currentPlane.GetComponent<PlaneInfomation>().planes[surroundingIndexes[i]].GetComponent<PlanePosition>();
            if (plane == null || !plane.isHasCube) continue;
            if (currentCubeStand.currentCube.name.Contains(plane.currentCube.name))
                sameCube.Add(surroundingIndexes[i]);
        }
        if (sameCube.Count > 0) sameCube.Add(index);

        for (int i = 0; i < sameCube.Count; i++)
        {
            PlanePosition plane = currentPlane.GetComponent<PlaneInfomation>().planes[sameCube[i]].GetComponent<PlanePosition>();
            plane.RemoveCube();
        }
    }
    private void InitPlane()
    {
        if (currentGridType == EGridType.none && currentPlane.GetComponent<PlaneInfomation>().type == currentGridType) return;
        if (currentPlane != null)
        {
            Destroy(currentPlane);
        }
        switch (currentGridType)
        {
            case EGridType.Plane3x4:
                currentPlane = Instantiate(planes.Find(x => x.type == EGridType.Plane3x4).gameObject, this.transform);
                Init3x4PlaneCube();
                break;
            case EGridType.Plane4x4:
                currentPlane = Instantiate(planes.Find(x => x.type == EGridType.Plane4x4).gameObject, this.transform);
                Init4x4PlaneCube();
                break;
            case EGridType.Plane4x5:
                currentPlane = Instantiate(planes.Find(x => x.type == EGridType.Plane4x5).gameObject, this.transform);
                Init4x5PlaneCube();
                break;
        }
    }
    void Init3x4PlaneCube()
    {
        int totalPlane = 3 * 4;
        columns = 3;
        rows = 4;
        int randomCube = UnityEngine.Random.Range(0, totalPlane / 3);

        PlaneInfomation plane = currentPlane.GetComponent<PlaneInfomation>();
        for (int i = 0; i < randomCube; i++)
        {
            int randomPlane = UnityEngine.Random.Range(0, plane.planes.Length - 1);
            if (!plane.planes[randomPlane].GetComponent<PlanePosition>().isHasCube)
                plane.planes[randomPlane].GetComponent<PlanePosition>().InitPlanePosition(Instantiate(cubePrefs[UnityEngine.Random.Range(0, cubePrefs.Count - 1)]));
        }
    }
    void Init4x4PlaneCube()
    {
        int totalPlane = 4 * 4;
        columns = 4;
        rows = 4;
        int randomCube = UnityEngine.Random.Range(0, totalPlane / 3);

        PlaneInfomation plane = currentPlane.GetComponent<PlaneInfomation>();
        for (int i = 0; i < randomCube; i++)
        {
            int randomPlane = UnityEngine.Random.Range(0, plane.planes.Length - 1);
            if (!plane.planes[randomPlane].GetComponent<PlanePosition>().isHasCube)
                plane.planes[randomPlane].GetComponent<PlanePosition>().InitPlanePosition(Instantiate(cubePrefs[UnityEngine.Random.Range(0, cubePrefs.Count - 1)]));
        }
    }
    void Init4x5PlaneCube()
    {
        int totalPlane = 4 * 5;
        columns = 4;
        rows = 5;
        int randomCube = UnityEngine.Random.Range(0, totalPlane / 3);

        PlaneInfomation plane = currentPlane.GetComponent<PlaneInfomation>();
        for (int i = 0; i < randomCube; i++)
        {
            int randomPlane = UnityEngine.Random.Range(0, plane.planes.Length - 1);
            if (!plane.planes[randomPlane].GetComponent<PlanePosition>().isHasCube)
                plane.planes[randomPlane].GetComponent<PlanePosition>().InitPlanePosition(Instantiate(cubePrefs[UnityEngine.Random.Range(0, cubePrefs.Count - 1)]));
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
