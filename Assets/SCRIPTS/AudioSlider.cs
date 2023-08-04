
using UnityEngine;

public class AudioSlider : MonoBehaviour
{
    public void SetVolume(float vol)
    {
        AudioListener.volume = vol;
    }
}