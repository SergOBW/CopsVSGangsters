using Level;
using UnityEngine;

namespace Abstract
{
    public class GameModeMechanic : MonoBehaviour
    {
        protected Scenario currentScenario;
        public virtual void Initialize(Scenario scenario)
        {
            currentScenario = scenario;
        }

        public virtual void DeInitialize()
        {
            
        }
    }
}