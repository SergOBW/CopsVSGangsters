using UnityEngine;

namespace Abstract
{
    public abstract class GlobalMonoMechanic : MonoBehaviour , IMechanic
    {
        public abstract void Initialize();
    }
}