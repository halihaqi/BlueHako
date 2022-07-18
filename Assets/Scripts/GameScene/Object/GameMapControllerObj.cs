using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMapControllerObj : SingletonMono<GameMapControllerObj>
{
    public List<GameObject> stops;
    public List<Transform> catPos;
    public int state = 1;
    private void Start()
    {
        float complete = DataMgr.Instance.NowPlayerInfo.complete;
        if(complete >= 30f)
        {
            state = 2;
            stops[0].gameObject.SetActive(false);
            if (!DataMgr.Instance.NowPlayerInfo.taskList[0.ToString()])
                ResMgr.Instance.LoadAsync<GameObject>("Prefabs/NPC/Cat", (obj) =>
                {
                    obj.transform.position = catPos[0].position;
                    obj.transform.rotation = catPos[0].rotation;
                });
        }
        if(complete >= 60f)
        {
            state = 3;
            stops[1].gameObject.SetActive(false);
            if (!DataMgr.Instance.NowPlayerInfo.taskList[1.ToString()])
                for (int i = 1; i < 3; i++)
                {
                    int index = i;
                    ResMgr.Instance.LoadAsync<GameObject>("Prefabs/NPC/Cat", (obj) =>
                    {
                        obj.transform.position = catPos[index].position;
                        obj.transform.rotation = catPos[index].rotation;
                    });
                }
        }
        if (complete >= 90f)
        {
            state = 3;
            stops[2].gameObject.SetActive(false);
            if (!DataMgr.Instance.NowPlayerInfo.taskList[2.ToString()])
                for (int i = 3; i < 7; i++)
                {
                    int index = i;
                    ResMgr.Instance.LoadAsync<GameObject>("Prefabs/NPC/Cat", (obj) =>
                    {
                        obj.transform.position = catPos[index].position;
                        obj.transform.rotation = catPos[index].rotation;
                    });
                }
        }
    }
}
