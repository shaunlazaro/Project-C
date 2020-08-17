using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSelfDestruct : MonoBehaviour
{

    public float lifespan;
    private float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = lifespan;
    }

    // Update is called once per frame
    void Update()
    {

        if(timeLeft > 0)
            timeLeft-=Time.deltaTime;
        if(timeLeft <= 0)
            Destroy(gameObject);
    }
}
