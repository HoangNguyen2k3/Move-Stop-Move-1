using System.Collections;
using UnityEngine;

public class LobbyAnimUI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject lobby;
    [SerializeField] private LobbyManager lobby_manage;
    public void EnableLobby()
    {
        //Debug.LogWarning("vcl");
        lobby.SetActive(true);
        animator.Play("Lobby");
        //  StartCoroutine(WaitForAnimation());
    }

    public void DisableLobby()
    {
        //Debug.LogWarning("vcl1");
        StartCoroutine(DisableLobbyCoroutine());
    }

    private IEnumerator DisableLobbyCoroutine()
    {
        animator.Play("Lobby");
        //yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        yield return new WaitForSeconds(0.3f);
        lobby_manage.InGame();
        //     lobby.SetActive(false);
    }
    //In other TH
    public void DisableLobbyInOther()
    {
        StartCoroutine(DisableLobbyInOtherCoroutine());
    }
    private IEnumerator DisableLobbyInOtherCoroutine()
    {
        animator.SetTrigger("Lobby");
        //yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        yield return new WaitForSeconds(0.3f);
        //animator.Play("New State");
        //     lobby.SetActive(false);
    }
}
