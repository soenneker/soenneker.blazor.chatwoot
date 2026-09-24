import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import { runInNewContext } from 'node:vm';
import test from 'node:test';

const source = readFileSync(new URL('../src/Soenneker.Blazor.Chatwoot/wwwroot/js/chatwootinterop.js', import.meta.url), 'utf8');

function setup(options = {}) {
    const events = new Map(), frames = [], timers = [], observers = [], calls = [], callbacks = [];
    let scans = 0;
    const element = (widget = false) => ({
        nodeType: 1, style: {}, children: [], attributes: {}, widget,
        appendChild(child) { this.children.push(child); },
        addEventListener(name, handler) { this[name] = handler; },
        setAttribute(name, value) { this.attributes[name] = value; },
        removeAttribute(name) { delete this.attributes[name]; },
        remove() { this.removed = true; },
        matches() { return this.widget; },
        querySelector() { return null; }
    });
    const frame = element(true);
    const document = {
        body: element(), head: element(),
        createElement: () => element(),
        getElementById: () => null,
        querySelector: () => { scans++; return frame; },
        querySelectorAll: () => { scans++; return [frame]; }
    };
    const window = {
        addEventListener: (name, fn) => events.set(name, fn),
        removeEventListener: name => events.delete(name),
        requestAnimationFrame: fn => frames.push(fn),
        setTimeout: fn => timers.push(fn),
        setInterval: fn => timers.push(fn),
        clearInterval() {},
        getComputedStyle: () => { scans++; return { display: 'block', visibility: 'visible', opacity: '1' }; },
        chatwootSDK: { run() {
            calls.push(['run']);
            window.$chatwoot = Object.fromEntries(['toggle', 'reset', 'setUser', 'setUserAttributes', 'setLabel', 'popoutChatWindow'].map(name =>
                [name, (...args) => calls.push([name, ...args])]));
        } }
    };
    class MutationObserver {
        constructor(callback) { this.callback = callback; observers.push(this); }
        observe() {}
        disconnect() { this.disconnected = true; }
    }
    const context = { document, window, MutationObserver, console };
    runInNewContext(source.replace(/^export /gm, '') + '\nthis.api = {init, open, close, toggle, shutdown, setUser, setLabel, reset, popoutChatWindow};', context);
    context.api.init('chat', options, { invokeMethodAsync: async (...args) => callbacks.push(args) });
    return {
        api: context.api, calls, callbacks, document, observers, element, window, frame,
        get scans() { return scans; },
        paint() { frames.splice(0).forEach(fn => fn()); timers.splice(0).forEach(fn => fn()); },
        ready() { events.get('chatwoot:ready')?.({}); }
    };
}

test('unrelated page interactions do not start the SDK or install a page observer', () => {
    const state = setup();
    assert.equal(state.calls.length, 0);
    assert.equal(state.observers.length, 0);
    assert.equal(state.document.body.children[0].attributes['aria-label'], 'Open chat');
});

test('launcher paints feedback, starts once, replays user settings and opens when ready', () => {
    const state = setup();
    state.api.setUser('chat', 'user-1', { name: 'Example' });
    state.api.setLabel('chat', 'support');
    const launcher = state.document.body.children[0];
    launcher.click();
    state.api.open('chat');
    assert.equal(launcher.attributes['aria-busy'], 'true');
    assert.equal(state.calls.length, 0);
    state.paint();
    assert.deepEqual(state.calls.map(c => c[0]), ['run', 'setUser', 'setLabel']);
    state.ready();
    assert.deepEqual(state.calls.at(-1), ['toggle', 'open']);
    assert.deepEqual(state.callbacks, [['OnReadyCallback']]);
    assert.equal(launcher.removed, true);
});

test('hidden launcher supports programmatic open and cancellation before startup', () => {
    const state = setup({ hideMessageBubble: true });
    assert.equal(state.document.body.children.length, 0);
    state.api.open('chat');
    state.api.close('chat');
    state.paint();
    assert.equal(state.calls.length, 0);
    state.api.open('chat');
    state.paint();
    state.ready();
    assert.deepEqual(state.calls.at(-1), ['toggle', 'open']);
});

test('closing while the SDK loads prevents ready from reopening the widget', () => {
    const state = setup();
    state.api.open('chat');
    state.paint();
    state.api.close('chat');
    state.ready();
    assert.equal(state.calls.some(c => c[0] === 'toggle' && c[1] === 'open'), false);
});

test('shutdown cancels deferred startup even if the same element id is reused', () => {
    const state = setup();
    state.api.open('chat');
    state.api.shutdown('chat');
    state.api.init('chat', {}, { invokeMethodAsync: async () => {} });
    state.paint();
    assert.equal(state.calls.length, 0);
    assert.equal(state.document.body.children[0].removed, true);
});

test('reset discards queued identity before chat is started', () => {
    const state = setup();
    state.api.setUser('chat', 'old-user', {});
    state.api.reset('chat');
    state.api.open('chat');
    state.paint();
    assert.deepEqual(state.calls.map(c => c[0]), ['run', 'reset']);
});

test('eager opt-out retains startup and ignores unrelated page mutations', () => {
    const state = setup({ deferUntilOpen: false });
    assert.deepEqual(state.calls, [['run']]);
    assert.equal(state.document.body.children.length, 0);
    const before = state.scans;
    const unrelated = state.element();
    state.observers[0].callback([{ type: 'attributes', target: unrelated }]);
    state.observers[0].callback([{ type: 'childList', addedNodes: [unrelated], removedNodes: [] }]);
    assert.equal(state.scans, before);
    state.observers[0].callback([{ type: 'attributes', target: state.element(true) }]);
    assert.ok(state.scans > before);
    state.api.shutdown('chat');
    assert.equal(state.observers[0].disconnected, true);
});

test('popout starts synchronously to preserve browser user activation', () => {
    const state = setup();
    state.api.popoutChatWindow('chat');
    assert.deepEqual(state.calls.map(c => c[0]), ['run', 'popoutChatWindow']);
});

test('SDK state keeps the widget interactive when open/close events are absent', () => {
    const state = setup({ deferUntilOpen: false });
    state.window.$chatwoot.isOpen = true;
    state.observers[0].callback([{ type: 'attributes', target: state.frame }]);
    assert.equal(state.frame.style.pointerEvents, '');
    state.window.$chatwoot.isOpen = false;
    state.observers[0].callback([{ type: 'attributes', target: state.frame }]);
    assert.equal(state.frame.style.pointerEvents, 'none');
});

test('failed startup exposes a retry and reports the failure', () => {
    const state = setup();
    state.window.chatwootSDK.run = () => { throw new Error('SDK unavailable'); };
    state.api.open('chat');
    state.paint();
    assert.equal(state.document.body.children[0].textContent, 'Retry chat');
    assert.equal(state.callbacks[0][0], 'OnErrorCallback');
    state.window.chatwootSDK.run = () => state.calls.push(['run']);
    state.api.open('chat');
    state.paint();
    assert.deepEqual(state.calls, [['run']]);
});
