using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastRotation : MonoBehaviour
{
    [SerializeField] float rotation = 20f;
    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward) * 1000.0f, out hit, Mathf.Infinity);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000.0f, Color.yellow);
        transform.RotateAround(gameObject.transform.position, Vector3.up, rotation * Time.deltaTime);
    }
}
