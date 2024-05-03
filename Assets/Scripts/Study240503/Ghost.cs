using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 100.0f;
    [SerializeField] float moveSpeed = 5f;
    Vector3 rotateValue;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // window 안에서만 마우스가 동작.
    }

    private void cursorMode() {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }


    void Update()
    {
        cursorMode();
        rotating();
        moving();


    }

    private void rotating()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotateValue.x -= mouseY;
        rotateValue.y += mouseX;

        rotateValue.x = Mathf.Clamp(rotateValue.x, -45f, 45f);

        transform.rotation = Quaternion.Euler(rotateValue);
    }

    /*
    * Wrong CASE
    * transform.position += Vector3.forward * moveSpeed * Time.deltaTime; // global move ... 카메라 방향으로 전진하지 않음.

    * Method 3 : multiply transform.rotation, multiply transform.{direction}, transform.TransformDirection({Direction})
    *transform.position += transform.rotation * Vector3.forward * moveSpeed * Time.deltaTime;   ---> Recommended
    *transform.position += transform.forward * moveSpeed * Time.deltaTime;
    *transform.position += transform.TransformDirection(Vector3.forward) * moveSpeed * Time.deltaTime;
    */

    // mouse 움직이면 방향이 달라짐
    private void moving() {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        if (Input.GetKey(KeyCode.W)) {


            transform.position += transform.TransformDirection(Vector3.forward) * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.position -= transform.TransformDirection(Vector3.forward) * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A)) {
            transform.position -= transform.TransformDirection(Vector3.right) * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.position += transform.TransformDirection(Vector3.right) * moveSpeed * Time.deltaTime;
        }

        // up/down 은 로테이션을 반영하지 말자.
        if (Input.GetKey(KeyCode.C)) {
            transform.position -= Vector3.up * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space)) {
            transform.position += Vector3.up* moveSpeed * Time.deltaTime;
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 0f, 30f), transform.position.z);
    }
}
