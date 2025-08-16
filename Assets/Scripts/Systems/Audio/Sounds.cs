using UnityEngine;

public class Sounds : Singleton<Sounds>
{
}

//This script is to be attached to the game object
//That holds all the audio sources that can be transferable across scenes
//Like UI click button sounds.

//This is so the "AudioManager" can reference those audio sources across scenes
//Because the "AudioManager" is a singleton.