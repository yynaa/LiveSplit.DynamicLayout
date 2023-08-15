# LiveSplit.DynamicLayout

An OBS layout for LiveSplit

# Setup
## LiveSplit
- Copy `Fleck.dll` and `LiveSplit.DynamicLayout.dll` to your `LiveSplit/Components` folder
- Open LiveSplit, choose your layout
- Right-click on LiveSplit, click on `Edit Layout...`
- Add the `Dynamic Layout` component, located in `Other`
## OBS
- Extract `dyn.html`, `dynUpdater.js` and `style.css` anywhere you'd like, as long as all files are together 
- Create a new browser source
- Select `Local file` and browse to `dyn.html`
- Set size to `Width: 1000` and `Height: 1000`

> [!NOTE]
> DO NOT set `Page permissions` to `No access to OBS`, it may disable Javascript

# Settings
## Javascript
- `framerate`: pretty self-explanatory
- `splitsMaxAmount`: maximum amount of splits shown on screen

## CSS
see `style.css`
