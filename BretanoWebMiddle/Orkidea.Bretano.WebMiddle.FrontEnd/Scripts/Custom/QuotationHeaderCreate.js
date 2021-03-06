﻿$(document).ready(function () {

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
        var url = '../../Customer/paymentAge/' + $('#cardCode').val();

        $(".stockBody").load(url);
        $('#paymentModal').modal('show')
    })

    var series = $('#series');

    series.empty();
    series.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );

    $.getJSON('../../Quotation/AsyncSeriesList', function (result) {
        $(result).each(function () {
            series.append(
                $('<option/>', {
                    value: this.series
                }).html(this.seriesName)
            );
        });

        var table = $('#customers').DataTable({
            //processing: true,
            "ajax": '../../SalesOrder/AsyncCustomersList',
            "columnDefs": [{
                "targets": -1,
                "data": null,
                "defaultContent": "<button data-dismiss='modal'>Seleccionar</button>"
            }],
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

            var shipToCode = $('#shipToCode');
            var payToCode = $('#payToCode');
            var groupNum = $('#groupNum');

            var urlS = "../../SalesOrder/AsyncAddressList/" + data[0] + "|S";
            var urlB = "../../SalesOrder/AsyncAddressList/" + data[0] + "|B";

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

            $.getJSON('../../Customer/AsyncPaymentTermList', function (result) {
                $(result).each(function () {
                    groupNum.append(
                        $('<option/>', {
                            value: this.groupNum
                        }).html(this.pymntGroup)
                    );
                });
            });
        });
    });
});