using UnityEngine;

//Use it for trajectory reneder WITHOUT physics processing
public class TrajectotyRenderer : MonoBehaviour
{
	LineRenderer lineRenderer;

    void Start()
    {
		lineRenderer = GetComponent<LineRenderer>();   
    }

    public void ShowTrajectory(Vector3 origin, Vector3 direction)
    {
		//create array, every element is a point of time when our bullet can move
		Vector3[] points = new Vector3[100];
		lineRenderer.positionCount = points.Length;

		for (int i = 0; i < points.Length; i++)
		{
			//we build trajectory for 10 seconds of moving (100 elements of array *= 0.1f == 10 seconds)
			float time = i * 0.1f;
			//body cast at an angle to the horizon
			points[i] = origin + direction * time + Physics.gravity * time * time / 2;
			//if our trajectory under the ground - don't draw it
			if (points[i].y < 0)
			{
				lineRenderer.positionCount = i + 1;	
				break;
			}
		}
		//set linerenderer
		lineRenderer.SetPositions(points);
    }
}
