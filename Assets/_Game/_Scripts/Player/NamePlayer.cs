using TMPro;
using UnityEngine;

public class NamePlayer : MonoBehaviour
{
    [SerializeField] private TMP_InputField namePlayer;
    [SerializeField] private TextMeshProUGUI namePlayerInGame;
    private void Start()
    {
        namePlayer.text = PlayerPrefs.GetString(ApplicationVariable.NAME_PLAYER, "YOU");
        namePlayerInGame.text = namePlayer.text;
    }
    public void ChangeNamePlayer()
    {
        PlayerPrefs.SetString(ApplicationVariable.NAME_PLAYER, namePlayer.text);
        namePlayer.text = PlayerPrefs.GetString(ApplicationVariable.NAME_PLAYER, "YOU");
        namePlayerInGame.text = namePlayer.text;
    }
}
