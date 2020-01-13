using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneWrapperButton : MonoBehaviour
{
    [SerializeField]
    string scene;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Wrap);
    }

    void Wrap()
    {
        SceneManager.LoadScene(this.scene);
    }
}
