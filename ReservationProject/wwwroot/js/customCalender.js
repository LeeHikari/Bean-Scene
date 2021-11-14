function selectDate(date) {
    $('.calendar-wrapper').updateCalendarOptions({
        date: date
    });
    document.getElementById("calendarDate").value = formatDate(date);

    $('#TimeDiv').hide();
}


function formatDate(date) {

    var d = new Date(date),

    month = '' + (d.getMonth() + 1),
    day = '' + d.getDate(),
    year = d.getFullYear();

    if (month.length < 2)
    month = '0' + month;
    if (day.length < 2)
    day = '0' + day;

    return [year, month, day].join('-');

}

    var maxDate = new Date();
    var todayDate = new Date();

    todayDate.setHours(0, 0, 0, 0);
    maxDate.setHours(0, 0, 0, 0);
    maxDate.setMonth(todayDate.getMonth() + 6);

    var defaultConfig = {
        weekDayLength: 1,
        date: document.getElementById("calendarDate").value,
        onClickDate: selectDate,
        highlightSelectedWeek: false,
        highlightSelectedWeekday: false,
        todayButtonContent: "Reset",
        min: todayDate,
        max: maxDate,
    };

    $('.calendar-wrapper').calendar(defaultConfig);