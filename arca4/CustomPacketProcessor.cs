using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ares.IO;
using Ares.Protocol;

namespace arca4
{
    class CustomPacketProcessor
    {
        public static void Eval(UserObject userobj, ProtoMessage msg, AresTCPPacketReader packet)
        {
            switch (msg)
            {
                case ProtoMessage.MSG_CHAT_CLIENT_CUSTOM_ADD_TAGS:
                    ProcessAddCustomTag(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_CUSTOM_REM_TAGS:
                    ProcessRemCustomTag(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_CUSTOM_FONT:
                    ProcessCustomFont(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_VC_SUPPORTED:
                    ProcessVCSupported(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_VC_FIRST:
                    ProcessVCFirst(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_VC_FIRST_TO:
                    ProcessVCFirstTo(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_VC_CHUNK:
                    ProcessVCChunk(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_VC_CHUNK_TO:
                    ProcessVCChunkTo(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_VC_IGNORE:
                    ProcessVCIgnore(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_SUPPORTS_CUSTOM_EMOTES:
                    ProcessSupportsCustomEmotes(userobj);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_CUSTOM_EMOTES_UPLOAD_ITEM:
                    ProcessCustomEmoteUpload(userobj, packet);
                    break;

                case ProtoMessage.MSG_CHAT_CLIENT_CUSTOM_EMOTE_DELETE:
                    ProcessCustomEmoteDelete(userobj, packet);
                    break;
            }
        }

        private static void ProcessAddCustomTag(UserObject userobj, AresTCPPacketReader packet)
        {
            while (packet.Remaining > 0)
                userobj.CustomTags.Add(packet.ReadString());
        }

        private static void ProcessRemCustomTag(UserObject userobj, AresTCPPacketReader packet)
        {
            while (packet.Remaining > 0)
            {
                String str = packet.ReadString();
                userobj.CustomTags.Remove(str);
            }
        }

        private static void ProcessCustomFont(UserObject userobj, AresTCPPacketReader packet)
        {
            if (packet.Remaining == 2)
            {
                userobj.Font = null;
                UserPool.BroadcastToVroom(userobj.Vroom, CustomPackets.CustomFontDefault(userobj));
            }
            else
            {
                userobj.Font = new Font(packet.ReadByte(), packet.ReadString(), packet.ReadByte(), packet.ReadByte());
                UserPool.BroadcastToVroom(userobj.Vroom, CustomPackets.CustomFont(userobj));
            }
        }

        private static void ProcessVCSupported(UserObject userobj, AresTCPPacketReader packet)
        {
            if (Settings.CanVoiceChat)
            {
                userobj.CanVCPublic = packet.ReadByte() == 1;
                userobj.CanVCPrivate = packet.ReadByte() == 1;
                UserPool.BroadcastToVroom(userobj.Vroom, CustomPackets.VoiceChatUserSupport(userobj));
            }
        }

        private static void ProcessVCFirst(UserObject userobj, AresTCPPacketReader packet)
        {
            if (Settings.CanVoiceChat)
            {
                byte[] buffer = CustomPackets.VoiceChatFirst(userobj.Name, packet.ReadBytes());
                UserPool.BroadcastVoiceClip(userobj, buffer);
            }
        }

        private static void ProcessVCFirstTo(UserObject userobj, AresTCPPacketReader packet)
        {
            if (Settings.CanVoiceChat)
            {
                String name = packet.ReadString();
                UserObject target = UserPool.Users.Find(x => x.LoggedIn && x.Name == name);

                if (target != null)
                    if (!target.VCIgnores.Contains(userobj.Name))
                        if (target.CanVCPrivate)
                        {
                            byte[] buffer = CustomPackets.VoiceChatFirstTo(userobj.Name, packet.ReadBytes());
                            target.SendPacket(buffer);
                        }
                        else userobj.SendPacket(CustomPackets.VoiceChatNoPrivate(target.Name));
                    else userobj.SendPacket(CustomPackets.VoiceChatIgnored(target.Name));
                else userobj.SendPacket(AresTCPPackets.OfflineUser(name));
            }
        }

        private static void ProcessVCChunk(UserObject userobj, AresTCPPacketReader packet)
        {
            if (Settings.CanVoiceChat)
            {
                byte[] buffer = CustomPackets.VoiceChatChunk(userobj.Name, packet.ReadBytes());
                UserPool.BroadcastVoiceClip(userobj, buffer);
            }
        }

        private static void ProcessVCChunkTo(UserObject userobj, AresTCPPacketReader packet)
        {
            if (Settings.CanVoiceChat)
            {
                String name = packet.ReadString();
                UserObject target = UserPool.Users.Find(x => x.LoggedIn && x.Name == name);

                if (target != null)
                    if (target.CanVCPrivate)
                        if (!target.VCIgnores.Contains(userobj.Name))
                        {
                            byte[] buffer = CustomPackets.VoiceChatChunkTo(userobj.Name, packet.ReadBytes());
                            target.SendPacket(buffer);
                        }
            }
        }

        private static void ProcessVCIgnore(UserObject userobj, AresTCPPacketReader packet)
        {
            if (Settings.CanVoiceChat)
            {
                String name = packet.ReadString();

                if (userobj.VCIgnores.Contains(name))
                {
                    userobj.SendPacket(AresTCPPackets.NoSuch(name + " is voice chat unignored"));
                    userobj.VCIgnores.RemoveAll(x => x == name);
                }
                else
                {
                    userobj.SendPacket(AresTCPPackets.NoSuch(name + " is voice chat ignored"));
                    userobj.VCIgnores.Add(name);
                }
            }
        }

        private static void ProcessSupportsCustomEmotes(UserObject userobj)
        {
            if (!Settings.CanCustomEmotes)
                return;

            if (!userobj.SupportsCustomEmoticons)
            {
                userobj.SupportsCustomEmoticons = true;
                userobj.CustomEmoticons.Clear();
                UserPool.SendCustomEmotesToUser(userobj);
            }
        }

        private static void ProcessCustomEmoteUpload(UserObject userobj, AresTCPPacketReader packet)
        {
            if (!Settings.CanCustomEmotes)
                return;

            ProcessSupportsCustomEmotes(userobj);

            CustomEmoticon emoticon = new CustomEmoticon
            {
                Shortcut = packet.ReadString(),
                Size = packet.ReadByte(),
                Image = packet.ReadBytes()
            };

            userobj.CustomEmoticons.Add(emoticon);

            if (userobj.CustomEmoticons.Count > 16)
                throw new Exception("exceeded custom emoticon maximum");

            byte[] buf = CustomPackets.CustomEmoteItem(userobj, emoticon);

            UserPool.Users.ForEach(x =>
            {
                if (x.LoggedIn && x.Vroom == userobj.Vroom)
                    if (x.SupportsCustomEmoticons)
                        x.SendPacket(buf);
            });
        }

        private static void ProcessCustomEmoteDelete(UserObject userobj, AresTCPPacketReader packet)
        {
            if (!Settings.CanCustomEmotes)
                return;

            String text = packet.ReadString();
            userobj.CustomEmoticons.RemoveAll(x => x.Shortcut == text);
            byte[] buf = CustomPackets.CustomEmoteDelete(userobj, text);

            UserPool.Users.ForEach(x =>
            {
                if (x.LoggedIn && x.Vroom == userobj.Vroom)
                    if (x.SupportsCustomEmoticons)
                        x.SendPacket(buf);
            });
        }
    }
}
