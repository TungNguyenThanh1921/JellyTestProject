using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class PlanePosition : MonoBehaviour
{
    public int indexPlane;
    public bool isHasCube = false;
    public GameObject currentCube = null;
    public GameObject highLightPlane;
    public void InitPlanePosition(GameObject _obj)
    {
        currentCube = _obj;
        isHasCube = true;
        ApplyOnPlane();
    }
    void ApplyOnPlane()
    {
        float planeWidth = this.transform.localScale.x * 10;
        float planeHeight = this.transform.localScale.z * 10; // Plane sử dụng trục z cho chiều dài
        // Thay đổi kích thước của Cube sao cho giống kích thước của Plane
        currentCube.transform.localScale = new Vector3(planeWidth, currentCube.transform.localScale.y, planeHeight);
        currentCube.transform.position = new Vector3(this.transform.position.x, 13.8f, this.transform.position.z);
        currentCube.GetComponent<Move>().isMoveCube = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isHasCube && !other.gameObject.tag.Equals("Cube"))
            return;
        Move temp = other.GetComponent<Move>();

        highLightPlane.gameObject.SetActive(true);
        UnityAction<GameObject> callback = (obj) =>
        {
            InitPlanePosition(obj);
            SpawnController.instance.CheckNearCube(indexPlane);
        };
        SpawnController.instance.isCubeAttachPlane = callback;
    }
    private void OnTriggerExit(Collider other)
    {
        highLightPlane.gameObject.SetActive(false);

    }
    public void RemoveCube()
    {
        if (currentCube == null) return;
        Vector3 targetScale = new Vector3(0f, 0f, 0f);
        currentCube.transform.DOScale(targetScale, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(currentCube);
            isHasCube = false;
            highLightPlane.gameObject.SetActive(false);
        });

    }
}
