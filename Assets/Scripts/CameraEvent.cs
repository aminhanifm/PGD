using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEvent : MonoBehaviour
{

    public float zoomFactor = 1.0f;
    public float zoomSpeed = 5.0f;

    private float originalSize = 0f;

    private Camera thisCamera;

    private Vector2 velocity;

    public float smoothTimeX;
    public float smoothTimeY;

    public GameObject player;

    public bool bounds;

    public Vector3 minCameraPos;
    public Vector3 maxCameraPos;


    // Update is called once per frame

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        thisCamera = GetComponent<Camera>();
        originalSize = thisCamera.orthographicSize;
    }

    void Update()
    {

        float targetSize = originalSize * zoomFactor;
        if (targetSize != thisCamera.orthographicSize)
        {
            thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize,
            targetSize, Time.deltaTime * zoomSpeed);
        }

        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);

        if (bounds)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
                Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
                Mathf.Clamp(transform.position.z, minCameraPos.z, maxCameraPos.z));
        }
    }

    public void SetZoom(float zoomFactor)
    {
        this.zoomFactor = zoomFactor;
    }
}
