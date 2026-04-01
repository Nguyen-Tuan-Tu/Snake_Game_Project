using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewTopic", menuName = "GameData/Topic")]
public class TopicData : ScriptableObject
{
    public string topicName;
    public Sprite topicIcon;
    public string[] words;
    public bool isUnlocked;
}
