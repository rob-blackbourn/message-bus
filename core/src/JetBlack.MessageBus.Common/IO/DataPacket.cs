#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace JetBlack.MessageBus.Common.IO
{
    public class DataPacket : IEquatable<DataPacket>
    {
        public DataPacket(ISet<int>? entitlements, byte[]? data)
        {
            Entitlements = entitlements;
            Data = data;
        }

        public ISet<int>? Entitlements { get; }
        public byte[]? Data { get; }

        public bool Equals(DataPacket? other)
        {
            return other != null &&
                (
                    (Entitlements == null && other.Entitlements == null) ||
                    (Entitlements != null && Entitlements.SetEquals(other.Entitlements))
                ) &&
                (
                    (Data == null && other.Data == null) ||
                    (Data != null && Data.SequenceEqual(other.Data))
                );
        }

        public bool IsAuthorized(ISet<int> allEntitlements)
        {
            return Entitlements != null && allEntitlements.IsSupersetOf(Entitlements);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DataPacket);
        }

        public override int GetHashCode()
        {
            return (Entitlements?.GetHashCode() ?? 0) ^ (Data?.GetHashCode() ?? 0);
        }

        public override string ToString() => $"{nameof(Entitlements)}.Count={Entitlements?.Count ?? 0}, {nameof(Data)}.Length={Data?.Length ?? 0}";
    }
}