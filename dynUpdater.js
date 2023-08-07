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

splitsMaxLength = 0;
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
    console.log(post_hook);
    if (post_hook) post_hook(elem);
    return elem;
}

function document_AddSplit(inputname, time, delta, color) {
    if (splits.length >= splitsMaxAmount) {
        var collection = document.getElementsByClassName("split-container")
        collection.item(0).remove();
        splits.shift();
    }
    var subsplitOffset = 0

    var name = inputname

    //subsplit detection
    if (name[0] == "-") {
        name = name.substring(1);
        subsplitOffset = 20
    }

    //section name detection
    if (name.includes("{")) {
        name = name.substring(name.indexOf("{") + 1, name.indexOf("}"))
    }

    var length = 90 + 15 * 2 + displayTextWidth(name, "400 18px Roboto");
    var leftBG = length - 90

    const newSplit = createElem("div", ["split-container", "split-container-anim-slideIn"], undefined, undefined, [
        createElem("div", ["split-background"], undefined, undefined, [
            createElem("span", ["split-name"], name),
            createElem("div", ["split-time-background"], undefined, undefined, [
                createElem("span", ["split-time"], delta, (e) => { console.log(e); e.style.color = color })
            ])
        ])
    ]);
    //subsplit detection
    if (name[0] == "-") {
        name = name.substring(1);
        newSplit.classList.add("subsplit");
    }

    document.getElementById("splits-container").appendChild(newSplit);

    if (length > splitsMaxLength) { splitsMaxLength = length; }
    splits[splits.length] = [name, time, delta, color];

    document.getElementById("splits-container").classList.add("splits-container-anim-slideIn");


    if (length > splitsMaxLength) { splitsMaxLength = length; }
    splits[splits.length] = [name, time, delta, color];

    clearInterval(splitAnimInterval)
    splitAnimInterval = setInterval(anim_ClearAllAnims, 1000)

    document.getElementById("splits-container").classList.add("splits-container-anim-slideIn");

    document_UpdateSplitLooks();
}

function document_ResetSplits() {
    clearInterval(splitAnimInterval)
    anim_ClearAllAnims()
    document.getElementById("splits-container").innerHTML = ""
    splits = []
    splitsMaxLength = 0;
}

function document_UndoSplit() {
    clearInterval(splitAnimInterval);
    anim_ClearAllAnims();

    splits.pop();

    splitsMaxLength = 0;
    var collection = document.getElementsByClassName("split-name")
    Array.from(collection).forEach(function (element) {
        var length = 90 + 15 * 2 + displayTextWidth(element.innerHTML, "400 18px Roboto");
        if (length > splitsMaxLength) { splitsMaxLength = length; }
    });
    var collection = document.getElementsByClassName("split-container")
    collection.item(collection.length - 1).remove();

    clearInterval(splitAnimInterval)
    splitAnimInterval = setInterval(anim_ClearAllAnims, 1000)

    document.getElementById("splits-container").classList.add("splits-container-anim-slideOut");

    document_UpdateSplitLooks();
}

function anim_Filter(value) { return value == "split-container-anim-slideIn"; }

function anim_ClearAllAnims() {
    clearInterval(splitAnimInterval)
    var collection = document.getElementsByClassName("split-container-anim-slideIn")
    Array.from(collection).forEach(function (element) {
        element.classList.remove("split-container-anim-slideIn");
    });
    document.getElementById("splits-container").classList.remove("splits-container-anim-slideIn");
    document.getElementById("splits-container").classList.remove("splits-container-anim-slideOut");
}

//MISC STUFF

function displayTextWidth(text, font) {
    let canvas = displayTextWidth.canvas || (displayTextWidth.canvas = document.createElement("canvas"));
    let context = canvas.getContext("2d");
    context.font = font;
    let metrics = context.measureText(text);
    return metrics.width;
}