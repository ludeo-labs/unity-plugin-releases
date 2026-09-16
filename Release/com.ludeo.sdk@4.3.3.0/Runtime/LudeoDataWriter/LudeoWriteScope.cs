// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using System;
    using Internal;

    /// <summary>
    /// A write scope keeps a <see cref="LudeoWritableObject"/> or <see cref="LudeoWritableComponent"/> active so that
    /// attributes can be written to it. It is created via <see cref="LudeoWritableObject.EnterObjectScope"/> or
    /// <see cref="LudeoWritableComponent.EnterComponentScope"/> and should always be used inside a <c>using</c> block so the
    /// scope is closed automatically:
    /// <code>
    /// using (writableObject.EnterObjectScope())
    /// {
    ///     writableObject.WriteData("health", 100);
    /// }
    /// </code>
    /// WriteData calls only take effect while the matching scope is open. Component scopes must be opened
    /// inside their parent object's scope.
    /// </summary>
    public readonly struct LudeoWriteScope : IDisposable
    {
        private readonly bool m_entered;
        private readonly bool m_isComponent;

        internal LudeoWriteScope(bool isComponent, bool entered)
        {
            m_isComponent = isComponent;
            m_entered = entered;
        }

        /// <summary>
        /// True when the object/component was entered successfully and attributes written within this scope
        /// will be recorded. If false, the scope failed to open and writes will be ignored.
        /// </summary>
        public bool IsValid { get { return m_entered; } }

        /// <summary>Closes the scope, leaving the object or component on the underlying data writer.</summary>
        public void Dispose()
        {
            if (!m_entered)
                return;

            if (m_isComponent)
                LudeoDataWriter.LeaveComponent();
            else
                LudeoDataWriter.LeaveObject();
        }
    }
}
