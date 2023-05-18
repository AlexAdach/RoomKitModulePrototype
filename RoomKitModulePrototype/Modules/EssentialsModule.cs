using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class EssentialsModule : LogicModule
    {
        private readonly List<ICiscoStatus> _states;


        public CiscoState PrivacyMuteState;
        public CiscoState StandbyState;
        public CiscoState SpeakerTrackState;

        public CiscoValue CameraSelection;

        public EssentialsModule() 
        {
            ModuleType = "Essentials Module";
            _states = new List<ICiscoStatus>();

            _states.Add(PrivacyMuteState = new CiscoState()
            {
                Path = new string[] { "Audio", "Microphones" },
                States = new string[] { "Mute", "Unmute" },
                ShouldRegisterFeedback = true,
                StatusArgument = "Mute",
                FeedbackStates = new string[] { "On", "Off" },
                SendCommandToCodecHandler = SendCommandToCodec                
            });
            _states.Add(StandbyState = new CiscoState()
            {
                Path = new string[] {"Standby"},
                States = new string[] {"Activate", "Deactivate", "Halfwake"},
                ShouldRegisterFeedback = true,
                StatusArgument = "State",
                FeedbackStates = new string[] {"Standby", "Off", "Halfwake"},
                SendCommandToCodecHandler = SendCommandToCodec
            });

            _states.Add(SpeakerTrackState = new CiscoState()
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
            });
        }


        protected override void ModulePropertiesBoot()
        {
            base.ModulePropertiesBoot();
            foreach(var state in _states)
            {
                state.RegisterFeedback();
                state.GetState();
            }
        }

        public override void FromCommandModuleMessageReceived(object sender, InterModuleEventArgs args)
        {
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
