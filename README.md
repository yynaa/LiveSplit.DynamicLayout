# LiveSplit.DynamicLayout

An OBS layout for LiveSplit

# Setup
## LiveSplit
- Copy `Flack.dll` and `LiveSplit.DynamicLayout.dll` to your `LiveSplit/Components` folder
- Open LiveSplit, choose your layout
- Right-click on LiveSplit, click on `Edit Layout...`
- Add the `Dynamic Layout` component, located in `Other`
## OBS
- Create a new browser source
- Select `Local file` and browse to `dyn.html`
- Set size to `Width: 1000` and `Height: 500`

> [!IMPORTANT]
> You need to start LiveSplit with the Dynamic Layout before starting OBS (or the browser).
> A quick way to refresh the browser is to go in its properties, and press

> [!NOTE]
> You can set `Page permissions` to `No access to OBS` for better security
