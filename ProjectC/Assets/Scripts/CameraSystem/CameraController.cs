using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private BoxCollider2D cameraBox;
    private Camera cam;
    private Transform player;
    public float yOffset;
    public float smoothTime;
    // Start is called before the first frame update
    void Start()
    {
        cameraBox = gameObject.GetComponent<BoxCollider2D>();
        cam = gameObject.GetComponent<Camera>();
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        FitAspectRatioToBox();
        FollowPlayer();
    }
    void FitAspectRatioToBox()
    {
        cameraBox.size = new Vector2(
            cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane)).x-
            cam.ViewportToWorldPoint(new Vector3(0,1,cam.nearClipPlane)).x,
            cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane)).y-
            cam.ViewportToWorldPoint(new Vector3(1,0,cam.nearClipPlane)).y);
    }

    private Vector3 tempVelocity;
    void FollowPlayer()
    {

        // Tried to make it better, but this was glitchy and moved too slow when near the player.
        /*tempVelocity = gameObject.GetComponent<Rigidbody2D>().velocity;
        Vector3.SmoothDamp(
            transform.position,
            new Vector3(
                Mathf.Clamp(
                    player.position.x, 
                    GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.min.x + cameraBox.size.x/2,
                    GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.max.x - cameraBox.size.x/2),
                Mathf.Clamp(
                    player.position.y+yOffset, 
                    GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.min.y + cameraBox.size.y/2,
                    GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.max.y - cameraBox.size.y/2),
                transform.position.z),
            ref tempVelocity, smoothTime);
        gameObject.GetComponent<Rigidbody2D>().velocity = tempVelocity;
        */

        if (GameObject.Find("Boundary"))
        {
            if(GameObject.Find("Boundary").transform.parent.localScale.x > 20)
            {
                transform.position = new Vector3(
                    Mathf.Clamp(
                        player.position.x, 
                        GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.min.x + cameraBox.size.x/2,
                        GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.max.x - cameraBox.size.x/2),
                    Mathf.Clamp(
                        player.position.y+yOffset, 
                        GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.min.y + cameraBox.size.y/2,
                        GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.max.y - cameraBox.size.y/2),
                    transform.position.z
                );
            }
            else
            {
                transform.position = new Vector3(GameObject.Find("Boundary").transform.position.x, 
                    GameObject.Find("Boundary").transform.position.y,
                    transform.position.z);
            }
        }
    }
}
