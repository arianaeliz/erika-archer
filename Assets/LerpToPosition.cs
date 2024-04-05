using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToPosition : MonoBehaviour
{
    public Transform targetObject;
    public float moveSpeed = 0.01f;
    public float rotateSpeed = 0.01f;


    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, targetObject.position, moveSpeed);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetObject.rotation, rotateSpeed);
    }
}
