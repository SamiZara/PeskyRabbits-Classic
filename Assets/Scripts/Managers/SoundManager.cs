using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    public AudioSource enemyEaten, loseGame, backgroundLoop;
    private bool isMute;

    void Awake()
    {
        instance = this;
        int sound = PlayerPrefs.GetInt("Sound", 0);
        if (sound == 0)
        {
            isMute = false;
        }
        else if (sound == 1)
        {
            isMute = true;
            MuteAllSounds();
        }
    }

    public void MuteAllSounds()
    {
        backgroundLoop.volume = 0;
        loseGame.volume = 0;
        enemyEaten.volume = 0;
    }

    public void PlayEnemeyEaten()
    {
        enemyEaten.Play();
    }

    public void LoseGame()
    {
        loseGame.Play();
    }
}
