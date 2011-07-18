﻿namespace Ares.IO
{
    /// <summary>Ares protocol message enumerator</summary>
    public enum ProtoMessage : byte
    {
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_ERROR = 0,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_RELOGIN = 1,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_LOGIN = 2,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_LOGIN_ACK = 3,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_UPDATE_STATUS = 4,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_UPDATE_USER_STATUS = 5,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_REDIRECT = 6,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_AUTOLOGIN = 7,
        /// <summary>protocol message</summary>
        MSG_SERVER_ECHO = 8,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_AVATAR = 9,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_AVATAR = 9,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_PUBLIC = 10,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_PUBLIC = 10,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_EMOTE = 11,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_EMOTE = 11,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_PERSONAL_MESSAGE = 13,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_PERSONAL_MESSAGE = 13,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_FASTPING = 14,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_FASTPING = 14,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_JOIN = 20,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_PVT = 25,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_PVT = 25,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_ISIGNORINGYOU = 26,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_OFFLINEUSER = 27,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_PART = 22,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_CHANNEL_USER_LIST = 30,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_TOPIC = 31,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_TOPIC_FIRST = 32,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_CHANNEL_USER_LIST_END = 35,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_NOSUCH = 44,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_IGNORELIST = 45,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_ADDSHARE = 50,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_REMSHARE = 51,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_BROWSE = 52,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_ENDOFBROWSE = 53,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_BROWSEERROR = 54,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_BROWSEITEM = 55,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_STARTOFBROWSE = 56,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_SEARCH = 60,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_SEARCHHIT = 61,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_ENDOFSEARCH = 62,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_DUMMY = 64,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_SEND_SUPERNODES = 70,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_HERE_SUPERNODES = 70,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_DIRCHATPUSH = 72,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_URL = 73,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_COMMAND = 74,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_OPCHANGE = 75,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENTCOMPRESSED = 80,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_AUTHLOGIN = 82,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_AUTHREGISTER = 83,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_MYFEATURES = 92,
        /// <summary>protocol message</summary>
        MSG_SERVER_LINK_REQ = 100,
        /// <summary>protocol message</summary>
        MSG_SERVER_LINK_ACK = 101,
        /// <summary>protocol message</summary>
        MSG_SERVER_LINK_ERROR = 102,
        /// <summary>protocol message</summary>
        MSG_SERVER_BROADCAST = 103,
        /// <summary>protocol message</summary>
        MSG_SERVER_RELAYTOUSER = 104,
        /// <summary>protocol message</summary>
        MSG_SERVER_CLOAK = 105,
        /// <summary>protocol message</summary>
        MSG_SERVER_NEWLINK = 106,
        /// <summary>protocol message</summary>
        MSG_SERVER_PONG = 107,
        /// <summary>protocol message</summary>
        MSG_HUB_TOSERVER_LOGINREQ = 110,
        /// <summary>protocol message</summary>
        MSG_HUB_TOSERVER_LOGINACK = 111,
        /// <summary>protocol message</summary>
        MSG_SERVER_TOHUB_LOGINREQ = 112,
        /// <summary>protocol message</summary>
        MSG_SERVER_TOHUB_LOGINACK = 113,

        /// <summary>protocol message</summary>
        MSG_SERVER_ADMIN_COMMAND = 120,
        /// <summary>protocol message</summary>
        MSG_SERVER_ADMIN_DISABLED = 121,
        /// <summary>protocol message</summary>
        MSG_SERVER_ADMIN_MESSAGE = 122,
        /// <summary>protocol message</summary>
        MSG_SERVER_SERVER_COMMAND = 123,
        /// <summary>protocol message</summary>
        MSG_SERVER_TEXT_RECEIVED = 124,
        /// <summary>protocol message</summary>
        MSG_SERVER_EMOTE_RECEIVED = 125,
        /// <summary>protocol message</summary>
        MSG_SERVER_FORCE_PM = 126,
        /// <summary>protocol message</summary>
        MSG_SERVER_CUSTOM_SET_TAGS = 127,
        /// <summary>protocol message</summary>
        MSG_SERVER_CUSTOM_NAME_TEXT = 128,

        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_CUSTOM_DATA = 200,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_CUSTOM_DATA = 200,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_CUSTOM_DATA_ALL = 201,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_CUSTOM_ADD_TAGS = 202,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_CUSTOM_REM_TAGS = 203,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_CUSTOM_FONT = 204,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_CUSTOM_FONT = 204,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_VC_SUPPORTED = 205,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_VC_SUPPORTED = 205,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_VC_FIRST = 206,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_VC_FIRST = 206,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_VC_FIRST_FROM = 207,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_VC_FIRST_TO = 207,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_VC_CHUNK = 208,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_VC_CHUNK = 208,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_VC_CHUNK_FROM = 209,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_VC_CHUNK_TO = 209,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_VC_IGNORE = 210,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_VC_IGNORE = 210,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_VC_NOPVT = 211,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_VC_USER_SUPPORTED = 212,
        /// <summary>protocol message</summary>
        MSG_CHAT_ADVANCED_FEATURES_PROTOCOL = 250,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_SUPPORTS_CUSTOM_EMOTES = 220,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_SUPPORTS_CUSTOM_EMOTES = 220,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_CUSTOM_EMOTES_ITEM = 221,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_CUSTOM_EMOTES_UPLOAD_ITEM = 221,
        /// <summary>protocol message</summary>
        MSG_CHAT_SERVER_CUSTOM_EMOTE_DELETE = 222,
        /// <summary>protocol message</summary>
        MSG_CHAT_CLIENT_CUSTOM_EMOTE_DELETE = 222,
        /// <summary>protocol message</summary>
        UNKNOWN = 255
    }
}
