const chatwootInstances = {};
const eventHandlersByElementId = {};
const widgetStateObserversByElementId = {};

const widgetFrameSelector = [
    ".woot-widget-holder",
    "#woot-widget-holder",
    "iframe.woot-widget-holder",
    "iframe[src*='chatwoot']",
    "iframe[title*='Chat']",
    "iframe[title*='chat']"
].join(",");

const widgetBubbleSelector = [
    ".woot-widget-bubble",
    "#woot-widget-bubble",
    ".woot--bubble-holder",
    "[id^='woot-widget-bubble']",
    "[class*='woot-widget-bubble']",
    "[class*='woot--bubble']"
].join(",");

export function init(elementId, options, dotNetCallback) {
    if (chatwootInstances[elementId]?.isLoaded)
        return;

    if (Object.keys(chatwootInstances).length > 0)
        throw new Error("Only one Chatwoot widget can be active on a page.");

    chatwootInstances[elementId] = {
        dotNetCallback,
        isOpening: false,
        isLoaded: true,
        isOpen: false,
        isReady: false,
        openAttempt: 0,
        isStarted: false,
        wantsOpen: false,
        pendingCommands: [],
        options
    };

    window.chatwootSettings = options;

    attachEvents(elementId);

    applyWidgetLayer(options);
    if (options.deferUntilOpen !== false) {
        createDeferredLauncher(elementId);
    } else {
        startWidget(elementId);
    }
}

function createDeferredLauncher(elementId) {
    const state = chatwootInstances[elementId];
    if (state.options.hideMessageBubble)
        return;

    const launcher = document.createElement("button");
    launcher.type = "button";
    launcher.className = "soenneker-chatwoot-launcher";
    launcher.textContent = "Chat with us";
    launcher.setAttribute("aria-label", "Open chat");
    launcher.style.cssText = `position:fixed;bottom:20px;${state.options.position === "left" ? "left" : "right"}:20px;z-index:${Number.isFinite(state.options.widgetZIndex) ? state.options.widgetZIndex : 40};border:0;border-radius:28px;padding:16px 20px;background:#1f93ff;color:white;font:600 14px system-ui,sans-serif;cursor:pointer;box-shadow:0 4px 12px #0003`;
    launcher.addEventListener("click", () => open(elementId));
    document.body.appendChild(launcher);
    state.launcher = launcher;
}

function startWidget(elementId) {
    const state = chatwootInstances[elementId];
    if (!state || state.isStarted)
        return;

    window.chatwootSDK.run(state.options);
    state.isStarted = true;
    for (const [method, args] of state.pendingCommands)
        window.$chatwoot?.[method]?.(...args);
    state.pendingCommands.length = 0;
    createWidgetStateObserver(elementId);
    setWidgetPointerEvents(elementId, false);
}

function invokeOrQueue(elementId, method, ...args) {
    const state = chatwootInstances[elementId];
    if (!state)
        return;
    if (state.isStarted)
        window.$chatwoot?.[method]?.(...args);
    else
        state.pendingCommands.push([method, args]);
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

            const args = eventName === "chatwoot:on-message" || eventName === "chatwoot:error" ? [payload] : [];
            cwState.dotNetCallback
                ?.invokeMethodAsync(map[eventName], ...args)
                .catch(err => console.warn("Chatwoot callback error:", err));
        };

        eventHandlersByElementId[elementId][eventName] = handler;
        window.addEventListener(eventName, handler);
    });
}

function updateWidgetStateFromEvent(elementId, eventName) {
    const cwState = chatwootInstances[elementId];

    if (!cwState)
        return;

    if (eventName === "chatwoot:ready") {
        cwState.isReady = true;
        cwState.launcher?.remove();
        cwState.launcher = null;
        createWidgetStateObserver(elementId);
        if (cwState.wantsOpen)
            tryOpenWidget(elementId);
        return;
    }

    if (eventName === "chatwoot:open") {
        cwState.wantsOpen = true;
        cwState.isOpen = true;
        cwState.isOpening = false;
        cwState.isReady = true;
        setWidgetPointerEvents(elementId, true);
        return;
    }

    if (eventName === "chatwoot:close" || eventName === "chatwoot:error") {
        cwState.wantsOpen = false;
        cwState.isOpen = false;
        cwState.isOpening = false;
        setWidgetPointerEvents(elementId, false);
    }
}

