﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Core.Events {
    public enum CapturableEvents {

        // BFBC2 Server events
        PlayerJoin,
        PlayerLeave,
        PlayerKilled,
        PlayerKicked,
        IPKicked,
        PlayerTimedBanned,
        PlayerPermanentBanned,
        PlayerRoundBanned,
        PlayerUnbanned,
        IPTimedBanned,
        IPPermanentBanned,
        IPRoundBanned,
        IPUnbanned,
        GUIDTimedBanned,
        GUIDPermanentBanned,
        GUIDRoundBanned,
        GUIDUnbanned,

        LevelLoading,
        LevelStarted,

        // Playerlist events
        PlayerSuicide,
        PlayerSwitchedTeams,
        PlayerSwitchedSquads,
        PlayerTeamKilled,
        RoundChange,
        MapChange,

        PlayerKickedByAdmin,
        PlayerKilledByAdmin,
        PlayerMovedByAdmin,

        // Connection events
        AttemptingConnection,
        Connected,
        Disconnected,
        LostConnection,
        Reconnected,
        LoggedIn,
        LoggedOut,
        LoginFailure,

        // Plugins
        PluginLoaded,
        PluginEnabled,
        PluginDisabled,

        CompilingPlugins,
        RecompilingPlugins,
        PluginsCompiled,

        PluginAction,

        // Layer
        LayerOnline,
        LayerError,
        LayerOffline,
        AccountLogin,
        AccountLogout,

    }
}
