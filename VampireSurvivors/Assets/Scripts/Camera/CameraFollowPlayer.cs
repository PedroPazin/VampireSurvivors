using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;

    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if(player != null)
        {
            transform.position = Vector3.Lerp(this.transform.position, new Vector3(player.transform.position.x, player.transform.position.y, -10), delay * Time.fixedDeltaTime);
        }
    }
}
