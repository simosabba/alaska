using Alaska.Foundation.Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Items
{
    internal enum ProtectionMode { Enabled, Disabled }

    public class ItemProtectionContext : IDisposable
    {
        private const ItemState DefaultState = ItemState.Unprotected;
        private const ItemOrigin DefaultOrigin = ItemOrigin.Application;

        [ThreadStatic]
        private static ItemState? _ProtectionState;

        [ThreadStatic]
        private static ItemOrigin? _Origin;

        public ItemProtectionContext(ItemOrigin origin)
            : this()
        {
            _Origin = origin;
        }

        public ItemProtectionContext()
        {
            _ProtectionState = ItemState.Protected;
        }

        public void Dispose()
        {
            _ProtectionState = null;
            _Origin = null;
        }

        internal static ItemState ProtectionState => _ProtectionState.HasValue ? _ProtectionState.Value : DefaultState;
        internal static ItemOrigin Origin => _Origin.HasValue ? _Origin.Value : DefaultOrigin;
    }
}
