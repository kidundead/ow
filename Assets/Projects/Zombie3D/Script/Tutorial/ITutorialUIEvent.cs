using UnityEngine;
using System.Collections;
using Zombie3D;


public interface ITutorialUIEvent {

    void SetGameGUI(ITutorialGameUI guis);
    void OK(Player player);

}
