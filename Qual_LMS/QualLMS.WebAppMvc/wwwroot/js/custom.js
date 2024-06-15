
function formatDate(value) {
    if (value) {
        var date = new Date(value);
        var day = date.getDate().toString().padStart(2, '0');
        var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var month = monthNames[date.getMonth()];
        var year = date.getFullYear();
        return `${day}-${month}-${year}`;
    }
    return '';
}

function formatTime(value) {
    if (value) {
        var date = new Date(value);
        var hours = date.getHours().toString().padStart(2, '0');
        var minutes = date.getMinutes().toString().padStart(2, '0');
        var seconds = date.getSeconds().toString().padStart(2, '0');
        return hours + ':' + minutes + ':' + seconds;
    }
    return '';
}

function serialFormatter(value, row, index) {
    return index + 1;
}

function formatOrgLocation(value) {
    if (value) {
        // Parse the latitude and longitude from the string
        var coordinates = value.split(',');
        if (coordinates.length === 2) {
            var latitude = coordinates[0].trim();
            var longitude = coordinates[1].trim();

            // Create the Google Maps URL
            var googleMapsUrl = "<a href='https://www.google.com/maps?q=" + latitude + ',' + longitude + "' target='_blank'>Click</a>";

            return googleMapsUrl;
        }
    }
    return '';
}

function generateGUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0,
            v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
