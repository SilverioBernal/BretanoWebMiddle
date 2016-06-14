$(document).ready(function () {
    $('#paymentState').click(function () {
        $.getJSON("../../Customer/AsyncPaymentReportType", function (result) {
            var url = ''
            if (result == "Si") {
                url = '../../Customer/smallPaymentAge/' + $('#cardCode').val();
            }
            else {
                url = '../../Customer/paymentAge/' + $('#cardCode').val();
            }

            $(".stockBody").load(url);
            $('#paymentModal').modal('show')
        });
    })


});