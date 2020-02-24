using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConductor : MonoBehaviour
{
    private Chart chart;
    public GameObject blueNotePrefab;
    public GameObject redNotePrefab;
    public GameObject blueSkyNotePrefab;
    public GameObject redSkyNotePrefab;

    public TMPro.TextMeshProUGUI hitsText;
    public TMPro.TextMeshProUGUI missesText;
    public TMPro.TextMeshProUGUI earliesText;
    public TMPro.TextMeshProUGUI latesText;
    public TMPro.TextMeshProUGUI healthText;
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI comboText;
    public TMPro.TextMeshProUGUI maxComboText;

    public GameObject earlyIndicator;
    public GameObject lateIndicator;
    public GameObject missIndicator;
    public GameObject wrongIndicator;
    public GameObject comboIndicator;
    public GameObject hitIndicator;

    private Queue<Note> notesOnScreen;
    private float songCurrentPosition;
    public float songCurrentPosInBeats;
    private float songBeatDuration;
    private float songElapsed;

    private float[] noteBeats;
    private int[] noteTypes;

    private int nextNoteIndex;

    private float noteStartZ;
    private float noteEndZ;
    private float noteGroundY;
    private float noteSkyY;
    private float noteGroundBlueX;
    private float noteGroundRedY;
    private float noteSkyRedX;
    private float noteSkyBlueX;

    private float maxScore;
    private float hitScore;
    private float nearScore;
    private float playerScore;
    private int playerCombo;
    private int playerComboMax;

    private int hitCount;
    private int earlyCount;
    private int lateCount;
    private int missCount;

    private float maxHP;
    private float missScale;
    private float hitScale;
    private float playerHP;
    private float missHP;
    private float hitHP;

    private bool chartEnded;
    private bool gameEnded;
    private bool audioPaused;

    public float beatsOnTrack;
    public float hitOffset;
    public float nearOffset;

    public bool blueGroundInUse;
    public bool redGroundInUse;
    public bool blueXInUse;
    public bool blueYInUse;
    public bool redXInUse;
    public bool redYInUse;
    


    void Start()
    {
        //

        string filepath = PlayerPrefs.GetString("chart");
        chart = Resources.Load<Chart>("Charts/" + filepath);

        //variable init and setting
        notesOnScreen = new Queue<Note>();

        //note positions
        noteStartZ = 15f;
        noteEndZ = 0.02f;
        noteGroundY = 0.25f;
        noteSkyY = 0.75f;
        noteGroundBlueX = -0.45f;
        noteGroundRedY = 0.45f;
        noteSkyBlueX = -0.4f;
        noteSkyRedX = 0.4f;

        //looping
        chartEnded = false;
        gameEnded = false;
        audioPaused = false;
        nextNoteIndex = 0;

        //input checking
        blueGroundInUse = false;
        redGroundInUse = false;
        blueXInUse = false;
        blueYInUse = false;
        redXInUse = false;
        redYInUse = false;

        //chart data
        noteBeats = chart.notes;
        noteTypes = chart.noteID;
        songBeatDuration = 60f / chart.bpm;
        GetComponent<AudioSource>().clip = chart.file;

        //score
        maxScore = 100000;
        hitScore = maxScore / noteBeats.Length;
        nearScore = hitScore / 2;
        playerScore = 0;
        playerCombo = 0;
        playerComboMax = 0;

        //accuracy
        hitCount = 0;
        missCount = 0;
        earlyCount = 0;
        lateCount = 0;

        //hp
        maxHP = 100;
        missScale = 0.7f;
        hitScale = 0.5f;
        float perHitHP = maxHP / (noteBeats.Length * hitScale);
        float perMissHP = maxHP / (noteBeats.Length * missScale);
        hitHP = perHitHP;
        missHP = -perMissHP;
        playerHP = 0;

        //
        hitsText.text = "0";
        missesText.text = "0";
        earliesText.text = "0";
        latesText.text = "0";
        healthText.text = "0";
        comboText.text = "0";
        maxComboText.text = "0";
        scoreText.text = "0";

        GetComponent<AudioSource>().Play();
        songElapsed = (float)AudioSettings.dspTime;
    }

    void Update()
    {
        //if chart hasnt ended
        if (!chartEnded) 
        {   
            //Chart Loop  -  Chart is in progress - could be paused or unpaused

            //If game has been paused
            if (PauseMenu.gameIsPaused)
            {
                GetComponent<AudioSource>().Pause();
                audioPaused = true;
            }
            //If game has been unpaused, but audio is still paused
            else if (!PauseMenu.gameIsPaused && audioPaused)
            {
                GetComponent<AudioSource>().UnPause();
                songElapsed += PauseMenu.pauseTime;
                audioPaused = false;
            }

            //if game is not paused, and the audio is not paused
            if (!PauseMenu.gameIsPaused && !audioPaused)
            {
                //Game Loop  - Chart is being played - game is unpaused

                //Get the songs current position in time
                songCurrentPosition = (float)((AudioSettings.dspTime - songElapsed) - chart.songOffset);

                //Convert the songs current time into beats
                songCurrentPosInBeats = songCurrentPosition / songBeatDuration;

                //Spawn next note - if there is one and its time in the song has come
                //If there is a next note and it's beat is less than the songs position on the track
                if (nextNoteIndex < noteBeats.Length && noteBeats[nextNoteIndex] < songCurrentPosInBeats + beatsOnTrack)
                {
                    //what note to spawn
                    switch (noteTypes[nextNoteIndex])
                    {
                        case 0:
                            {   //spawn note    - ground blue note 
                                Note note = ((GameObject)Instantiate(blueNotePrefab)).GetComponent<Note>();
                                note.PlaceNote(this, noteBeats[nextNoteIndex], noteStartZ, noteEndZ, noteGroundBlueX, noteGroundY, noteTypes[nextNoteIndex]);
                                notesOnScreen.Enqueue(note);
                                break;
                            }
                        case 1:
                            {   //spawn note    - ground red note
                                Note note = ((GameObject)Instantiate(redNotePrefab)).GetComponent<Note>();
                                note.PlaceNote(this, noteBeats[nextNoteIndex], noteStartZ, noteEndZ, noteGroundRedY, noteGroundY, noteTypes[nextNoteIndex]);
                                notesOnScreen.Enqueue(note);
                                break;
                            }
                        case 2:
                            {   //spawn note    - sky blue note
                                Note note = ((GameObject)Instantiate(blueSkyNotePrefab)).GetComponent<Note>();
                                note.PlaceNote(this, noteBeats[nextNoteIndex], noteStartZ, noteEndZ, noteSkyBlueX, noteSkyY, noteTypes[nextNoteIndex]);
                                notesOnScreen.Enqueue(note);
                                break;
                            }
                        case 3:
                            {   //spawn note    - sky red note
                                Note note = ((GameObject)Instantiate(redSkyNotePrefab)).GetComponent<Note>();
                                note.PlaceNote(this, noteBeats[nextNoteIndex], noteStartZ, noteEndZ, noteSkyRedX, noteSkyY, noteTypes[nextNoteIndex]);
                                notesOnScreen.Enqueue(note);
                                break;
                            }
                    }
                    //increment the note index
                    nextNoteIndex++;
                }

                //Check Inputs
                CheckInput();

                //Has Player missed any notes
                //Checks if the player has missed any notes
                if (notesOnScreen.Count > 0)
                {
                    //Find the front note
                    Note currentNote = notesOnScreen.Peek();
                    //Find its duration until the beat it is on (can be positive- time left to go, or negative- player has missed the note)
                    float noteDuration = currentNote.beat - songCurrentPosInBeats;
                    //If it falls behind the negative bound for the near offset
                    if (noteDuration < -nearOffset)
                    {
                        currentNote.gameObject.SetActive(false);
                        AddHealth(missHP);
                        missCount++;
                        playerCombo = 0;
                        StartCoroutine(IndicatorPosition(missIndicator, currentNote.indicator));
                        notesOnScreen.Dequeue();
                    }
                }

                //Has player's combo surpassed their max combo 
                if(playerCombo > playerComboMax)
                {
                    //update max combo to current combo
                    playerComboMax = playerCombo;
                }


                //Has chart finished
                //if there is not a next note and there isnt any notes on screen
                if (nextNoteIndex >= noteBeats.Length && notesOnScreen.Count <= 0)
                {
                    Debug.Log("Chart Finished!");
                    chartEnded = true;
                }

                //Has the player lost all their HP
                //Hard Mode
                //if the player has lost all their HP
                //if (playerHP <= 0)
                //{
                //    Debug.Log("Player Died!");
                //    chartEnded = true;
                //}

                //Low Priority Updates
                //Update Text
                TextUpdate();
            }
        }

        if (chartEnded && !gameEnded) //
        {   //game ended

            GetComponent<AudioSource>().Pause();

            if (playerHP >= 80) Debug.Log("You cleared!");
            if (playerHP < 80) Debug.Log("You failed!");

            Debug.Log("Score: " + playerScore);
            Debug.Log("Hit Count: " + hitCount + "/" + noteBeats.Length);
            Debug.Log("Near Count: " + (earlyCount + lateCount) + " - E:" + earlyCount + " | L:" + lateCount);
            Debug.Log("Miss Count: " + missCount + "/" + noteBeats.Length);
            gameEnded = true;
        }
    }

    void PlayerInputted(int note)
    {
        //Map Maker Timer
        Debug.Log("Hit:" + songCurrentPosInBeats.ToString("F2"));

        //mark the song position where the player hit
        float hit = songCurrentPosInBeats;

        //are there any notes to be hit
        if (notesOnScreen.Count > 0)
        {
            // Get the current note
            Note currentNote = notesOnScreen.Peek();

            // calculate the timing between the player hit and the note's beat  
            float hitTiming = currentNote.beat - hit;

            //if the note type of the current note is equal to button hit
            if (currentNote.type == note)
            {
                //if the hit timing is within the positive and negative bounds of the hit offset
                if (hitTiming <= hitOffset && hitTiming >= -hitOffset)
                {   //player hit the note
                    currentNote.gameObject.SetActive(false);
                    playerScore += hitScore;
                    playerCombo++;
                    StartCoroutine(IndicatorSlow(comboIndicator));
                    StartCoroutine(IndicatorPosition(hitIndicator, currentNote.indicator));
                    notesOnScreen.Dequeue();
                    AddHealth(hitHP);
                    hitCount++;
                } //else if the hit timing is within the positive and negative bounds of the near offset 
                else if (hitTiming <= nearOffset && hitTiming >= -nearOffset)
                {   //player neared the note
                    currentNote.gameObject.SetActive(false);
                    playerScore += nearScore;
                    playerCombo++;
                    StartCoroutine(IndicatorSlow(comboIndicator));
                    notesOnScreen.Dequeue();
                    //if the hit timing was positive
                    if (hitTiming > 0)
                    {   //player hit the note early
                        earlyCount++;
                        StartCoroutine(IndicatorFast(earlyIndicator));
                    }
                    else
                    {   //player hit the note late
                        lateCount++;
                        StartCoroutine(IndicatorFast(lateIndicator));
                    }
                }
            }
            //else if the hit timing is less than the near off set- they were going for that note but didnt hit the right button
            else if (hitTiming < nearOffset)
            {
                //didnt hit the right button
                currentNote.gameObject.SetActive(false);
                AddHealth(missHP);
                missCount++;
                playerCombo = 0;
                StartCoroutine(IndicatorPosition(wrongIndicator,currentNote.indicator));
                notesOnScreen.Dequeue();
            }
        }

        

    }

    void CheckInput()
    {
        if(Input.GetAxis("LeftGround") > 0.2f)
        {
            if(!blueGroundInUse)
            {
                PlayerInputted(0);
                blueGroundInUse = true;
            }
        }
        else if(Input.GetAxis("LeftGround") == 0)
        {
            blueGroundInUse = false;
        }

        if (Input.GetAxis("RightGround") > 0.2f)
        {
            if (!redGroundInUse)
            {
                PlayerInputted(1);
                redGroundInUse = true;
            }
        }
        else if (Input.GetAxis("RightGround") == 0)
        {
            redGroundInUse = false;
        }

        if(Input.GetAxis("LeftHorizontal") > 0.6f || Input.GetAxis("LeftHorizontal") < -0.6f)
        {
            if (!blueXInUse)
            {
                PlayerInputted(2);
                blueXInUse = true;
            }
        }
        else if (Input.GetAxis("LeftHorizontal") == 0)
        {
            blueXInUse = false;
        }

        if (Input.GetAxis("LeftVertical") > 0.6f || Input.GetAxis("LeftVertical") < -0.6f)
        {
            if (!blueYInUse)
            {
                PlayerInputted(2);
                blueYInUse = true;
            }
        }
        else if (Input.GetAxis("LeftVertical") == 0)
        {
            blueYInUse = false;
        }


        if (Input.GetAxis("RightHorizontal") > 0.6f || Input.GetAxis("RightHorizontal") < -0.6f)
        {
            if (!redXInUse)
            {
                PlayerInputted(3);
                redXInUse = true;
            }
        }
        else if (Input.GetAxis("RightHorizontal") == 0)
        {
            redXInUse = false;
        }

        if (Input.GetAxis("RightVertical") > 0.6f || Input.GetAxis("RightVertical") < -0.6f)
        {
            if (!redYInUse)
            {
                PlayerInputted(3);
                redYInUse = true;
            }
        }
        else if (Input.GetAxis("RightVertical") == 0)
        {
            redYInUse = false;
        }

    }

    void TextUpdate()
    {
        hitsText.text = hitCount.ToString("F0");
        missesText.text = missCount.ToString("F0");
        earliesText.text = earlyCount.ToString("F0");
        latesText.text = lateCount.ToString("F0");
        healthText.text = playerHP.ToString("F0");
        comboText.text = playerCombo.ToString("F0");
        maxComboText.text = playerComboMax.ToString("F0");
        scoreText.text = playerScore.ToString("F0");
    }

    //adds or removes health and clamps the value between 0-100
    void AddHealth(float health)
    {
        //take player hp and add new hp value to it
        float newHealth = playerHP + health;

        //if new health is over 100
        if (newHealth > 100)
        {//clamp to 100
            playerHP = 100;
        }//if new health is less than 0
        else if (newHealth < 0)
        {//clamp to 0
            playerHP = 0;
        }//means newHealth is within our range so:
        else
        {//set new hp to players hp
            playerHP = newHealth;
        }

    }


    private IEnumerator IndicatorFast(GameObject indicator)
    {
        indicator.SetActive(true);

        yield return new WaitForSeconds(0.25f);

        indicator.SetActive(false);
    }

    private IEnumerator IndicatorSlow(GameObject indicator)
    {
        indicator.SetActive(true);

        yield return new WaitForSeconds(0.7f);

        indicator.SetActive(false);
    }

    private IEnumerator IndicatorPosition(GameObject indicator, Vector3 pos)
    {
        indicator.transform.localPosition = pos;
        indicator.SetActive(true);

        yield return new WaitForSeconds(0.25f);

        indicator.SetActive(false);
    }



}
            

  