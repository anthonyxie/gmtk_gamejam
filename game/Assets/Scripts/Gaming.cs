using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Gaming : MonoBehaviour {
    // Start is called before the first frame update
    public Player player;
    public Follow camFollow;
    public bool isPossessing;
    public Possessable possessed = null;

    public UnityEvent onPickUp;
    public bool isDead;

    public List<string> items;

    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    public void ForceUnpossess() {
        Debug.Log("mfw I am being forced to unpossussy");
        if (possessed) {
            isPossessing = false;
            possessed.Unpossess();
        }
    }

    public void ChangeScene(string pathToScene) {
        this.WaitThen(0.9f, () => { SceneManager.LoadSceneAsync(pathToScene); });
    }

    public void OnPickUp(string id) {
        onPickUp.Invoke();
    }

    public void AddItem(string id) {
        items.Add(id);
    }


}
