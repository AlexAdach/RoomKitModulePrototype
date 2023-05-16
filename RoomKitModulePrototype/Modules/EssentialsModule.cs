using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class EssentialsModule : LogicModule
    {
        private readonly List<ICiscoStateControl> _states;

        public CiscoStateControl PrivacyMuteState;
        public CiscoStateControl StandbyState;
        public CiscoStateControl SpeakerTrackState;


        public EssentialsModule() 
        {
            ModuleType = "Essentials Module";
            _states = new List<ICiscoStateControl>();

            _states.Add(PrivacyMuteState = new CiscoStateControl()
            {
                Path = new string[] { "Audio", "Microphones" },
                States = new string[] { "Mute", "Unmute" },
                ShouldRegisterFeedback = true,
                StateArgument = "Mute",
                FeedbackStates = new string[] { "On", "Off" },
                SendCommandToCodecHandler = SendCommandToCodec                
            });
            _states.Add(StandbyState = new CiscoStateControl()
            {
                Path = new string[] {"Standby"},
                States = new string[] {"Activate", "Deactivate", "Halfwake"},
                ShouldRegisterFeedback = true,
                StateArgument = "State",
                FeedbackStates = new string[] {"Standby", "Off", "Halfwake"},
                SendCommandToCodecHandler = SendCommandToCodec
            });

            _states.Add(SpeakerTrackState = new CiscoStateControl()
            {
                Path = new string[] { "Cameras", "SpeakerTrack" },
                States = new string[] { "Activate", "Deactivate" },
                ShouldRegisterFeedback = true,
                StateArgument = "Status",
                FeedbackStates = new string[] { "Active", "Inactive" },
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
