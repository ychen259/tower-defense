using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : singleTon<CameraMovement> {

    public float cameraSpeed;

    /*start position is left top*/
    public float xMax; /*right most movement*/
    public float yMin; /*bottom most movement*/

    private void Update()
    {
        GetInput();    
    }

    /*Move the camera with Mouse or Keyboard*/
    public void GetInput()
    {
        /*
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);
        }*/

        if (Input.GetMouseButton(0))
        {
              /*If the mouse going left, then move the camera right*/
              transform.position -= new Vector3(Input.GetAxis("Mouse X") * Time.deltaTime * cameraSpeed,
                    Input.GetAxis("Mouse Y") * Time.deltaTime * cameraSpeed, 0);
        }

        /*Mathf.Clamp限制
        static function Clamp (value : float, min :float, max : float) : float
        限制value的值在min和max之间， 如果value小于min，返回min。 如果value大于max，返回max，否则返回value*/
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, xMax), Mathf.Clamp(transform.position.y,yMin, 0), -10);
    }

    /*set the limit of the camera*/
    public void SetLimit(Vector3 maxTile)
    {
        Vector3 wp = Camera.main.ViewportToWorldPoint(new Vector3(1, 0));

        xMax = maxTile.x - wp.x;
        yMin = maxTile.y - wp.y;
    }
}
