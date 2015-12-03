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

    var salesPerson = $('#slpCode');

    $.getJSON('../../Customer/AsyncSalesPersonList', function (result) {
        $(result).each(function () {
            salesPerson.append(
                $('<option/>', {
                    value: this.slpCode
                }).html(this.slpName)
            );
        });
    });
    
    $('#btnSearch').click(function () {
        var start = $('#start').val();
        var end = $('#end').val();
        var cardCode = $('#slpCode').val();

        var id = start + '|' + end + '|' + cardCode;

        var url = '../../SalesOrder/ShowGeoReport/' + id;

        $(".ordersMap").load(url);
    });

});