function setWidgetPointerEvents(elementId, enabled) {
    const options = chatwootInstances[elementId]?.options;

    const pointerEvents = enabled ? "" : "none";

    for (const element of document.querySelectorAll(widgetFrameSelector)) {
        if (element.style.pointerEvents !== pointerEvents)
            element.style.pointerEvents = pointerEvents;
    }

    if (options?.hideMessageBubble) {
        for (const element of document.querySelectorAll(widgetBubbleSelector)) {
            if (element.style.pointerEvents !== pointerEvents)
                element.style.pointerEvents = pointerEvents;
        }
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

    if (cwState.isOpening)
        return;

    const frame = document.querySelector(widgetFrameSelector);

    // Current SDK releases expose isOpen but do not always dispatch open/close
    // DOM events (including when their own launcher is used).
    if (typeof window.$chatwoot?.isOpen === "boolean")
        cwState.isOpen = window.$chatwoot.isOpen;
    if (cwState.isOpen && frame && isHidden(frame))
        cwState.isOpen = false;

    setWidgetPointerEvents(elementId, cwState.isOpen);
}

function createWidgetStateObserver(elementId) {
    if (widgetStateObserversByElementId[elementId] || !document.body)
        return;

    // Blazor updates classes/styles throughout the page. Those mutations must not
    // trigger document-wide selectors and computed-style reads for the chat widget.
    const selector = `${widgetFrameSelector},${widgetBubbleSelector}`;
    const affectsWidget = node => node.nodeType === 1 &&
        (node.matches(selector) || !!node.querySelector(selector));
    const observer = new MutationObserver(records => {
        const relevant = records.some(record => record.type === "attributes"
            ? affectsWidget(record.target)
            : [...record.addedNodes, ...record.removedNodes].some(affectsWidget));
        if (relevant)
            reconcileWidgetPointerEvents(elementId);
    });
    observer.observe(document.body, {
        childList: true,
        subtree: true,
        attributes: true,
        attributeFilter: ["class", "style"]
    });

    widgetStateObserversByElementId[elementId] = observer;
}

function scheduleWidgetReconcile(elementId) {
    const state = chatwootInstances[elementId];
    window.setTimeout(() => {
        const cwState = chatwootInstances[elementId];

        if (!cwState || cwState !== state)
            return;

        cwState.isOpening = false;
        reconcileWidgetPointerEvents(elementId);
    }, 1500);
}

function tryOpenWidget(elementId) {
    const cwState = chatwootInstances[elementId];

    if (!cwState?.isReady || !cwState.wantsOpen || !window.$chatwoot?.toggle)
        return false;

    setWidgetPointerEvents(elementId, true);
    cwState.isOpening = true;
    window.$chatwoot.toggle("open");
    cwState.isOpen = window.$chatwoot.isOpen === true;
    scheduleWidgetReconcile(elementId);

    return true;
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
    if (cwState?.isStarted)
        window.$chatwoot?.reset();
    cwState?.launcher?.remove();

    if (cwState?.observer) {
        cwState.observer.disconnect();
    }

    widgetStateObserversByElementId[elementId]?.disconnect();
    delete widgetStateObserversByElementId[elementId];

    delete chatwootInstances[elementId];
}

export function toggle(elementId) {
    const state = chatwootInstances[elementId];
    if (state?.wantsOpen || state?.isOpen)
        close(elementId);
    else
        open(elementId);
}

export function open(elementId) {
    const cwState = chatwootInstances[elementId];

    if (!cwState?.isLoaded)
        return;

    cwState.wantsOpen = true;
    if (!cwState.isStarted) {
        if (cwState.startScheduled)
            return;
        cwState.startScheduled = true;
        if (cwState.launcher) {
            cwState.launcher.textContent = "Opening chat…";
            cwState.launcher.setAttribute("aria-busy", "true");
        }
        // Allow the launcher feedback to paint before starting third-party work.
        window.requestAnimationFrame(() => window.setTimeout(() => {
            if (chatwootInstances[elementId] !== cwState)
                return;
            cwState.startScheduled = false;
            if (!cwState.wantsOpen)
                return;
            try {
                startWidget(elementId);
            } catch (error) {
                cwState.wantsOpen = false;
                if (cwState.launcher) {
                    cwState.launcher.textContent = "Retry chat";
                    cwState.launcher.removeAttribute("aria-busy");
                }
                cwState.dotNetCallback?.invokeMethodAsync("OnErrorCallback", { message: String(error) })
                    .catch(err => console.warn("Chatwoot callback error:", err));
            }
        }, 0));
        return;
    }

    const attemptId = ++cwState.openAttempt;
    createWidgetStateObserver(elementId);

    if (tryOpenWidget(elementId))
        return;

    let attempts = 0;
    const interval = window.setInterval(() => {
        if (chatwootInstances[elementId] !== cwState || attemptId !== cwState.openAttempt || !cwState.wantsOpen) {
            window.clearInterval(interval);
            return;
        }
        attempts += 1;

        if (tryOpenWidget(elementId) || attempts >= 20) {
            const latestState = chatwootInstances[elementId];

            if (attempts >= 20 && latestState && !latestState.isOpen && attemptId === latestState.openAttempt) {
                latestState.isOpening = false;
                setWidgetPointerEvents(elementId, false);
            }

            window.clearInterval(interval);
        }
    }, 100);
}

export function close(elementId) {
    const cwState = chatwootInstances[elementId];

    if (!cwState?.isLoaded)
        return;

    cwState.isOpen = false;
    cwState.isOpening = false;
    cwState.wantsOpen = false;
    cwState.openAttempt++;
    if (cwState.launcher) {
        cwState.launcher.textContent = "Chat with us";
        cwState.launcher.removeAttribute("aria-busy");
    }

    if (window.$chatwoot?.toggle) {
        window.$chatwoot.toggle("close");
    }

    setWidgetPointerEvents(elementId, false);
}

export function setUser(elementId, identifier, attributes) {
    invokeOrQueue(elementId, "setUser", identifier, attributes);
}

export function setUserAttributes(elementId, attributes) {
    invokeOrQueue(elementId, "setUserAttributes", attributes);
}

export function setLabel(elementId, label) {
    invokeOrQueue(elementId, "setLabel", label);
}

export function removeLabel(elementId, label) {
    invokeOrQueue(elementId, "removeLabel", label);
}

export function setLocale(elementId, locale) {
    invokeOrQueue(elementId, "setLocale", locale);
}

export function deleteCustomAttribute(elementId, key) {
    invokeOrQueue(elementId, "deleteCustomAttribute", key);
}

export function reset(elementId) {
    const state = chatwootInstances[elementId];
    if (state && !state.isStarted)
        state.pendingCommands.length = 0;
    invokeOrQueue(elementId, "reset");
}

export function setCustomAttributes(elementId, attributes) {
    invokeOrQueue(elementId, "setCustomAttributes", attributes);
}

export function popoutChatWindow(elementId) {
    // Preserve the user activation required by window.open.
    if (!chatwootInstances[elementId])
        return;
    startWidget(elementId);
    window.$chatwoot?.popoutChatWindow();
}

export function createObserver(elementId) {
    const target = document.getElementById(elementId);

    if (!target || !target.parentNode)
        return null;

    const observer = new MutationObserver((mutations) => {
        const removed = mutations.some(m =>
            Array.prototype.includes.call(m.removedNodes, target)
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
