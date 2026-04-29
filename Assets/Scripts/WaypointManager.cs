using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [Header("Referenciador de Waypoints")]
    public Transform[] Waypoints;

    [Header("Ajustes de patrulha")]
    public float MoveSpeed;
    public float RotationSpeed;
    public bool IsLooping;
    public bool IsRandom;
    private int CurrentPoint = 0;

    void Update()
    {
        if (Waypoints.Length == 0) return;

            transform.position = Vector3.MoveTowards(transform.position, Waypoints[CurrentPoint].position, MoveSpeed * Time.deltaTime);

            var Direction = Waypoints[CurrentPoint].position - transform.position;
            var TargetRotation = Quaternion.LookRotation(Direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation, RotationSpeed * Time.deltaTime); 

            var Distance = Vector3.Distance(transform.position, Waypoints[CurrentPoint].position);

            if (Distance <= 0.01f)
            {
                if (IsRandom)
                {
                    CurrentPoint = Random.Range (0, Waypoints.Length);
                }
                else
                {
                    CurrentPoint++;

                    if (CurrentPoint >= Waypoints.Length)
                    {
                        if (IsLooping) CurrentPoint = 0;
                        if (!IsLooping) CurrentPoint = Waypoints.Length -1;
                    }      
                }

        }

    }

}
