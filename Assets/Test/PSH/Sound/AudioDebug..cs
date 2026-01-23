using UnityEngine;
public class AudioDebug : MonoBehaviour
{
    private void Start()
    {
        var src = GetComponent<AudioSource>();
        /*Debug.Log($"Clip: {src.clip}");
        Debug.Log($"Volume: {src.volume}");
        Debug.Log($"Mute: {src.mute}");
        Debug.Log($"Listener Volume: {AudioListener.volume}");*/
        src.Play();
    }
}

