using System;

namespace Net.RichardLord.Ash.Core
{
    public interface IGame
    {
        /// <summary>
        /// Indicates if the game is currently in its update loop.
        /// </summary>
        bool Updating { get; }

        /// <summary>
        /// Dispatched when the update loop ends. If you want to add and remove systems from the
        /// game it is usually best not to do so during the update loop. To avoid this you can
        /// listen for this signal and make the change when the signal is dispatched.
        /// </summary>
        event Action UpdateComplete;

        void AddSystem(SystemBase system, int priority);

        SystemBase GetSystem(Type type);

        void RemoveSystem(SystemBase system);

        void RemoveAllSystems();

        void Update(double time);
    }
}
