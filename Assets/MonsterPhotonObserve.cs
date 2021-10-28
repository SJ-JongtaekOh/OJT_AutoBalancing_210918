using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class MonsterPhotonObserve :MonoBehaviourPunCallbacks,IPunObservable
{
    [SerializeField]
    Scrollbar hp; 

   

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       
    }

    // Start is called before the first frame update
    private void Awake()
    {
        

    }



    // Update is called once per frame

}
