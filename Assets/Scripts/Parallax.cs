using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectParallax : MonoBehaviour
{
    private float length, startPosX, startPosY, startPosZ;
    public GameObject cam;
    public float parallaxEffectX;
    public float parallaxEffectY;

    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        startPosZ = transform.position.z;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float tempX = (cam.transform.position.x * (1 - parallaxEffectX));
        float tempY = (cam.transform.position.y * (1 - parallaxEffectY));
        float distX = (cam.transform.position.x * parallaxEffectX);
        float distY = (cam.transform.position.y * parallaxEffectY);

        transform.position = new Vector3(startPosX + distX, startPosY + distY, startPosZ);

        if (tempX > startPosX + length) startPosX += length;
        else if (tempX < startPosX - length) startPosX -= length;
    }
}
