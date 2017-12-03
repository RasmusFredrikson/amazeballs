using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedToRollAudio : MonoBehaviour {

    public GameObject thunkGO;
    public GameObject boardGO;
    private AudioSource thunkAS;
    private AudioSource source;
    private float lowPitchRange = 0.85f;//Based on observations
    private float highPitchRange = 1f;
    private float thunkLowPitchRange = 0.9f;
    private float thunkHighPitchRange = 1.1f;

    private float speedMin = 0.8f;//Based on observations
    private float speedMax = 3.7f;
    private float previousSpeed = 0f;
    private float minVolume = 0f;
    private float maxVolume = 0.3f;
    private float volume;

	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {

        source = GetComponent<AudioSource>();
        thunkAS = thunkGO.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        Vector3 vel = GetComponent<Rigidbody>().velocity;      //to get a Vector3 representation of the velocity
        float speed = vel.magnitude;
        float soundSpeed = Mathf.Clamp(speed, speedMin, speedMax);
        float pitch = Remap(soundSpeed, speedMin, speedMax, lowPitchRange, highPitchRange);
        volume = Remap(soundSpeed, speedMin, speedMax, minVolume, maxVolume);
        if (!Physics.Raycast(transform.position, -boardGO.transform.up, 0.51f))
            volume = 0;
        source.volume = volume;
        source.pitch = pitch;

        if (previousSpeed - speed > 0.5f)
        {
            thunkAS.volume = Remap(previousSpeed - speed, 0, 3, 0, 1);
            thunkAS.pitch = Random.Range(thunkLowPitchRange, thunkHighPitchRange);
            Debug.Log("stop");
            thunkAS.Play();
        }

        previousSpeed = speed;
    }

    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
