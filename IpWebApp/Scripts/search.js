function search (selector) {
    $(selector).on("keyup", function() {
        var value = $(this).val().toLowerCase();
        $(`${selector} tr`).filter(function() {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
}