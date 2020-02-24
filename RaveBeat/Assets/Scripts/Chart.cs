using UnityEngine;

[CreateAssetMenu(fileName ="New Chart", menuName = "Chart")]
public class Chart : ScriptableObject
{
    public int songID;
    public string songTitle;
    public string songArtist;
    public Sprite songImage;


    public AudioClip file;
    public float bpm;
    public float songOffset;

    public float[] notes;
    public int[] noteID;


}
