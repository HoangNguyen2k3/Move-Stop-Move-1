using System;
using UnityEngine;

public static class ApplicationVariable
{
    [Header("-----------------Normal Mode---------------------")]
    //Status Player and enemy
    public static string IDLE_PLAYER_STATE = "IsIdle";
    public static string ATTACK_PLAYER_STATE = "IsAttack";
    public static string IS_DEAD_STATE = "IsDead";
    public static string WIN_STATE = "IsWin";
    public static string ULTI = "IsUlti";
    //Tag
    public static string PLAYER_TAG = "Player";
    public static string ENEMY_TAG = "Enemy";
    public static string IGNORE_TAG = "Ignore";
    //Status purchase
    public static string purchase_status = "Purchased";
    public static string eqquipped_status = "Equipped";
    public static string notPurchase_status = "NotPurchased";
    public static string COIN = "Coin";
    public static string PATH_CLOTHES_SAVE = "Clothes";
    public static string PATH_CLOTHES_PLAYER = "Player_Clothes";
    public static string PATH_PERM_PARAM = "PermParam";
    public static string CURRENT_WEAPON_EQUIP = "EquipCurrentWeapon";
    //Level Game
    public static string LEVEL_GAME = "num_enemy_level";
    public static string CURRENT_LEVEL_GAME = "LevelGame";
    public static string CURRENT_MAP = "Current_Map";
    public static string MAX_RECORD_GAME = "Max_Record";
    //Ads One Time Clothes
    public static string NAME_ADD_ONE_TIME = "NameAdsOneTime";
    public static string TYPE_ADD_ONE_TIME = "TypeAdsOneTime";
    public static string USE_ONE_TIME_IN_ZOMBIE = "zombie_clother";
    //
    public static string TEXT_ANNOUCE = "TextAnouce";
    //Player
    public static string NAME_PLAYER = "NamePlayer";
    //Save data
    public static string FIRST_SAVE = "firstSave";
    //Sound
    public static string SOUND = "Sound";
    public static string VIBRANT = "Vibrant";
    [Header("--------------Zombie Mode-----------------")]
    public static string DAY_ZOMBIE_MODE = "DayZombieMode";
    public static string EXPLOSION_TAG = "Explosion";
    public static string THROW_WEAPON_TAG = "ThrowWeapon";
    //Floating Text
    public static string FLOATING_TEXT = "FloatingText";
    //Saving Zombie Mode
    public static string FIRST_SAVE_ZOMBIE_MODE = "firstSaveZombieMode";
    //Animation Zombie
    public static string ZOMBIE_WALK = "isWalk";
    public static string ZOMBIE_WIN = "IsWin";

    //Ads Reward
    public enum ConditionAds
    {
        None,
        ReviveNormal,
        ReviveInZombie,
    }
    public enum StateGame
    {
        Winning,
        Losing,
        InLobby,
        InPlaying,
        InRevive,
        EndRevive,
        Setting
    }
}
