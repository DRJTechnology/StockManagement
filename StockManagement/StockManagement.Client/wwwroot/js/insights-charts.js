// Chart.js interop for the Sales Insights page.
//
// Chart.js refuses to attach to a canvas that already has a chart on it
// ("Canvas is already in use"), and the Insights page re-renders every chart on
// every filter change, so instances are tracked by canvas id and torn down
// before being replaced.
window.insightsCharts = (function () {
    const charts = {};

    const currency = new Intl.NumberFormat('en-GB', {
        style: 'currency', currency: 'GBP', maximumFractionDigits: 0
    });
    const number = new Intl.NumberFormat('en-GB');

    // Axis ticks and tooltips are formatted by a function, which cannot survive
    // the trip through JSON. The page instead sets a "smValueFormat" hint on the
    // config and the callbacks are attached here.
    function applyValueFormat(config) {
        const hint = config.smValueFormat;
        if (!hint) {
            return;
        }
        delete config.smValueFormat;

        const format = hint === 'currency'
            ? function (value) { return currency.format(value); }
            : function (value) { return number.format(value); };

        config.options = config.options || {};

        // Which axis carries the value rather than the category. Horizontal
        // bars swap them over; doughnuts have no axes at all and parse to a
        // bare number.
        const valueAxis = config.options.indexAxis === 'y' ? 'x' : 'y';

        config.options.plugins = config.options.plugins || {};
        config.options.plugins.tooltip = config.options.plugins.tooltip || {};
        config.options.plugins.tooltip.callbacks = {
            label: function (context) {
                const label = context.dataset.label || context.label || '';
                const parsed = context.parsed;
                const value = (parsed !== null && typeof parsed === 'object') ? parsed[valueAxis] : parsed;
                return label + ': ' + format(value);
            }
        };

        if (config.options.scales && config.options.scales[valueAxis]) {
            config.options.scales[valueAxis].ticks = { callback: format };
        }
    }

    function destroy(canvasId) {
        const existing = charts[canvasId];
        if (existing) {
            existing.destroy();
            delete charts[canvasId];
        }
    }

    function render(canvasId, config) {
        destroy(canvasId);

        const canvas = document.getElementById(canvasId);
        // The canvas is gone if the user navigated away mid-render.
        if (!canvas || !config) {
            return;
        }

        applyValueFormat(config);
        charts[canvasId] = new Chart(canvas, config);
    }

    return { render: render, destroy: destroy };
})();
