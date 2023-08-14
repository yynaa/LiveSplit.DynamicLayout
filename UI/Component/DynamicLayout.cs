using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Fleck;

namespace LiveSplit.UI.Components
{
    public class DynamicLayout : IComponent
    {
        public DynamicLayoutSettings Settings { get; set; }
        protected LiveSplitState CurrentState { get; set; }

        public string ComponentName => "Dynamic Layout";

        public float HorizontalWidth => 0f;
        public float MinimumWidth => 0f;
        public float VerticalHeight => 0f;
        public float MinimumHeight => 0f;

        public float PaddingTop => 0f;
        public float PaddingLeft => 0f;
        public float PaddingBottom => 0f;
        public float PaddingRight => 0f;

        public IDictionary<string, Action> ContextMenuControls => null;

        //CODE HERE

        private WebSocketServer server;
        private string serverIP = "ws://0.0.0.0";

        private string _sMC = "█"; //split message character
        private char _sMCchar = '█'; 

        private LiveSplitState state;
        private List<IWebSocketConnection> sockets;

        public DynamicLayout(LiveSplitState newState)
        {
			sockets = new List<IWebSocketConnection>();
            Settings = new DynamicLayoutSettings(restartServer);

            newState.OnSplit += state_OnSplit;
            newState.OnSkipSplit += state_OnSkipSplit;
            newState.OnUndoSplit += state_OnUndoSplit;
            newState.OnReset += state_OnReset;

            state = newState;
			startServer();
        }

		public void startServer() {
			server = new WebSocketServer(serverIP + ":" + Settings.Port);
			server.Start(newsocket =>
			{
				sockets.Add(newsocket);
				newsocket.OnMessage = message => OnMessage(newsocket, message);
				newsocket.OnClose = () => sockets.Remove(newsocket);
			});
		}

		public void restartServer() {
			server.Dispose();
			startServer();
		}

        public void Dispose()
        { 
            server.Dispose();
        }

        public void Update(IInvalidator invalidator, LiveSplitState newState, float width, float height, LayoutMode mode) {
            state = newState;
        }

        //WEBSOCKET EVENTS

        private void OnMessage(IWebSocketConnection socket, string message)
        {
            var split = message.Split(_sMCchar);

            if (split.Length > 0)
            {
                switch (split[0])
                {
                    case "update":
                        UpdateSocket(socket);
                        break;
                }
                    
            } 
        }

        //LIVESPLIT EVENTS

        public void state_OnSplit(object sender, EventArgs e)
        {
            sockets.ForEach(s => s.Send("split" + _sMC + formatSplitSend()));
        }

        public string formatSplitSend() {
            string splitColor = HexConverter(LiveSplitStateHelper.GetSplitColor(state, state.CurrentTime[state.CurrentTimingMethod] - state.Run[state.CurrentSplitIndex - 1].Comparisons[state.CurrentComparison][state.CurrentTimingMethod],
                        state.CurrentSplitIndex - 1, true, false, state.CurrentComparison, state.CurrentTimingMethod).GetValueOrDefault());
            string delta = DeltaFormatter(LiveSplitStateHelper.GetLastDelta(state, state.CurrentSplitIndex - 1, state.CurrentComparison, state.CurrentTimingMethod).GetValueOrDefault());

            if (splitColor == "#000000")
            {
                delta = "-";
                splitColor = HexConverter(state.Layout.Settings.NotRunningColor);
            }

			return state.Run[state.CurrentSplitIndex - 1].Name
				+ _sMC + TimeFormatter(state.CurrentTime[state.CurrentTimingMethod].GetValueOrDefault())
				+ _sMC + delta
				+ _sMC + splitColor;
        }

        public void state_OnSkipSplit(object sender, EventArgs e)
		{
			string msg = "split"
				+ _sMC + state.Run[state.CurrentSplitIndex - 1].Name
				+ _sMC + "-"
				+ _sMC + "-"
				+ _sMC + HexConverter(state.Layout.Settings.NotRunningColor);

			sockets.ForEach(s => s.Send(msg));
        }

        public void state_OnUndoSplit(object sender, EventArgs e)
		{
			sockets.ForEach(s => s.Send("undo"));
		}

