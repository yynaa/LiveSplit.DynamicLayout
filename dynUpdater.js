//SETTINGS
const websocketIP = "ws://127.0.0.1:8085"; //DO NOT CHANGE FOR NOW!!
const framerate = 15; //framerate
const splitsMaxAmount = 5; //maximum amount of splits shown on screen

//DO NOT EDIT
const sMC = "█"

console.log("trying to connect to " + websocketIP);
const websocket = new WebSocket(websocketIP);

websocket.addEventListener("open", (event) => {
    console.log("connected");
    interval = setInterval(socket_RequestUpdate, 1000 / framerate);
    setInterval(document_infoNext, 5000)
});

websocket.addEventListener("close", (event) => {
    console.log("disconnected");
    if (interval != null) { clearInterval(interval) };
    document.write("Disconnected.");
});

websocket.addEventListener("message", (event) => {
    var message = event.data;
    var messageArray = message.split(sMC);
    // console.log(messageArray);

    if (messageArray.length > 0) {
        switch (messageArray[0]) {
            case "update":
                document_UpdateInfo(messageArray[1], messageArray[2]);
                document_UpdateTimer(messageArray[3], messageArray[4], messageArray[5]);
                if (infoPanelPointer == -1) { document_infoNext(); }
                break;
            case "split":
                document_AddSplit(messageArray[1], messageArray[2], messageArray[3], messageArray[4])
                break;
            case "undo":
                document_UndoSplit();
                break;
            case "reset":
                document_ResetSplits()
                break;
        }
    }
})

function socket_RequestUpdate() {
    websocket.send("update");
}

infoStrings = ["PB", "SoB"];
timeStrings = ["n/a", "n/a"];

infoPanelPointer = -1;

function document_UpdateInfo(pb, sob) {
    timeStrings = [pb, sob];
}

function document_infoNext() {
    infoPanelPointer += 1
    if (infoPanelPointer == infoStrings.length) { infoPanelPointer = 0; }
    document.getElementById("info").innerHTML = infoStrings[infoPanelPointer];
    document.getElementById("infotime").innerHTML = timeStrings[infoPanelPointer];
}

function document_UpdateTimer(timer, ms, color) {
    var element = document.getElementById("timer");
    element.innerHTML = timer;
    element.style.color = color;
    var element = document.getElementById("timer-ms");
    element.innerHTML = "." + ms + "  "
    element.style.color = color;
}

splits = [];
splitAnimInterval = null;

function createElem(tag, classes, content = undefined, post_hook = undefined, children = []) {
    const elem = document.createElement(tag);
    for (const c of classes) {
        elem.classList.add(c);
    }
    if (content) elem.innerHTML = content;
    for (const ch of children) {
        elem.appendChild(ch);
    }
    if (post_hook) post_hook(elem);
    return elem;
}

function document_AddSplit(inputname, time, delta, color) {
    if (splits.length >= splitsMaxAmount) {
        var collection = document.getElementsByClassName("split-container")
        collection.item(0).remove();
        splits.shift();
    }

    var name = inputname
    const subsplit = name[0] == "-";
    if (subsplit) name = name.substring(1);

    //section name detection
    if (name.includes("{")) {
        name = name.substring(name.indexOf("{") + 1, name.indexOf("}"))
    }

    const newSplit = createElem("div", ["split-container"], undefined, undefined, [
        createElem("div", ["split-background"], undefined, undefined, [
            createElem("span", ["split-name"], name),
            createElem("div", ["split-time-background"], undefined, undefined, [
                createElem("span", ["split-time"], delta, (e) => e.style.color = color)
            ])
        ])
    ]);
    //subsplit detection
    if (subsplit) newSplit.classList.add("subsplit");

    document.getElementById("splits-container").appendChild(newSplit);
    splits[splits.length] = [name, time, delta, color];
}

function document_ResetSplits() {
    document.getElementById("splits-container").innerHTML = ""
    splits = []
}

function document_UndoSplit() {
    splits.pop();

    // Find newest split that is not in the process of animating out
    var collection = document.getElementsByClassName("split-container");
    const split = Array.prototype.findLast.call(collection, (e) => !e.classList.contains("split-del"));
    // Add delete anim and delete element on anim end
    split.classList.add("split-del");
    split.addEventListener("animationend", () => split.remove());
}

function anim_Filter(value) { return value == "split-container-anim-slideIn"; }

//MISC STUFF

function displayTextWidth(text, font) {
    let canvas = displayTextWidth.canvas || (displayTextWidth.canvas = document.createElement("canvas"));
    let context = canvas.getContext("2d");
    context.font = font;
    let metrics = context.measureText(text);
    return metrics.width;
}