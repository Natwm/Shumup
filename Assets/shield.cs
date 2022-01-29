using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class shield : MonoBehaviour
{
    [SerializeField] private Vector3 m_DeflectSize;

    [Space]
    [Header("List")]
    [SerializeField] private List<GameObject> perfectHit = new List<GameObject>();
    [SerializeField] private List<GameObject> casualHit = new List<GameObject>();
    public void UseDeflect()
    {
        perfectHit.Clear();
        casualHit.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            RaycastHit[] hit = Physics.BoxCastAll(transform.GetChild(i).transform.position, m_DeflectSize, Vector3.forward, Quaternion.identity, 2, CharacterBehaviours.instance.BulletLayer);

            foreach (var item in hit)
            {
                GameObject bullet = item.collider.gameObject.transform.parent.gameObject;

                bool isPerfect = Vector3.Distance(CharacterBehaviours.instance.transform.position, bullet.transform.position) < CharacterBehaviours.instance.MaxDistanceToPerfect && Vector3.Distance(CharacterBehaviours.instance.transform.position, bullet.transform.position) > CharacterBehaviours.instance.MinDistanceToPerfect;

                if (!casualHit.Contains(bullet) && !perfectHit.Contains(bullet))
                {
                    if (isPerfect)
                        perfectHit.Add(bullet);
                    else
                        casualHit.Add(bullet);
                }
                else if(casualHit.Contains(bullet) && isPerfect)
                {
                    casualHit.Remove(bullet);
                    perfectHit.Add(bullet);
                }
            }
        }
        ApplyDeflect();
    }

    public void ApplyDeflect()
    {
        if(casualHit.Count > 0)
        {
            foreach (var item in casualHit)
            {
                item.transform.parent.gameObject.GetComponent<BulletBehaviours>().LaunchBullet(RayCastRotation.instance.ShootGO.transform.position,false);
            }
        }

        if(perfectHit.Count > 0)
        {
            foreach (var item in perfectHit)
            {
                item.transform.parent.gameObject.GetComponent<BulletBehaviours>().LaunchBullet(RayCastRotation.instance.ShootGO.transform.position,true);
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        /*for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.DrawCube(transform.GetChild(i).transform.position, m_DeflectSize);
        }*/
    }

}
