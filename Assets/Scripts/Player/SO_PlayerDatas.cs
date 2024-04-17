using UnityEngine;

namespace Player
{
    public enum PlayerState
    {
        NORMAL,
        SMALL,
        INVISIBLE,
    }
    
    [CreateAssetMenu(fileName = "PlayerDatas", menuName = "ScriptableObjects/Create Player Datas", order = 0)]
    public class SO_PlayerDatas : ScriptableObject
    {
        public int NbLife;
        public float Speed;
        public PlayerState State;

        public void InitDatas()
        {
            NbLife = 1;
            State = PlayerState.NORMAL;
        }
    }
}