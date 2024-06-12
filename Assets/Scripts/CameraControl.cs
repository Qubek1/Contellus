using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform playerTransform;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = Game.Instance.CurrentPlayer.transform;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.mousePosition.x - Screen.width/2;
        float y = Input.mousePosition.y - Screen.height/2;
        transform.position = startPosition + playerTransform.position + x * 0.004f * new Vector3(1,0,-1) + y * 0.004f * new Vector3(1,0,1);
    }
}
