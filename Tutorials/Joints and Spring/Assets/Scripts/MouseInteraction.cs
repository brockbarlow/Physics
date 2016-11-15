using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    GameObject current = null; //current is set to null be default

    public void Update()
    {
        if (Input.GetMouseButtonDown(1)) //MouseButtonDown(1) refers to the right mouse button.
        {
            if (ShootRay() != null && ShootRay().GetComponent<MonoParticle>() != null) //as long shootray does not equal null...
            {
                current = ShootRay(); //current will receive shootray value
                current.GetComponent<MonoParticle>().anchorPoint = (current.GetComponent<MonoParticle>().anchorPoint == true) ? false : true; //determines if the particle clicked on becomes an anchor point or not
            }
            else
            {
                current = null;
            }
        }
    }

    public void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) && ShootRay().GetComponent<MonoParticle>() != null) //MouseButtonDown(0) refers to the left mouse button.
        { //if user is pressing right mouse button and shootray does not equal null...
            current = ShootRay(); //current will receive shootray value
        }
        if (Input.GetMouseButton(0) && current != null) //if user is pressing right mouse button and current does not equal null...
        {
            current.GetComponent<MonoParticle>().particle.force = Vector3.zero; //this force equals a new vector3(0,0,0)
            current.GetComponent<MonoParticle>().particle.velocity = Vector3.zero; //this velocity equals a new vector3(0,0,0)

            Vector3 mouse = Input.mousePosition; //mouse equals the mouse position
            mouse.z = -Camera.main.transform.position.z; //mouse z axis equals the negative camera z position value

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouse); //worldPos equals the transforms position from the screen space into world space
            worldPos.z = current.transform.position.z; //worldPos z axis equals the current gameobjects z position value

            current.GetComponent<MonoParticle>().particle.position = worldPos; //current get component particle position equals the worldPos value
            current.transform.position = worldPos; //current transform position equals worldPos value
        }
    }

    public GameObject ShootRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //returns a ray going from camera through a screen point
        RaycastHit hit = new RaycastHit(); //structure used to get information back from a raycast
        Physics.Raycast(ray.origin, ray.direction, out hit); //global physics properties and helper methods
        if (hit.transform != null) //if hit.transform does not equal null
        {
            return hit.transform.gameObject;
        }
        return null;
    }
}