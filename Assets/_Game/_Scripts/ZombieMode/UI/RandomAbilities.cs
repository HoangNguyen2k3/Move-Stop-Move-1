using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomAbilities : MonoBehaviour
{
    [SerializeField] private ListAbilities list;
    //Show abilities in pause
    [SerializeField] private GameObject noHaveAbilities;
    [SerializeField] private GameObject haveAbilities;
    [SerializeField] private Image img_abilities;
    [SerializeField] private TextMeshProUGUI name_abilities;


    public TextMeshProUGUI nameAbilities;
    public Image ability;
    public int choice = 1;
    public Button btnChange;
    public Button btnChageVer2;
    public PlayerZombie player;
    private int choice1;
    private int choice2;
    private Sprite ability1;
    private Sprite ability2;
    private ZombieGameController zombieManager;

    private void Start() {
        zombieManager = FindFirstObjectByType<ZombieGameController>();
        list.SetupAbilitiesInStartGame();
        ability1 = list.spriteAbilities[list.current_rand_1];
        ability2 = list.spriteAbilities[list.current_rand_2];
        choice1 = list.current_rand_1 + 1;
        choice2 = list.current_rand_2 + 1;
        btnChange.onClick.AddListener(() => ChangeAbilities());
        btnChageVer2.onClick.AddListener(() => ChangeAbilities());
        ability.sprite = ability1;
        choice = choice1;
        nameAbilities.text = list.list_name[list.current_rand_1];
    }
    private void ChangeAbilities() {
        if (choice == choice1) {
            ability.sprite = ability2;
            choice = choice2;
            nameAbilities.text = list.list_name[list.current_rand_2];
        }
        else {
            ability.sprite = ability1;
            choice = choice1;
            nameAbilities.text = list.list_name[list.current_rand_1];
        }
    }
    public void ChooseAbilities() {
        Debug.Log(choice);
        switch (choice) {
            case 2:
                player.OrbitWeapon();
                player.num_choose = choice; break;
            case 8:
                zombieManager.x2GoldAbilities = true;
                player.num_choose = 0; break;
            case 10:
                player.UpSPeed();
                player.num_choose = 0; break;
            case 12:
                player.GetRevive = true;
                player.num_choose = 0;
                break;
            case 13:
                player.levelManager.isUpScale = true;
                player.num_choose = 0;
                break;
            default: player.num_choose = choice; break;
        }
        ChooseAbilitiesSetting();
    }
    public void ChooseAbilitiesSetting() {
        noHaveAbilities.SetActive(false);
        haveAbilities.SetActive(true);
        img_abilities.sprite = list.spriteAbilities[choice - 1];
        name_abilities.text = list.list_name[choice - 1];

    }
    public void NoChoiceAbilities() {
        noHaveAbilities.SetActive(true);
        haveAbilities.SetActive(false);
    }
}
