const interop = (() => {
    const instance = {};
    instance.init = function(elementId, options, dotNetCallback) {
        if (this.instances[elementId]?.isLoaded)
            return;

        this.instances[elementId] = {
            dotNetCallback,
            isLoaded: true
        };

        window.chatwootSettings = options;

        this.attachEvents(elementId);

        window.chatwootSDK.run(window.chatwootSettings);
    };

    instance.attachEvents = function(elementId) {
        const map = {
            "chatwoot:ready": "OnReadyCallback",
            "chatwoot:open": "OnOpenCallback",
            "chatwoot:close": "OnCloseCallback",
            "chatwoot:on-message": "OnMessageCallback",
            "chatwoot:error": "OnErrorCallback",
        };

        const instance = this.instances[elementId];

        if (!instance)
            return;

        this._handlers[elementId] = {};

        Object.keys(map).forEach(eventName => {
            const handler = (event) => {
                const payload = event?.detail ?? null;
                try {
                    instance.dotNetCallback?.invokeMethodAsync(map[eventName], payload);
                } catch (err) {
                    console.warn("Chatwoot callback error:", err);
                }
            };

            this._handlers[elementId][eventName] = handler;
            window.addEventListener(eventName, handler);
        });
    };

    instance.shutdown = function(elementId) {
        const handlers = this._handlers[elementId];

        if (handlers) {
            Object.keys(handlers).forEach(eventName => {
                window.removeEventListener(eventName, handlers[eventName]);
            });
            delete this._handlers[elementId];
        }

        window.$chatwoot?.reset();

        const instance = this.instances[elementId];

        if (instance?.observer) {
            instance.observer.disconnect();
        }

        delete this.instances[elementId];
    };

    instance.toggle = function(elementId) {
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.toggle();
        }
    };

    instance.setUser = function(elementId, identifier, attributesJson) {
        const attributes = JSON.parse(attributesJson);
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.setUser(identifier, attributes);
        }
    };

    instance.setUserAttributes = function(elementId, attributesJson) {
        const attributes = JSON.parse(attributesJson);
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.setUserAttributes(attributes);
        }
    };

    instance.setLabel = function(elementId, label) {
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.setLabel(label);
        }
    };

    instance.removeLabel = function(elementId, label) {
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.removeLabel(label);
        }
    };

    instance.setLocale = function(elementId, locale) {
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.setLocale(locale);
        }
    };

    instance.deleteCustomAttribute = function(elementId, key) {
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.deleteCustomAttribute(key);
        }
    };

    instance.reset = function(elementId) {
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.reset();
        }
    };

    instance.setCustomAttributes = function(elementId, attributesJson) {
        const attributes = JSON.parse(attributesJson);
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.setCustomAttributes(attributes);
        }
    };

    instance.popoutChatWindow = function(elementId) {
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.popoutChatWindow();
        }
    };

    instance.createObserver = function(elementId) {
        const target = document.getElementById(elementId);

        if (!target || !target.parentNode)
            return null;

        const observer = new MutationObserver((mutations) => {
            const removed = mutations.some(m =>
                Array.from(m.removedNodes).includes(target)
            );

            if (removed) {
                this.shutdown(elementId);
            }
        });

        observer.observe(target.parentNode, { childList: true });

        if (this.instances[elementId]) {
            this.instances[elementId].observer = observer;
        }

        return observer;
    };

        instance.instances = {};
        instance._handlers = {};
    

    return instance;
})();
export function init(elementId, options, dotNetCallback) {
    return interop.init(elementId, options, dotNetCallback);
}

export function shutdown(elementId) {
    return interop.shutdown(elementId);
}

export function toggle(elementId) {
    return interop.toggle(elementId);
}

export function setUser(elementId, identifier, attributes) {
    return interop.setUser(elementId, identifier, attributes);
}

export function setUserAttributes(elementId, attributes) {
    return interop.setUserAttributes(elementId, attributes);
}

export function setLabel(elementId, label) {
    return interop.setLabel(elementId, label);
}

export function createObserver(elementId) {
    return interop.createObserver(elementId);
}

export function removeLabel(elementId, label) {
    return interop.removeLabel(elementId, label);
}

export function setLocale(elementId, locale) {
    return interop.setLocale(elementId, locale);
}

export function deleteCustomAttribute(elementId, attributeKey) {
    return interop.deleteCustomAttribute(elementId, attributeKey);
}

export function reset(elementId) {
    return interop.reset(elementId);
}

export function setCustomAttributes(elementId, attributes) {
    return interop.setCustomAttributes(elementId, attributes);
}

export function popoutChatWindow(elementId) {
    return interop.popoutChatWindow(elementId);
}
