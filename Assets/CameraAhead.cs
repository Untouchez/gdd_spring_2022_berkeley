using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAhead : MonoBehaviour
{
    public Transform player;
    Transform mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
        transform.rotation = Quaternion.Euler(mainCamera.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
