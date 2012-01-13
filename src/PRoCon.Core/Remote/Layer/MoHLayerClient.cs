using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Core.Remote.Layer {
    public class MoHLayerClient : FrostbiteLayerClient {

        public MoHLayerClient(FrostbiteLayerConnection connection)
            : base(connection) {

            this.m_requestDelegates.Add("vars.clanTeams", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.noAmmoPickups", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.noCrosshairs", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.noSpotting", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.noUnlocks", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.realisticHealth", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.skillLimit", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.preRoundLimit", this.DispatchVarsRequest);

            this.m_requestDelegates.Add("admin.stopPreRound", this.DispatchUseMapFunctionRequest);

            this.m_requestDelegates.Add("admin.roundStartTimerEnabled", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.tdmScoreCounterMaxScore", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.roundStartTimerPlayersLimit", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.roundStartTimerDelay", this.DispatchVarsRequest);
            
            this.m_requestDelegates.Add("reservedSpectateSlots.configFile", this.DispatchAlterReservedSlotsListRequest);
            this.m_requestDelegates.Add("reservedSpectateSlots.load", this.DispatchAlterReservedSlotsListRequest);
            this.m_requestDelegates.Add("reservedSpectateSlots.save", this.DispatchAlterReservedSlotsListRequest);
            this.m_requestDelegates.Add("reservedSpectateSlots.addPlayer", this.DispatchAlterReservedSlotsListRequest);
            this.m_requestDelegates.Add("reservedSpectateSlots.removePlayer", this.DispatchAlterReservedSlotsListRequest);
            this.m_requestDelegates.Add("reservedSpectateSlots.clear", this.DispatchAlterReservedSlotsListRequest);
            this.m_requestDelegates.Add("reservedSpectateSlots.list", this.DispatchSecureSafeListedRequest);
        }

    }
}
