using UnityEngine;

//use this script to look after mouse in 3D world
public class MouseLook3D : MonoBehaviour
{
	Camera cam;

    void Start()
    {
		cam = Camera.main;
    }

    void Update()
    {
		float enter;
		//take a ray going from camera to our cursor position
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		//create a plane, that goes through transform.position of our rotatable obj
		//and write a point "enter" on the normal vector to this plane (vec.right in my case)
		new Plane(Vector3.right, transform.position).Raycast(ray, out enter);
		//take a point "enter"
		Vector3 mouseInWorld = ray.GetPoint(enter);
		//in what direction does our object look
		Vector3 dir = (mouseInWorld - transform.position);
		//rotate it
		transform.rotation = Quaternion.LookRotation(dir);
    }
}
