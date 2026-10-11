// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using System;
    using Internal;

    /// <summary>
    /// A read scope keeps a <see cref="LudeoReadableObject"/> or <see cref="LudeoReadableComponent"/>
    /// active so that attributes can be read from it. It is created via
    /// <see cref="LudeoReadableObject.EnterObjectScope"/> or <see cref="LudeoReadableComponent.EnterComponentScope"/> and
    /// should always be used inside a <c>using</c> block so the scope is closed automatically:
    /// <code>
    /// using (readableObject.EnterObjectScope())
    /// {
    ///     readableObject.ReadData("health", out int health);
    /// }
    /// </code>
    /// Reads only take effect while the matching scope is open. Component scopes must be opened inside their
    /// parent object's scope.
    /// </summary>
    public readonly struct LudeoReadScope : IDisposable
    {
        private readonly bool m_entered;
        private readonly bool m_isComponent;

        internal LudeoReadScope(bool isComponent, bool entered)
        {
            m_isComponent = isComponent;
            m_entered = entered;
        }

        /// <summary>
        /// True when the object/component was entered successfully and reads within this scope will return data.
        /// If false, the scope failed to open and reads will be ignored.
        /// </summary>
        public bool IsValid { get { return m_entered; } }

        /// <summary>Closes the scope, leaving the object or component on the underlying data reader.</summary>
        public void Dispose()
        {
            if (!m_entered)
                return;

            if (m_isComponent)
                LudeoDataReader_GetObjects.LeaveComponent();
            else
                LudeoDataReader_GetObjects.LeaveObject();
        }
    }
}
