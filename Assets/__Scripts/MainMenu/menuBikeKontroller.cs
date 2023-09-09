using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class menuBikeKontroller : MonoBehaviour
{
    [SerializeField] GameObject Bike;
    PathCreator pathCreator;

    [SerializeField] float speed = 5f;
    private float distanceTravelled;

    [SerializeField] float wheelSpinSpeed = 0.5f;
    [SerializeField] GameObject wheel1;
    [SerializeField] GameObject wheel2;

    [SerializeField] GameObject bikeTilter;
    [SerializeField] Vector2 TiltRange;
    [SerializeField] float TiltSpeed = 1f;

   
    private void Start()
    {
        //pathCreator  child 0 get component path creator
        pathCreator = transform.GetChild(0).GetComponent<PathCreator>();
        StartCoroutine(WheelSpinner());
        Application.targetFrameRate = 120;
    }

    


    
    private void Update()
    {
        //distance travelled = distance travelled + speed * time
        distanceTravelled += speed * Time.deltaTime;
        //bike position = path creator path get point at distance travelled
        Bike.transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        //bike rotation = path creator path get rotation at distance travelled
        Bike.transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);

        //ad -90 to bike rotation x and -90 to bike rotation z
        Bike.transform.rotation = Quaternion.Euler(Bike.transform.rotation.eulerAngles.x, Bike.transform.rotation.eulerAngles.y, Bike.transform.rotation.eulerAngles.z - 90);
        
        //bounce bikeTilter rotation z between TiltRange.x and TiltRange.y with speed TiltSpeed
        bikeTilter.transform.rotation = Quaternion.Euler(bikeTilter.transform.rotation.eulerAngles.x, bikeTilter.transform.rotation.eulerAngles.y, Mathf.PingPong(Time.time * TiltSpeed, TiltRange.y - TiltRange.x) + TiltRange.x);

    }

    IEnumerator WheelSpinner()
    {
        while(true)
        {
            //rotate wheels on x axis in which speed is multiplied by time and wheel size
            wheel1.transform.Rotate(speed * Time.deltaTime * wheelSpinSpeed, 0, 0);
            wheel2.transform.Rotate(speed * Time.deltaTime * wheelSpinSpeed, 0, 0);
            yield return null;
            
        }
    }


    
    
}
