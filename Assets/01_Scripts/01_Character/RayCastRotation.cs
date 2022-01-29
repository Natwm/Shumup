using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastRotation : MonoBehaviour
{
    [SerializeField] Material mat;
    // Update is called once per frame
    void FixedUpdate()
    {
        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + 90f));
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(0, 1.0f, 0.0f)), out hit, 100f))
        {
            if (hit.transform.gameObject.CompareTag("shield"))
            {
                //Color c = (hit.collider.gameObject != null && hit.collider.gameObject != this.gameObject) ? Color.green : Color.red;
                CharacterBehaviours.instance.NewOrientation(hit.transform.gameObject);
                Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(0, 1.0f, 0.0f)), Color.red);
            }
        }
        //Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(0, 1.0f, 0.0f)) * 1000.0f, Color.yellow);
    }
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
