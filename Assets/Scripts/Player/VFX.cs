using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VFX : MonoBehaviour
{
    private GameObject player;
    private TailStateManager playerState;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerCat").gameObject;
        playerState = player.GetComponent<TailStateManager>();
    }

    private void Step()
    {
        if(!playerState.isPuffyActivated)
        {
            CatToonSoundEffect("Step");
        }
    }

    //SOUND
    public void PlayPlayerSoundEffect(string clipName)
    {
        AudioSource playerAudio = player.GetComponentsInChildren<AudioSource>().FirstOrDefault(audio => audio.clip != null && audio.clip.name == clipName);
        if (playerAudio != null)
        {
            playerAudio.Play();
        }
    }

    private void CatToonSoundEffect(string clipName)
    {
        AudioSource playerAudio = transform.GetComponentsInChildren<AudioSource>().FirstOrDefault(audio => audio.clip != null && audio.clip.name == clipName);
        if (playerAudio != null)
        {
            playerAudio.Play();
        }
    }
}
