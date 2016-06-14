$(document).ready(function () {

    // Try HTML5 geolocation.
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {

            $('#uLatitud').val(position.coords.latitude);
            $('#uLongitud').val(position.coords.longitude);
           
        }, function () {
            //handleLocationError(true, infoWindow, map.getCenter());
        });
    } else {
        // Browser doesn't support Geolocation
        //handleLocationError(false, infoWindow, map.getCenter());
    }

    //*******************

    $('.dtPicker').datepicker({
        format: "yyyy-mm-dd",
        todayBtn: "linked",
        language: "es",
        orientation: "botom auto",
        autoclose: true,
        todayHighlight: true,
        toggleActive: true
    });

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

    var series = $('#series');

    series.empty();
    series.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );

    $.getJSON('../SalesOrder/AsyncSeriesList', function (result) {
        $(result).each(function () {
            series.append(
                $('<option/>', {
                    value: this.series
                }).html(this.seriesName)
            );
        });

        var table = $('#customers').DataTable({
            //processing: true,
            "ajax": '../SalesOrder/AsyncCustomersList',
            "columnDefs": [ {
                "targets": -1,
                "data": null,
                "defaultContent": "<button data-dismiss='modal'>Seleccionar</button>"
            } ],
            "language": {
                //"processing": "Hang on. Waiting for response...",
                "lengthMenu": "Mostrar _MENU_ registros por pagina",
                //"zeroRecords": "Ningun resultado encontrado",
                "sLoadingRecords": "Obteniendo los clientes, espere un momento...",
                "info": "Página _PAGE_ de _PAGES_",
                "infoEmpty": "Sin registros",
                "infoFiltered": "(filtered from _MAX_ total records)",
                "search": "Buscar:",
                "paginate": {
                    "first": "Primera",
                    "last": "Ultima",
                    "next": "Siguiente",
                    "previous": "Anterior"
                },                
            }
        });

        $('#customers tbody').on('click', 'button', function () {
            var data = table.row($(this).parents('tr')).data();
            $('#cardCode').val(data[0]);
            $('#cardName').val(data[1]);
            $('#customerCardName').val(data[1]);

            var shipToCode = $('#shipToCode');
            var payToCode = $('#payToCode');
            var groupNum = $('#groupNum');

            var urlS = "../SalesOrder/AsyncAddressList/" + data[0] + "|S";
            var urlB = "../SalesOrder/AsyncAddressList/" + data[0] + "|B";

            shipToCode.empty();
            payToCode.empty();
            groupNum.empty();

            shipToCode.append(
                        $('<option/>', {
                            value: ''
                        }).html('')
                    );
            payToCode.append(
                        $('<option/>', {
                            value: ''
                        }).html('')
                    );
            groupNum.append(
                        $('<option/>', {
                            value: ''
                        }).html('')
                    );

            $.getJSON(urlS, function (result) {
                $(result).each(function () {
                    shipToCode.append(
                        $('<option/>', {
                            value: this.address
                        }).html(this.address + '.::.' + this.street)
                    );
                });
            });

            $.getJSON(urlB, function (result) {
                $(result).each(function () {
                    payToCode.append(
                        $('<option/>', {
                            value: this.address
                        }).html(this.address + '.::.' + this.street)
                    );
                });
            });

            $.getJSON('../Customer/AsyncPaymentTermList', function (result) {
                $(result).each(function () {
                    groupNum.append(
                        $('<option/>', {
                            value: this.groupNum
                        }).html(this.pymntGroup)
                    );
                });
            });

            var urlC = '../../Customer/AsyncCustomerDetails/' + data[0];

            $.getJSON(urlC, function (result) {
                var customerAddress = "";
                var customerPhone1 = "";
                var customerPhone2 = "";
                var customerContact = "";

                $('#customerAddress').val(result.address);
                $('#customerPhone1').val(result.phone1);
                $('#customerPhone2').val(result.phone2);
                $('#customerContact').val(result.cntctPrsn);
            });
        });
    });


});