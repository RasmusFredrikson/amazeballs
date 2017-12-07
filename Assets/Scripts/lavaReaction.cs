using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaReaction : MonoBehaviour
{

    public GameObject ball;
    public GameObject start;
    public GameObject inside;
    Color lerpedColor = Color.white;
    Color inside_color;
    Rigidbody rb;
    Renderer rend;
    Renderer rend2;
    bool dead = false;
    bool reset = false;
    float t = 0;

    // Use this for initialization
    void Start()
    {
        rb = ball.GetComponent<Rigidbody>();
        rend = ball.GetComponent<Renderer>();
        rend2 = inside.GetComponent<Renderer>();
        inside_color = rend2.material.color;

    }

    // Update is called once per frame
    void Update()
    {
        if (dead) this.ballDead();
        if (reset) this.ballReset();
    }

    void OnTriggerEnter(Collider otherObj)
    {
        if (otherObj.gameObject == ball)
        {
            dead = true;
            StartCoroutine(deadTimer());
        }
    }

    void ballDead()
    {
        rb.drag = 6;
        lerpedColor = Color.Lerp(Color.white, Color.red, Mathf.Lerp(0, 1, t));
        rend.material.color = lerpedColor;
        rend2.material.color = lerpedColor;
        if (t < 1) t += 0.5f * Time.deltaTime;
    }

    void ballReset()
    {
        rb.drag = 0;
        Vector3 start_pos = start.transform.position;
        start_pos.y += 3f;
        ball.transform.position = start_pos;
        reset = false;
        t = 0;
    }

    IEnumerator deadTimer()
    {
        print(Time.time);
        yield return new WaitForSeconds(3);
        dead = false;
        reset = true;
        rend.material.color = Color.white;
        rend2.material.color = inside_color;
        print(Time.time);
    }
}