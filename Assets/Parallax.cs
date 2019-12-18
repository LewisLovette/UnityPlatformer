using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public Transform[] backgrounds;     //list of layers to move
    private float[] parallaxScale;      //proportion of camera movement
    public float smoothing = 1f;         //smoothness of parallax - needs to be > 0

    private Transform camPos;              //find position of camera
    private Vector3 previousCamPos;     //vec3 gets x,y,z - storing position in previous frame


    //before start, good for reference
    void Awake()
    {
        camPos = Camera.main.transform; //get camera pos
    }

    // Start is called before the first frame update
    void Start()
    {
        //store camPos from previous frame
        previousCamPos = camPos.position;

        //looping through layers to find z pos for each
        parallaxScale = new float[backgrounds.Length];
        
        for(int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScale[i] = backgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //
        for(int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - camPos.position.x) * parallaxScale[i];
            float parallay = (previousCamPos.y - camPos.position.y) * parallaxScale[i];

            //set target x pos -current pos + parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            float backgroundTargetPosY = backgrounds[i].position.y + parallay;

            // creating target pos - is background current pos and target x pos.
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);

            //fade current pos and target with lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing*Time.deltaTime); //lerp means to 'Linearly interpolates between two points'
        }

        previousCamPos = camPos.position;

    }
}
