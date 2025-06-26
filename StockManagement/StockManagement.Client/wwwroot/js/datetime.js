window.getLocalDateTime = function () {
    // Returns ISO string in local time (without timezone offset)
    const now = new Date();
    // Return as ISO string (e.g., "2025-06-25T14:30:00")
    return now.getFullYear() + '-' +
        String(now.getMonth() + 1).padStart(2, '0') + '-' +
        String(now.getDate()).padStart(2, '0') + 'T' +
        String(now.getHours()).padStart(2, '0') + ':' +
        String(now.getMinutes()).padStart(2, '0') + ':' +
        String(now.getSeconds()).padStart(2, '0');
};