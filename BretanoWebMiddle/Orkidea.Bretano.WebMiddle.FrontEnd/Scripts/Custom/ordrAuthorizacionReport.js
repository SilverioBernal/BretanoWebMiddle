$(document).ready(function () {
    $('.dtPicker').datepicker({
        format: "yyyy-mm-dd",
        todayBtn: "linked",
        language: "es",
        orientation: "bootom auto",
        autoclose: true,
        todayHighlight: true,
        toggleActive: true
    });    

    $('#btnSearch').click(function () {
        var start = $('#start').val();
        var end = $('#end').val();        

        var id = start + '|' + end ;

        var url = '../../SalesOrder/ShowAuthorizationReport/' + id;

        $(".ordersTable").load(url);
    });

});