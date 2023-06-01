using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RoomKitModulePrototype
{
    public class EssentialsModule : LogicModule
    {
        private readonly List<IState> _states;

        public AudioMicrophonesMute AudioMicrophonesMute;
        public StandbyState StandbyState;
        public SpeakerTrack SpeakerTrack;

        public CiscoState SpeakerTrackState;

        //public CiscoValue CameraSelection;

        public EssentialsModule() 
        {
            AudioMicrophonesMute = new AudioMicrophonesMute(SendCommandToCodec);
            StandbyState = new StandbyState(SendCommandToCodec);
            SpeakerTrack = new SpeakerTrack(SendCommandToCodec);

            _states = new List<IState>{
                AudioMicrophonesMute,
                StandbyState,
                SpeakerTrack
            };


            ModuleType = "Essentials Module";


/*            _states.Add(SpeakerTrackState = new CiscoState()
            {
                Path = new string[] { "Cameras", "SpeakerTrack" },
                StatusArgument = "Status",
                FeedbackStates = new string[] { "Active", "Inactive" },
                ShouldRegisterFeedback = true,
                States = new string[] { "Activate", "Deactivate" },   
                SendCommandToCodecHandler = SendCommandToCodec
            });
            _states.Add(CameraSelection = new CiscoValue()
            {
                Path = new string[] { "Video", "Input" },
                StatusArgument = "MainVideoSource",
                FeedbackStates = new string[] {"1", "2", "3", "4", "5", "6" },
                ShouldRegisterFeedback = true,
                SetValueArgument = "SetMainVideoSource",
                ValueParameters = new CiscoParameter[]
                {
                    new CiscoParameter("ConnectorId", new string[] { "1", "2", "3", "4", "5", "6" })
                },
                SendCommandToCodecHandler = SendCommandToCodec
            });*/
        }
        protected override void ModulePropertiesBoot()
        {
            base.ModulePropertiesBoot();
            foreach(var state in _states)
            {
                if(state.IsRegisterFeedback)
                state.RegisterFeedback();
            }
        }
        public override void FromCommandModuleMessageReceived(object sender, InterModuleEventArgs args)
        {
            Thread.CurrentThread.DebugThreadID("Essentials Module Msg Received!");
            base.FromCommandModuleMessageReceived(sender, args);
            if (args.Message is XAPIStatusResponse xapiStatus)
            {
                foreach (var state in _states)
                {
                    state.CheckStatusResponse(xapiStatus);
                }
            }
        }
    }
}
