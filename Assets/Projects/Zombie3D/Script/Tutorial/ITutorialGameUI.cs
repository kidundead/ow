using UnityEngine;
using System.Collections;
using Zombie3D;


public interface ITutorialGameUI {

    void SetTutorialText(string text);
    void SetTutorialUIEvent(ITutorialUIEvent tEvent);
    void EnableTutorialOKButton(bool enable);
}
