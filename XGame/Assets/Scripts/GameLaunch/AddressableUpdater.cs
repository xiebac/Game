using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddressableUpdater : MonoBehaviour
{
    public Text status_Text;
    public Slider update_Slider;
    float totalTime = 0f;
    bool needUpdateRes = false;
    // Start is called before the first frame update
    void Start()
    {
        totalTime = 0f;
        status_Text.text = "正在检测资源更新...";
    }
    public void StartCheckUpdate()
    {
        StartCoroutine(checkUpdate());
    }
    IEnumerator checkUpdate()
    {
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
