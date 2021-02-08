using System;
using UnityEngine;

namespace Akaal.Editor
{
    /// <summary>
    /// Is a registry of all the various icon drawers registered to the service.
    /// </summary>
    public interface IDrawerRegistry
    {
        /// <summary>
        /// Get an icon drawer that can be used to draw an object of the given type.
        /// </summary>
        /// <param name="type">Type of the object for which icon has to be drawn.</param>
        /// <returns>An icon drawer suitable for the type.</returns>
        IDrawer GetIconDrawer(Type type);

        /// <summary>
        /// Adds an icon drawer to this registry.
        /// </summary>
        /// <param name="drawer">The icon drawer to be registered.</param>
        void RegisterIconDrawer(IDrawer drawer);

        /// <summary>
        /// Removes an icon drawer from this registry.
        /// </summary>
        /// <param name="drawer">The icon drawer to be removed.</param>
        /// <returns>True if such an icon drawer was in the registry and has now been removed. False otherwise.</returns>
        bool UnregisterIconDrawer(IDrawer drawer);
    }
    /// <summary>
    /// An Icon Drawer has the responsibility of actually drawing a graphic for the passed in value.
    /// Implement this and register it using an IDrawerRegisterer.
    /// </summary>
    public interface IDrawer
    {
        void Draw(object       value, Rect fullRect, bool selected, IconStyle style);
        bool ValidForType(Type type);
        int  Priority { get; }
    }
}