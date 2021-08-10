﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimCheckCollision : MonoBehaviour
{   
    private PlayerController _PlayerController;

    [SerializeField]private LayerMask capsuleMask;
    [SerializeField]private Transform[] capsulePoints;
    [SerializeField]private float capsuleRadius;
    [SerializeField] private Collider[] colliders;
    [SerializeField] private float maxDistanceRay = 5f;
    
    [SerializeField] private float offsetY;
    public Transform target;
    private RaycastHit hit;

    void FixedUpdate()
    {
        colliders = Physics.OverlapCapsule(capsulePoints[0].position, capsulePoints[1].position, capsuleRadius, capsuleMask);
        
        if(colliders != null)
        {
            foreach(Collider c in colliders)
            {
                if(c.gameObject.layer == 10)
                {
                    Vector3 dir = c.transform.position - capsulePoints[0].position;
                    dir.Set(dir.x, dir.y + offsetY, dir.z);
                    Physics.Raycast(capsulePoints[0].position, dir,out hit, maxDistanceRay, capsuleMask);

                    if(hit.collider != null)
                    {
                        Debug.DrawRay(capsulePoints[0].position, dir * maxDistanceRay, Color.red, 0.1f);

                        if(hit.collider.gameObject.layer == 10)
                        {
                            target = hit.transform;
                        }
                        else
                        {
                            target = null;
                        }
                    } 
                }
            }
        }
    }
}
