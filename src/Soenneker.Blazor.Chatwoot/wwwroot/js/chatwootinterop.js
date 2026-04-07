const chatwootInstances = {};
const eventHandlersByElementId = {};

export function init(elementId, options, dotNetCallback) {
    if (chatwootInstances[elementId]?.isLoaded)
        return;

    chatwootInstances[elementId] = {
        dotNetCallback,
        isLoaded: true
    };

    window.chatwootSettings = options;

    attachEvents(elementId);

    window.chatwootSDK.run(window.chatwootSettings);
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

export function shutdown(elementId) {
    const handlers = eventHandlersByElementId[elementId];

    if (handlers) {
        Object.keys(handlers).forEach(eventName => {
            window.removeEventListener(eventName, handlers[eventName]);
        });
        delete eventHandlersByElementId[elementId];
    }

    window.$chatwoot?.reset();

    const cwState = chatwootInstances[elementId];

    if (cwState?.observer) {
        cwState.observer.disconnect();
    }

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
