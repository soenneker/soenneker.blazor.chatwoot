export class ChatwootInterop {
    constructor() {
        this.instances = {};
        this._handlers = {};
    }

    init(elementId, configuration, dotNetCallback) {
        if (this.instances[elementId]?.isLoaded)
            return;

        var options = JSON.parse(configuration);

        this.instances[elementId] = {
            dotNetCallback,
            isLoaded: true
        };

        window.chatwootSettings = options;

        this.attachEvents(elementId);

        window.chatwootSDK.run(window.chatwootSettings);
    }

    attachEvents(elementId) {
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
    }


    shutdown(elementId) {
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
    }

    toggle(elementId) {
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.toggle();
        }
    }

    setUser(elementId, identifier, attributesJson) {
        const attributes = JSON.parse(attributesJson);
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.setUser(identifier, attributes);
        }
    }

    setUserAttributes(elementId, attributesJson) {
        const attributes = JSON.parse(attributesJson);
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.setUserAttributes(attributes);
        }
    }

    setLabel(elementId, label) {
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.setLabel(label);
        }
    }

    removeLabel(elementId, label) {
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.removeLabel(label);
        }
    }

    setLocale(elementId, locale) {
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.setLocale(locale);
        }
    }

    deleteCustomAttribute(elementId, key) {
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.deleteCustomAttribute(key);
        }
    }

    reset(elementId) {
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.reset();
        }
    }

    setCustomAttributes(elementId, attributesJson) {
        const attributes = JSON.parse(attributesJson);
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.setCustomAttributes(attributes);
        }
    }

    popoutChatWindow(elementId) {
        if (this.instances[elementId]?.isLoaded) {
            window.$chatwoot?.popoutChatWindow();
        }
    }

    createObserver(elementId) {
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
    }
}

window.ChatwootInterop = new ChatwootInterop();
