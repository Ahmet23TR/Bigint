using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToward : MonoBehaviour
{
    void Update()
    {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("FacingCamera")){
          go.transform.LookAt(transform.position);   
        };
    }
}
