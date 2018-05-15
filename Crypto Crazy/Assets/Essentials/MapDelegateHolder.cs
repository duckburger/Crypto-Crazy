using UnityEngine;

public class MapDelegateHolder : MonoBehaviour {


    // This will get called whenever we finish updating a rig (sends a message to UI, for example)
    public delegate void OnRigUpgraded(int rigSlot, Rig newlyInstalledRig);
    public OnRigUpgraded upgradedRigActions;


    public delegate void OnRackUpgraded(Rig newlyInstalledRig, int rackSlot);
    public OnRackUpgraded upgradedRackActions;

}
