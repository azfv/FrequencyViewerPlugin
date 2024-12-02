# FrequencyViewerPlugin
A lightweight plugin for vatSys to show current frequencies of selected radar tracks.

---

### FAQ
**How does it work?**: The plugin fetches data from the public VATSIM API. The API updates every 30 seconds, and the plugin will request every 15 seconds.

**How do I see the frequencies?**: Once you designate a radar track, the correlated FDR (Flight Data Record), also known as a strip, will have it's local data replaced with the frequencies active - but don't worry! Your local data will be put back after undesignating the track.

**Why?**:
* Confirming monitor during Delivery -> Coordinator *or* Delivery -> Ground procedures
* Checking self-transfer to tower before handoff
* Easy coordination for a missing aircraft
* Forgot to handoff? Find out!

---

### Installation
Download the latest release on GitHub and install folder to vatSys Plugins folder: Usually `C:\Program Files (x86)\vatSys\bin\Plugins`.

---

### Bugs
Please contact me at **azfv** on Discord if you experience any problems.
