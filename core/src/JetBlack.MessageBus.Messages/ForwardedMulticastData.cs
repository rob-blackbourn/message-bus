#nullable enable

using System;
using System.Linq;

using JetBlack.MessageBus.Common.IO;

namespace JetBlack.MessageBus.Messages
{
    public class ForwardedMulticastData : Message, IEquatable<ForwardedMulticastData>
    {
        public ForwardedMulticastData(string user, string host, string feed, string topic, bool isImage, DataPacket[]? dataPackets)
            : base(MessageType.ForwardedMulticastData)
        {
            User = user;
            Host = host;
            Feed = feed;
            Topic = topic;
            IsImage = isImage;
            DataPackets = dataPackets;
        }

        public string User { get; }
        public string Host { get; }
        public string Feed { get; }
        public string Topic { get; }
        public bool IsImage { get; }
        public DataPacket[]? DataPackets { get; }

        public static ForwardedMulticastData ReadBody(DataReader reader)
        {
            var user = reader.ReadString();
            var host = reader.ReadString();
            var feed = reader.ReadString();
            var topic = reader.ReadString();
            var isImage = reader.ReadBoolean();
            var dataPackets = reader.ReadBinaryDataPacketArray();
            return new ForwardedMulticastData(user, host, feed, topic, isImage, dataPackets);
        }

        public override DataWriter Write(DataWriter writer)
        {
            base.Write(writer);
            writer.Write(User);
            writer.Write(Host);
            writer.Write(Feed);
            writer.Write(Topic);
            writer.Write(IsImage);
            writer.Write(DataPackets);
            return writer;
        }

        public bool Equals(ForwardedMulticastData? other)
        {
            return other != null &&
                User == other.User &&
                Host == other.Host &&
                Feed == other.Feed &&
                Topic == other.Topic &&
              IsImage == other.IsImage &&
              (
                (DataPackets == null && other.DataPackets == null) ||
                (DataPackets != null && other.DataPackets != null && DataPackets.SequenceEqual(other.DataPackets))
              );
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ForwardedMulticastData);
        }

        public override int GetHashCode()
        {
            return MessageType.GetHashCode() ^
              User.GetHashCode() ^
              Host.GetHashCode() ^
              Feed.GetHashCode() ^
              Topic.GetHashCode() ^
              IsImage.GetHashCode() ^
              (DataPackets?.GetHashCode() ?? 0);
        }

        public override string ToString() =>
            $"{base.ToString()}" +
            $",{nameof(User)}=\"{User}\"" +
            $",{nameof(Host)}=\"{Host}\"" +
            $",{nameof(Feed)}=\"{Feed}\"" +
            $",{nameof(Topic)}=\"{Topic}\"" +
            $",{nameof(IsImage)}={IsImage}" +
            $",{nameof(DataPackets)}.Length={DataPackets?.Length}";
    }
}
