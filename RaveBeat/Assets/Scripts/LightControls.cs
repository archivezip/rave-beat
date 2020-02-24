using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControls : MonoBehaviour
{
    public GameConductor conductor;
    public GameObject skyLine;
    public GameObject groundLine;
    public Material lineMaterial;
    public Material lineMaterialPressed;

    


    // Update is called once per frame
    void Update()
    {
        if (conductor.blueGroundInUse || conductor.redGroundInUse)
        { groundLine.GetComponent<MeshRenderer>().material = lineMaterialPressed; }
        else
        { groundLine.GetComponent<MeshRenderer>().material = lineMaterial; }

        if (conductor.blueXInUse || conductor.blueYInUse || conductor.redXInUse || conductor.redYInUse)
        { skyLine.GetComponent<MeshRenderer>().material = lineMaterialPressed; }
        else
        { skyLine.GetComponent<MeshRenderer>().material = lineMaterial; }


    }
}
