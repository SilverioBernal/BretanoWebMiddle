$(document).ready(function () {
    $("#btnCancel").click(function () {
        var order = $("#docEntry").val();

        window.location = "../../SalesOrder/delete/" + order;
    });
});