using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [SerializeField] float sensitivityY;
    
    Vector2 turn;
        
    // Update is called once per frame
    void Update()
    {
        turn.y += Input.GetAxis("Mouse Y") * sensitivityY;
        transform.localRotation = Quaternion.Euler(-turn.y, 0, 0);

        if (turn.y >= 8)
        {
            turn.y = 8;
        }

        if (turn.y <= -25)
        {
            turn.y = -25;
        }        
        
    }
}
