using System;

namespace Puzzles
{
    [Serializable]
    public class Puzzle
    {
        private bool active;
        private bool completed;
    
        public bool IsActive => active;
        public bool IsCompleted => completed;

        public Puzzle() => Reset();

        public void Start()
        {
            active = true;
            completed = false;
        }

        public void Complete() => completed = true;
    
        public void Reset()
        {
            active = false;
            completed = false;
        }
    }
}