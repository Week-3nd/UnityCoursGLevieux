using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMove : MonoBehaviour
{
    public Transform target;
    public float followdist;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Distance à parcourir
        Vector3 deplacement = target.position - transform.position;
        float dist = deplacement.magnitude;
        float saut = dist - followdist;

        //Déplacement actuel
        transform.position += deplacement.normalized * saut;
    }
}
