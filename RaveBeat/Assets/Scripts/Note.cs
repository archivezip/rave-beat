using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Note : MonoBehaviour
{
    //Public
    public float beat;
    public int type;
    public Vector3 indicator;

    //Private
    private GameConductor conductor;
    private Vector3 spawnPos;
    private Vector3 endPos;

    public void PlaceNote(GameConductor gameConductor, float noteBeat, float zStart, float zEnd, float xPos, float yPos, int noteType)
    {
        conductor = gameConductor;
        beat = noteBeat;
        type = noteType;

        spawnPos = new Vector3(xPos, yPos, zStart);
        endPos = new Vector3(xPos, yPos, zEnd);

        transform.position = new Vector3(xPos, yPos, zStart);

        switch (type)
        {
            case 0:
                {
                    indicator = new Vector3(-375, -150, 0);
                    break;
                }
            case 1:
                {
                    indicator = new Vector3(375, -150, 0);
                    break;
                }
            case 2:
                {
                    indicator = new Vector3(-375, 200, 0);
                    break;
                }
            case 3:
                {
                    indicator = new Vector3(375, 200, 0);
                    break;
                }
            default:
                {
                    break;
                }
        }

    }

    private void Start()
    {

    }

    void Update()
    {
        float timingGap = beat - conductor.songCurrentPosInBeats;
        float noteInterpolateRatio = (conductor.beatsOnTrack - timingGap) / conductor.beatsOnTrack;

        transform.position = Vector3.Lerp(spawnPos, endPos, noteInterpolateRatio);

    }


}
