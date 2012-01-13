using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Core.Remote.Layer {
    public class BFBC2LayerClient : FrostbiteLayerClient {

        public BFBC2LayerClient(FrostbiteLayerConnection connection)
            : base(connection) {

            this.m_requestDelegates.Add("vars.killCam", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.miniMap", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.crossHair", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.3dSpotting", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.miniMapSpotting", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.thirdPersonVehicleCameras", this.DispatchVarsRequest);
            this.m_requestDelegates.Add("vars.teamBalance", this.DispatchVarsRequest);

            this.m_requestDelegates.Add("reservedSlots.configFile", this.DispatchAlterReservedSlotsListRequest);
            this.m_requestDelegates.Add("reservedSlots.load", this.DispatchAlterReservedSlotsListRequest);
            this.m_requestDelegates.Add("reservedSlots.save", this.DispatchAlterReservedSlotsListRequest);
            this.m_requestDelegates.Add("reservedSlots.addPlayer", this.DispatchAlterReservedSlotsListRequest);
            this.m_requestDelegates.Add("reservedSlots.removePlayer", this.DispatchAlterReservedSlotsListRequest);
            this.m_requestDelegates.Add("reservedSlots.clear", this.DispatchAlterReservedSlotsListRequest);
            this.m_requestDelegates.Add("reservedSlots.list", this.DispatchSecureSafeListedRequest);
        }
    }
}
