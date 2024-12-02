﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http;
using System.Timers;
using vatsys;
using vatsys.Plugin;
using Newtonsoft.Json;

namespace FrequencyViewerPlugin
{
    [Export(typeof(IPlugin))]
    public class Plugin : ILabelPlugin
    {
        public string Name { get => "Frequency Viewer"; }
        private static List<Transceiver> Transceivers { get; set; } = new List<Transceiver>();
        private static readonly HttpClient httpClient = new HttpClient();
        private static Timer _timer;
        private static List<string[]> PreSelectOpData { get; set; } = new List<string[]>();
        private Track selectedTrack;

        private readonly string transceiversDataUrl = "https://data.vatsim.net/v3/transceivers-data.json";
        private bool networkErrorDisplayed = false;


        private void SelectedTrackChanged(object sender, EventArgs e)
        {
            if(MMI.SelectedTrack == null)
            {
                if (selectedTrack == null) return;

                var selTrackFdr = selectedTrack.GetFDR();
                if (selTrackFdr == null)
                {
                    selectedTrack = null;
                    return;
                }

                var opData = PreSelectOpData.Find(op => op[0] == selTrackFdr.Callsign);
                selTrackFdr.LocalOpData = opData[1];
                PreSelectOpData.Remove(opData);
                selectedTrack = null;

                return;
            };

            if(selectedTrack != null)
            {
                var oldTrackFdr = selectedTrack.GetFDR();
                if (oldTrackFdr == null) return;
                var opData = PreSelectOpData.Find(op => op[0] == oldTrackFdr.Callsign);

                oldTrackFdr.LocalOpData = opData[1];
                PreSelectOpData.Remove(opData);
                selectedTrack = null;
            }

            var newTrackFdr = MMI.SelectedTrack.GetFDR();
            if (newTrackFdr == null) return;

            var transceiver = Transceivers.Find(t => t.callsign == newTrackFdr.Callsign);
            if (transceiver != null)
            {
                selectedTrack = MMI.SelectedTrack;
                PreSelectOpData.Add(new string[] { newTrackFdr.Callsign, newTrackFdr.LocalOpData });
                newTrackFdr.LocalOpData = $"{string.Join(" ", transceiver.transceivers.Select(transceiverData => transceiverData.FreqMhz()).ToList())}";

                return;
            }
        }

        public async void UpdateTransceiverData(object sender, ElapsedEventArgs e)
        {
            try
            {
                var content = await httpClient.GetStringAsync(transceiversDataUrl);
                Transceivers = JsonConvert.DeserializeObject<List<Transceiver>>(content);
            } catch(Exception ex) {
                if (networkErrorDisplayed) return;
                networkErrorDisplayed = true;
                Errors.Add(new Exception($"An error occured when trying to fetch transceiver data! ${ex.Message}"), "Frequency Viewer");
            }
        }

        public Plugin()
        {
            _timer = new Timer(15000);
            _timer.Elapsed += UpdateTransceiverData;
            _timer.AutoReset = true;
            _timer.Start();

            MMI.SelectedTrackChanged += SelectedTrackChanged;
        }

        public CustomLabelItem GetCustomLabelItem(string itemType, Track track, FDP2.FDR flightDataRecord, RDP.RadarTrack radarTrack)
        {
            return null;
        }

        public void OnFDRUpdate(FDP2.FDR updated)
        {

        }

        public void OnRadarTrackUpdate(RDP.RadarTrack updated)
        {

        }

        public CustomColour SelectASDTrackColour(Track track)
        {
            return null;
        }

        public CustomColour SelectGroundTrackColour(Track track)
        {
            return null;
        }
    }
}