        public void state_OnReset(object sender, TimerPhase e)
        {
			sockets.ForEach(s => s.Send("reset"));
		}

        //RESPONSES

        private void UpdateSocket(IWebSocketConnection socket)
        {
            TimeSpan pb = TimeSpan.Zero;
            string pbFormat = "n/a";
            if (state.Run.Count() > 0)
            {
                ISegment segment = state.Run[state.Run.Count() - 1];
                pb = segment.PersonalBestSplitTime[state.CurrentTimingMethod].GetValueOrDefault();
            }
            if (pb != null && pb != TimeSpan.Zero)
            {
                pbFormat = TimeFormatter(pb);
            }

            TimeSpan sob = TimeSpan.Zero;
            string sobFormat = "n/a";
            if (state.Run.Count() > 0)
            {
                ISegment segment = state.Run[state.Run.Count() - 1];
                sob = segment.Comparisons["Best Segments"][state.CurrentTimingMethod].GetValueOrDefault();
            }
            if (sob != null && sob != TimeSpan.Zero)
            {
                sobFormat = TimeFormatter(sob);
            }

            string splitColorFormatted = HexConverter(state.Layout.Settings.NotRunningColor);

            if (state.CurrentSplitIndex != -1)
            {
                if (state.CurrentSplitIndex == state.Run.Count)
                {
                    if (LiveSplitStateHelper.GetLastDelta(state, state.CurrentSplitIndex - 1, state.CurrentComparison, state.CurrentTimingMethod).GetValueOrDefault() < TimeSpan.Zero){
                        splitColorFormatted = HexConverter(state.Layout.Settings.PersonalBestColor);
                    }
                }
                else
                {
                    Color? splitColor;
                    if (state.CurrentSplit.Comparisons[state.CurrentComparison][state.CurrentTimingMethod] != null)
                    {
                        splitColor = LiveSplitStateHelper.GetSplitColor(state, state.CurrentTime[state.CurrentTimingMethod] - state.CurrentSplit.Comparisons[state.CurrentComparison][state.CurrentTimingMethod],
                            state.CurrentSplitIndex, true, false, state.CurrentComparison, state.CurrentTimingMethod)
                            ?? state.LayoutSettings.AheadGainingTimeColor;
                    }
                    else
                        splitColor = state.LayoutSettings.AheadGainingTimeColor;
                    splitColorFormatted = HexConverter(splitColor.GetValueOrDefault());
                }
                
            }

            socket.Send("update" 
                + _sMC + pbFormat
                + _sMC + sobFormat
                + _sMC + TimeFormatter(state.CurrentTime[state.CurrentTimingMethod].GetValueOrDefault())
                + _sMC + state.CurrentTime[state.CurrentTimingMethod].GetValueOrDefault().ToString(@"ff")
                + _sMC + splitColorFormatted
            );
        }

        //MISC

        private string TimeFormatter(TimeSpan time)
        {
            if (time < new TimeSpan(1, 0, 0))
            {
                return time.ToString(@"m\:ss");
            }
            else
            {
                return time.ToString(@"h\:mm\:ss");
            }
        }

        private string DeltaFormatter(TimeSpan delta)
        {
            var s = "+";
            var time = delta;
            if (time < TimeSpan.Zero) { time = time.Negate(); s = "-"; }
            if (time < new TimeSpan(0, 0, 10))
            {
                s += time.ToString(@"s\.ff");
            }
            else if (time < new TimeSpan(0, 1, 0))
            {
                s += time.ToString(@"ss\.f");
            }
            else if (time < new TimeSpan(1, 0, 0))
            {
                s += time.ToString(@"m\:ss");
            }
            else
            {
                s += time.ToString(@"h\:mm\:ss");
            }
            return s;
        }

        private static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        //CODE END

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion) { }
        public void DrawVertical(Graphics g, LiveSplitState state, float height, Region clipRegion) { }

        public Control GetSettingsControl(LayoutMode mode)
        {
            Settings.Mode = mode;
            return Settings;
        }

        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            return Settings.GetSettings(document);
        }

        public void SetSettings(System.Xml.XmlNode settings)
        {
            Settings.SetSettings(settings);
        }
        public int GetSettingsHashCode() => Settings.GetSettingsHashCode();
    }
}
