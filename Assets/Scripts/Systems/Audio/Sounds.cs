using UnityEngine;

/// <summary>
/// Script that'll allow game objects with audio source components to persist across scenes
/// </summary>

/// <remarks>
/// 
/// This script is to be attached to a parent game object
/// that holds children game objects that each have an audio source component attached.
/// Each audio source will be one of the sound effects to be used in the game. 
/// This is to be transferable across scenes and easily accessible between all scriptS, especially given the "AudioManager" is a singleton.
/// "AudioManager" needs to access the audio sources to actually play them.
/// See <see cref="AudioManager"/> 
/// 
/// </remarks>

public class Sounds : Singleton<Sounds>
{
}
