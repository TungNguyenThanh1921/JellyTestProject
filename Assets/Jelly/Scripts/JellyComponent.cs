using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyComponent : MonoBehaviour
{
    public List<Transform> childrenCubes;

    void Start()
    {
        // Ban đầu tính toán lại scale cho các cube con
        UpdateChildrenScale();
    }

    void UpdateChildrenScale()
    {
        // Số lượng cube con hiện tại
        int count = childrenCubes.Count;

        if (count == 1)
        {
            // Nếu chỉ có 1 thằng con, nó chiếm toàn bộ không gian của cube cha
            childrenCubes[0].localScale = new Vector3(1f, 1f, 1f);  // scale full theo chiều x
        }
        else if (count == 2)
        {
            // Nếu có 2 thằng con, mỗi thằng sẽ chiếm 1 nửa không gian
            foreach (var cube in childrenCubes)
            {
                cube.localScale = new Vector3(0.5f, 1f, 1f);  // scale nửa theo chiều x
            }
        }
        else if (count == 3)
        {
            // Nếu có 3 thằng con, 1 thằng chiếm nửa không gian, 2 thằng kia chiếm phần còn lại
            childrenCubes[0].localScale = new Vector3(0.5f, 1f, 1f);  // thằng đầu chiếm nửa

            childrenCubes[1].localScale = new Vector3(0.5f, 1f, 0.5f);  // 2 thằng sau chia đều nửa còn lại
            childrenCubes[2].localScale = new Vector3(0.5f, 1f, 0.5f);  // 2 thằng sau chia đều nửa còn lại

        }

        // Cập nhật vị trí các cube con để sắp xếp chúng
        UpdateChildrenPositions();
    }

    void UpdateChildrenPositions()
    {
        float startX = -0.5f;  // điểm bắt đầu theo trục x
        float currentX = startX;

        foreach (var cube in childrenCubes)
        {
            float halfWidth = cube.localScale.x / 2f;
            cube.localPosition = new Vector3(currentX + halfWidth, 0f, 0f);
            currentX += cube.localScale.x;
        }
        if (childrenCubes.Count == 3)
        {
            childrenCubes[1].transform.localPosition = new(0.25f, 0, 0.25f);
            childrenCubes[2].transform.localPosition = new(0.25f, 0, -0.25f);
        }
    }

    public void RemoveChild(Transform child)
    {
        // Xoá cube con và cập nhật lại danh sách
        childrenCubes.Remove(child);
        Destroy(child.gameObject);

        // Tính toán lại scale cho các cube con còn lại
        UpdateChildrenScale();
    }
}
