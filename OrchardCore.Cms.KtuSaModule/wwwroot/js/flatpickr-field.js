document.addEventListener('DOMContentLoaded', function () {
    flatpickr('.datepicker', {
        dateFormat: 'Y-m-d'
    });

    flatpickr('.datetimepicker', {
        enableTime: true,
        dateFormat: 'Y-m-d H:i',
        time_24hr: true,
    });
});
