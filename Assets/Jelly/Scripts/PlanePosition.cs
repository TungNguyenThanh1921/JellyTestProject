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
    UnityAction<GameObject> callback;
    private void Start()
    {
        callback = (_obj) =>
       {
           if (!highLightPlane.activeSelf) return;
           SpawnController.instance.isCubeAttachPlane = null;
           InitPlanePosition(_obj);
           SpawnController.instance.RemoveCubeSlot(_obj);
           SpawnController.instance.CheckNearCube(indexPlane);
       };
    }
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
        if (isHasCube || !other.gameObject.tag.Equals("Cube") || currentCube != null)
            return;
        highLightPlane.gameObject.SetActive(true);
        SpawnController.instance.isCubeAttachPlane = callback;
        SpawnController.instance.isConfigObj = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        if (isHasCube || !other.gameObject.tag.Equals("Cube") || currentCube != null)
            return;
        highLightPlane.gameObject.SetActive(false);
        SpawnController.instance.isCubeAttachPlane = null;
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
