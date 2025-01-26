using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteDictionary : MonoBehaviour
{
    public string[] soapNames;
    public Sprite[] allSprites;
    
    public Dictionary<(PlayerColor, string), Sprite> spriteDictionary;

    private void Start()
    {
        SetUpDictionary();
    }

    public void SetUpDictionary()
    {
        spriteDictionary = new Dictionary<(PlayerColor, string), Sprite>
        {
            { (PlayerColor.Purple, soapNames[0]), allSprites[0] },
            { (PlayerColor.Blue, soapNames[0]), allSprites[1] },
            { (PlayerColor.Green, soapNames[0]), allSprites[2] },
            { (PlayerColor.Pink, soapNames[0]), allSprites[3] },
            { (PlayerColor.Red, soapNames[0]), allSprites[4] },
            { (PlayerColor.celeste, soapNames[0]), allSprites[5] },
            { (PlayerColor.Yellow, soapNames[0]), allSprites[6] },
            { (PlayerColor.Black, soapNames[0]), allSprites[7] },

            { (PlayerColor.Purple, soapNames[1]), allSprites[8] },
            { (PlayerColor.Blue, soapNames[1]), allSprites[9] },
            { (PlayerColor.Green, soapNames[1]), allSprites[10] },
            { (PlayerColor.Pink, soapNames[1]), allSprites[11] },
            { (PlayerColor.Red, soapNames[1]), allSprites[12] },
            { (PlayerColor.celeste, soapNames[1]), allSprites[13] },
            { (PlayerColor.Yellow, soapNames[1]), allSprites[14] },
            { (PlayerColor.Black, soapNames[1]), allSprites[15] },

            { (PlayerColor.Purple, soapNames[2]), allSprites[16] },
            { (PlayerColor.Blue, soapNames[2]), allSprites[17] },
            { (PlayerColor.Green, soapNames[2]), allSprites[18] },
            { (PlayerColor.Pink, soapNames[2]), allSprites[19] },
            { (PlayerColor.Red, soapNames[2]), allSprites[20] },
            { (PlayerColor.celeste, soapNames[2]), allSprites[21] },
            { (PlayerColor.Yellow, soapNames[2]), allSprites[22] },
            { (PlayerColor.Black, soapNames[2]), allSprites[23] },

            { (PlayerColor.Purple, soapNames[3]), allSprites[24] },
            { (PlayerColor.Blue, soapNames[3]), allSprites[25] },
            { (PlayerColor.Green, soapNames[3]), allSprites[26] },
            { (PlayerColor.Pink, soapNames[3]), allSprites[27] },
            { (PlayerColor.Red, soapNames[3]), allSprites[28] },
            { (PlayerColor.celeste, soapNames[3]), allSprites[29] },
            { (PlayerColor.Yellow, soapNames[3]), allSprites[30] },
            { (PlayerColor.Black, soapNames[3]), allSprites[31] },
        };
    }
}
