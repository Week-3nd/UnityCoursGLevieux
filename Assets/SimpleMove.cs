using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Vitesse de l'animation
    [SerializeField]
    private Vector3 movePerSec = new Vector3(1, 0, 0);

    // Update is called once per frame
    void Update()
    {
        Transform t = GetComponent<Transform>();
        t.position += movePerSec * Time.deltaTime;
    }
}
