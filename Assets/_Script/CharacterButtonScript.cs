using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButtonScript : MonoBehaviour
{
    public GameManager Gm;

    public void CharButton()
    {
        Gm.objCharbtn = this.gameObject;

    }

}



