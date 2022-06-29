using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Brick : MonoBehaviour
{
    public List<AudioClip> brickClips;
    public float timeToWaitBeforeSound = 4f;

    private AudioSource audioSource;
    
    
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other) {
        if(Time.time > timeToWaitBeforeSound) {
            if(other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Brick")) {
                int randomIndex = Random.Range(0, brickClips.Count);
                audioSource.PlayOneShot(brickClips[randomIndex]);
            }
        }
    }
}
