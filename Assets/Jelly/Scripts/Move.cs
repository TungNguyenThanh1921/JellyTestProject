using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float fixedY = 14f;  // Giá trị cố định cho trục Y
    private float distanceFromCamera;  // Khoảng cách tự động từ camera tới đối tượng
    public Vector3 offset = new Vector3(0, 0, 5);  // Offset di chuyển Cube lên trên trục Y
    public Collider cubeCollider;
    private bool isDragging = false;
    void Start()
    {
        // Tính toán khoảng cách từ camera đến vị trí ban đầu của đối tượng Cube
        distanceFromCamera = Vector3.Distance(Camera.main.transform.position, transform.position);
    }

    void Update()
    {
        CheckForTouch();

        if (isDragging)
        {
            MoveCube();
        }

    }

    void CheckForTouch()
    {
        if (Input.touchCount > 0)  // Kiểm tra nếu có ít nhất một ngón tay chạm vào màn hình
        {
            Touch touch = Input.GetTouch(0);  // Lấy thông tin của lần chạm đầu tiên
                    Debug.Log("có touch");

            if (touch.phase == TouchPhase.Began)  // Kiểm tra nếu bắt đầu chạm (Touch bắt đầu)
            {
                // Lấy vị trí chạm trên màn hình
                Vector3 touchPosition = touch.position;

                // Đặt khoảng cách từ camera đến điểm cần chuyển đổi
                touchPosition.z = distanceFromCamera;

                // Chuyển đổi vị trí chạm từ màn hình (screen) sang không gian thế giới (world)
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

                // Kiểm tra nếu vị trí chạm trong không gian thế giới nằm trong Collider của Cube
                if (cubeCollider.bounds.Contains(worldPosition))
                {
                    Debug.Log("có chạm");
                    isDragging = true;  // Bật cờ cho phép di chuyển
                    this.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                }
            }

            // Kiểm tra nếu ngón tay nhấc ra (Touch kết thúc)
            if (touch.phase == TouchPhase.Ended)
            {
                this.transform.localScale = new Vector3(1, 1, 1);
                SpawnController.instance.ReturnSpawnPosition();
                isDragging = false;  // Tắt cờ khi nhấc ngón tay khỏi màn hình
            }
        }
    }


    void MoveCube()
    {
        // // Lấy vị trí chuột trên màn hình
        // Vector3 mousePosition = Input.mousePosition;

        // // Đặt khoảng cách từ camera đến điểm cần chuyển đổi
        // mousePosition.z = distanceFromCamera;

        // // Chuyển đổi vị trí màn hình (screen) sang thế giới (world)
        // Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // // Khóa trục Y lại và chỉ di chuyển trên trục X và Z
        // worldPosition.y = fixedY;

        // // Thêm offset để Cube nằm lên trên con trỏ chuột một chút
        // worldPosition += offset;

        // // Di chuyển đối tượng Cube tới vị trí mới
        // transform.position = worldPosition;

        if (Input.touchCount > 0)  // Kiểm tra nếu có ít nhất một ngón tay trên màn hình
        {
            Touch touch = Input.GetTouch(0);  // Lấy thông tin của lần chạm đầu tiên

            // Lấy vị trí chạm trên màn hình
            Vector3 touchPosition = touch.position;

            // Đặt khoảng cách từ camera đến điểm cần chuyển đổi
            touchPosition.z = distanceFromCamera;
                    Debug.Log("có di chuyển");

            // Chuyển đổi vị trí màn hình (screen) sang thế giới (world)
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

            // Khóa trục Y lại và chỉ di chuyển trên trục X và Z
            worldPosition.y = fixedY;

            // Thêm offset để Cube nằm lên trên con trỏ một chút
            worldPosition += offset;

            // Di chuyển đối tượng Cube tới vị trí mới
            transform.position = worldPosition;
        }
    }
}
