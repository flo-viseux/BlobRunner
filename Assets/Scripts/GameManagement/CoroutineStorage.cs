using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public class CoroutineStorage
    {
        private List<Coroutine> gameRoutines;
        private int length;

        public CoroutineStorage(int size)
        {
            gameRoutines = new List<Coroutine>(size);
            length = 0;
        }

        public List<Coroutine> GameRoutines => gameRoutines;

        public void AddRoutine(Coroutine routine)
        {
            gameRoutines.Add(routine);
            length++;
        } 
        public void RemoveRoutine(Coroutine routine)
        {
            gameRoutines.Remove(routine);
            length--;
        }
        public void ClearRoutines()
        {
            gameRoutines.Clear();
            length = 0;
        }

        public int Length => length;
    }
}