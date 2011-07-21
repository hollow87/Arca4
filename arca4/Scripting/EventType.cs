using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace arca4.Scripting
{
	public enum EventType
	{
		OnTimer,
		OnFlood,
		OnJoinCheck,
		OnRejected,
		OnJoin,
		OnPart,
		OnAvatarReceived,
		OnPersonalMessageReceived,
		OnTextBefore,
		OnTextAfter,
		OnEmoteBefore,
		OnEmoteAfter,
		OnPM,
		OnBotPM,
		OnCommand,
		OnHelp,
		OnVroomJoinCheck,
		OnVroomJoin,
		OnVroomPart,
		OnFileReceived,
		OnLoginGranted,
		OnInvalidPassword
	}
}
