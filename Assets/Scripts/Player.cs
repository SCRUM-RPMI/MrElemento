using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{


        public void savePlayer(CharacterStats stats)
        {
                SaveSystem.SavePlayer(stats);
        }

        public void LoadPLayer()
        {
                PlayerData data = SaveSystem.LoadPlayer();
             

                Vector3 position;
                position.x = data.position[0];
                position.y = data.position[1];
                position.z = data.position[2];

                transform.position = position;
        }

    
}
