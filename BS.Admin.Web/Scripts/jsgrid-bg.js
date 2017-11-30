(function(jsGrid) {

    jsGrid.locales.bg = {
        grid: {
            noDataContent: "Няма записи",
            deleteConfirm: "Наистина ли искате да изтриете?",
            pagerFormat: "Страници: {first} {prev} {pages} {next} {last} &nbsp;&nbsp; {pageIndex} от {pageCount}",
            pagePrevText: "Predi[na",
            pageNextText: "Следваща",
            pageFirstText: "Първа",
            pageLastText: "Последна",
            loadMessage: "Моля, изчакайте...",
            invalidMessage: "Невалидни данни!"
        },

        loadIndicator: {
            message: "Зареждане..."
        },

        fields: {
            control: {
                searchModeButtonTooltip: "Търсене",
                insertModeButtonTooltip: "Добави",
                editButtonTooltip: "Редакция",
                deleteButtonTooltip: "Изтриване",
                searchButtonTooltip: "Търсене",
                clearFilterButtonTooltip: "Изчисти филтър",
                insertButtonTooltip: "Добави",
                updateButtonTooltip: "Запис",
                cancelEditButtonTooltip: "Отказване"
            }
        },

        validators: {
            required: { message: "Задължителен" },
            rangeLength: { message: "" },
            minLength: { message: "" },
            maxLength: { message: "" },
            pattern: { message: "" },
            range: { message: "" },
            min: { message: "" },
            max: { message: "" }
        }
    };

}(jsGrid, jQuery));
