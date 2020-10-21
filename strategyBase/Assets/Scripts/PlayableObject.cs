using UnityEngine;

namespace Assets.Scripts
{
    public class PlayableObject : Unit
    {
        public int factionId; //Change to enum later

        [SerializeField]private int _health;

        private protected GridCell _targetCell;

        public virtual void SetDestination(GridCell target)
        {
            _targetCell = target;
        }
    }
}
