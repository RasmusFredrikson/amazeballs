using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaReaction : MonoBehaviour
{

    private GameObject ball;
    public GameObject inside;
    Color lerpedColor = Color.white;
    Color inside_color;
    Rigidbody rb;
    Renderer rend;
    Renderer rend2;
    bool dead = false;
    float t = 0;
    float deadtime;

    // Use this for initialization
    void Start()
    {
        ball = GameObject.Find("Ball");
        inside = ball;

        rb = ball.GetComponent<Rigidbody>();
        rend = ball.GetComponent<Renderer>();
        rend2 = inside.GetComponent<Renderer>();
        inside_color = rend2.material.color;

    }

    // Update is called once per frame
    void Update()
    {
        if (dead) this.ballDead();
        if (dead && Time.time - deadtime > 1f)
        {
            dead = false;
            rend.material.color = Color.white;
            rend2.material.color = inside_color;
            this.ballReset();
        }
        
    }

    void OnTriggerEnter(Collider otherObj)
    {
        if (otherObj.gameObject == ball)
        {
            dead = true;
            deadtime = Time.time;
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
        ball.transform.position = new Vector3(0, -100, 0);
        t = 0;
    }

    IEnumerator deadTimer()
    {
        yield return new WaitForSeconds(3);
        
    }
}