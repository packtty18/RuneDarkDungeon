using UnityEngine;

public class AudioSettingsDebug : MonoBehaviour
{
    private void Start()
    {
        Debug.Log($"Audio Configuration: {AudioSettings.GetConfiguration()}");
        Debug.Log($"Output Sample Rate: {AudioSettings.outputSampleRate}");
        Debug.Log($"Speaker Mode: {AudioSettings.speakerMode}");
        Debug.Log($"DSP Buffer Size: {AudioSettings.GetConfiguration().dspBufferSize}");
        
        // 오디오 장치 정보 (Unity 2022.2 이상)
        Debug.Log($"Get Spatial Audio Device Name: {AudioSettings.GetSpatializerPluginName()}");
    }
}
