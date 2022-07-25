using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAudioSource : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(GameObject.Find("AudioSource"));
    }
}
