using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canard : MonoBehaviour
{
    public float vitesse = 1;
    public float vitesseTourne = 10;

    public float MinSoundDelay = 1;
    public float RandomDelayRange = 5;
    private float NextSoundDelay = 0;

    public float playbackSpeed = 0.1f;

    private AudioSource aSource;

    private void computeNextSoundDelay()
    {
        NextSoundDelay = MinSoundDelay + Random.Range(0, RandomDelayRange);
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        aSource = GetComponent<AudioSource>();
        computeNextSoundDelay();
    }

    // Update is called once per frame
    void Update()
    {
        NextSoundDelay -= Time.deltaTime;
        if (NextSoundDelay <= 0)
        {
            aSource.Play();
            aSource.pitch = 1 + Random.Range(0, playbackSpeed * 2) - playbackSpeed;
            computeNextSoundDelay();
        }
        transform.position += transform.forward * Time.deltaTime * vitesse;
        transform.rotation = Quaternion.AngleAxis(vitesseTourne * Time.deltaTime, Vector3.up) * transform.rotation;
    }
}
