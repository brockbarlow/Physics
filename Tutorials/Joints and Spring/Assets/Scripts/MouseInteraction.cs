﻿using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    GameObject current = null;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ShootRay() != null && ShootRay().GetComponent<MonoParticle>() != null)
            {
                current = ShootRay();
                current.GetComponent<MonoParticle>().anchorPoint = (current.GetComponent<MonoParticle>().anchorPoint == true) ? false : true;
            }
            else
            {
                current = null;
            }
        }
    }

    public void LateUpdate()
    {
        if (Input.GetMouseButton(0) && current != null)
        {
            Vector3 mouse = Input.mousePosition;
            mouse.z = -Camera.main.transform.position.z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouse);
            worldPos.z = current.transform.position.z;
            current.transform.position = worldPos;
        }
    }

    public GameObject ShootRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(ray.origin, ray.direction, out hit);
        if (hit.transform != null)
        {
            return hit.transform.gameObject;
        }
        return null;
    }
}