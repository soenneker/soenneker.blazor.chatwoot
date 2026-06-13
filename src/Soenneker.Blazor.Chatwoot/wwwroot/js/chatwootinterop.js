const chatwootInstances = {};
const eventHandlersByElementId = {};
const widgetStateObserversByElementId = {};

const widgetFrameSelector = [
    ".woot-widget-holder",
    "#woot-widget-holder",
    "iframe.woot-widget-holder"
].join(",");

const widgetBubbleSelector = [
    ".woot-widget-bubble",
    "#woot-widget-bubble",
    ".woot--bubble-holder"
].join(",");

export function init(elementId, options, dotNetCallback) {
    if (chatwootInstances[elementId]?.isLoaded)
        return;

    chatwootInstances[elementId] = {
        dotNetCallback,
        isLoaded: true,
        isOpen: false,
        options
    };

    window.chatwootSettings = options;

    attachEvents(elementId);

    window.chatwootSDK.run(window.chatwootSettings);
    applyWidgetLayer(options);
    createWidgetStateObserver(elementId);
    setWidgetPointerEvents(elementId, false);
}

function applyWidgetLayer(options) {
    const zIndex = Number.isFinite(options?.widgetZIndex) ? options.widgetZIndex : 40;
    const styleId = "soenneker-chatwoot-widget-layer";
    const css = `
        .woot-widget-bubble,
        .woot-widget-holder,
        #woot-widget-holder,
        iframe.woot-widget-holder {
            z-index: ${zIndex} !important;
        }
    `;

    let style = document.getElementById(styleId);

    if (!style) {
        style = document.createElement("style");
        style.id = styleId;
        document.head.appendChild(style);
    }

    style.textContent = css;
}

function attachEvents(elementId) {
    const map = {
        "chatwoot:ready": "OnReadyCallback",
        "chatwoot:open": "OnOpenCallback",
        "chatwoot:close": "OnCloseCallback",
        "chatwoot:on-message": "OnMessageCallback",
        "chatwoot:error": "OnErrorCallback",
    };

    const cwState = chatwootInstances[elementId];

    if (!cwState)
        return;

    eventHandlersByElementId[elementId] = {};

    Object.keys(map).forEach(eventName => {
        const handler = (event) => {
            const payload = event?.detail ?? null;
            updateWidgetStateFromEvent(elementId, eventName);

            try {
                cwState.dotNetCallback?.invokeMethodAsync(map[eventName], payload);
            } catch (err) {
                console.warn("Chatwoot callback error:", err);
            }
        };

        eventHandlersByElementId[elementId][eventName] = handler;
        window.addEventListener(eventName, handler);
    });
}

function updateWidgetStateFromEvent(elementId, eventName) {
    const cwState = chatwootInstances[elementId];

    if (!cwState)
        return;

    if (eventName === "chatwoot:open") {
        cwState.isOpen = true;
        setWidgetPointerEvents(elementId, true);
        return;
    }

    if (eventName === "chatwoot:close" || eventName === "chatwoot:error") {
        cwState.isOpen = false;
        setWidgetPointerEvents(elementId, false);
    }
}

function setWidgetPointerEvents(elementId, enabled) {
    const options = chatwootInstances[elementId]?.options;

    document.querySelectorAll(widgetFrameSelector).forEach(element => {
        element.style.pointerEvents = enabled ? "" : "none";
    });

    if (options?.hideMessageBubble) {
        document.querySelectorAll(widgetBubbleSelector).forEach(element => {
            element.style.pointerEvents = enabled ? "" : "none";
        });
    }
}

function isHidden(element) {
    const style = window.getComputedStyle(element);
    return style.display === "none" || style.visibility === "hidden" || style.opacity === "0";
}

function reconcileWidgetPointerEvents(elementId) {
    const cwState = chatwootInstances[elementId];

    if (!cwState)
        return;

    const frame = document.querySelector(widgetFrameSelector);

    if (cwState.isOpen && frame && isHidden(frame))
        cwState.isOpen = false;

    setWidgetPointerEvents(elementId, cwState.isOpen);
}

function createWidgetStateObserver(elementId) {
    if (widgetStateObserversByElementId[elementId] || !document.body)
        return;

    const observer = new MutationObserver(() => reconcileWidgetPointerEvents(elementId));
    observer.observe(document.body, {
        childList: true,
        subtree: true,
        attributes: true,
        attributeFilter: ["class", "style"]
    });

    widgetStateObserversByElementId[elementId] = observer;
}

export function shutdown(elementId) {
    const handlers = eventHandlersByElementId[elementId];

    if (handlers) {
        Object.keys(handlers).forEach(eventName => {
            window.removeEventListener(eventName, handlers[eventName]);
        });
        delete eventHandlersByElementId[elementId];
    }

    const cwState = chatwootInstances[elementId];

    setWidgetPointerEvents(elementId, false);
    window.$chatwoot?.reset();

    if (cwState?.observer) {
        cwState.observer.disconnect();
    }

    widgetStateObserversByElementId[elementId]?.disconnect();
    delete widgetStateObserversByElementId[elementId];

    delete chatwootInstances[elementId];
}

export function toggle(elementId) {
    if (chatwootInstances[elementId]?.isLoaded) {
        window.$chatwoot?.toggle();
    }
}

export function setUser(elementId, identifier, attributesJson) {
    const attributes = JSON.parse(attributesJson);
    if (chatwootInstances[elementId]?.isLoaded) {
        window.$chatwoot?.setUser(identifier, attributes);
    }
}

export function setUserAttributes(elementId, attributesJson) {
    const attributes = JSON.parse(attributesJson);
    if (chatwootInstances[elementId]?.isLoaded) {
        window.$chatwoot?.setUserAttributes(attributes);
    }
}

export function setLabel(elementId, label) {
    if (chatwootInstances[elementId]?.isLoaded) {
        window.$chatwoot?.setLabel(label);
    }
}

export function removeLabel(elementId, label) {
    if (chatwootInstances[elementId]?.isLoaded) {
        window.$chatwoot?.removeLabel(label);
    }
}

export function setLocale(elementId, locale) {
    if (chatwootInstances[elementId]?.isLoaded) {
        window.$chatwoot?.setLocale(locale);
    }
}

export function deleteCustomAttribute(elementId, key) {
    if (chatwootInstances[elementId]?.isLoaded) {
        window.$chatwoot?.deleteCustomAttribute(key);
    }
}

export function reset(elementId) {
    if (chatwootInstances[elementId]?.isLoaded) {
        window.$chatwoot?.reset();
    }
}

export function setCustomAttributes(elementId, attributesJson) {
    const attributes = JSON.parse(attributesJson);
    if (chatwootInstances[elementId]?.isLoaded) {
        window.$chatwoot?.setCustomAttributes(attributes);
    }
}

export function popoutChatWindow(elementId) {
    if (chatwootInstances[elementId]?.isLoaded) {
        window.$chatwoot?.popoutChatWindow();
    }
}

export function createObserver(elementId) {
    const target = document.getElementById(elementId);

    if (!target || !target.parentNode)
        return null;

    const observer = new MutationObserver((mutations) => {
        const removed = mutations.some(m =>
            Array.from(m.removedNodes).includes(target)
        );

        if (removed) {
            shutdown(elementId);
        }
    });

    observer.observe(target.parentNode, { childList: true });

    if (chatwootInstances[elementId]) {
        chatwootInstances[elementId].observer = observer;
    }

    return observer;
}
