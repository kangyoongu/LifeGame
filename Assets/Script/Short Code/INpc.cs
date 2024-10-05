using UnityEngine;

public interface INpc
{
    void LookPlayer();
    void OffInter();
    void OnClickTalk();
    void FindChildRecursivelyByName(Transform parent, string name);
    float ButtonColor(float value);
    void OnFarewall();
}
