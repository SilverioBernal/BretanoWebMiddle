$(document).ready(function () {
    $("#addType").change(function () {
        addType = this.value;

        if (addType == "B") {
            $("#streetNo").prop('disabled', true);
            $("#taxCode").prop('disabled', true);
            $("#streetNo").val('');
            $("#taxCode").val('');
        }
        if (addType == "S") {
            $("#streetNo").prop('disabled', false);
            $("#taxCode").prop('disabled', false);
        }
    });

    $("#country").change(function () {
        var country = this.value;
        var state = $("#state");
        var urlState = '/Customer/AsyncStateList/' + country
        state.empty();
        state.append(
                    $('<option/>', {
                        value: ''
                    }).html('')
                );


        $.getJSON(urlState, function (result) {
            $(result).each(function () {
                state.append(
                    $('<option/>', {
                        value: this.code
                    }).html(this.name)
                );
            });
        });
    });

    var cardCode = $("#cardCode").val();
    var urlS = "/Customer/AsyncAddressList/" + cardCode + "|S";
    var urlB = "/Customer/AsyncAddressList/" + cardCode + "|B";

    var zipCode = $('#zipCode');
    var country = $('#country');
    var taxCode = $('#taxCode');
    var uCssIva = $('#uCssIva');

    zipCode.empty();
    country.empty();
    taxCode.empty();
    uCssIva.empty();

    zipCode.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );

    country.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );

    taxCode.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );

    uCssIva.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );

    $.getJSON('/Customer/AsyncGetZipCodeList', function (result) {
        $(result).each(function () {
            zipCode.append(
                $('<option/>', {
                    value: this.fldValue
                }).html(this.descr)
            );
        });
    });

    $.getJSON('/Customer/AsyncSalesTaxCodeList', function (result) {
        $(result).each(function () {
            taxCode.append(
                $('<option/>', {
                    value: this.code
                }).html(this.name)
            );
        });
    });

    $.getJSON('/Customer/AsyncGetAddressIvaClassList', function (result) {
        $(result).each(function () {
            uCssIva.append(
                $('<option/>', {
                    value: this.fldValue
                }).html(this.descr)
            );
        });
    });

    $.getJSON('/Customer/AsyncCountryList', function (result) {
        $(result).each(function () {
            country.append(
                $('<option/>', {
                    value: this.code
                }).html(this.name)
            );
        });
        $('#billAddress').DataTable({
            "ajax": urlB,
            "language": {
                "lengthMenu": "Mostrar _MENU_ registros por pagina",
                //"zeroRecords": "Ningun resultado encontrado",
                "sLoadingRecords": "Obteniendo la información, espere un momento...",
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

        $('#shipAddress').DataTable({
            "ajax": urlS,
            "language": {
                "lengthMenu": "Mostrar _MENU_ registros por pagina",
                //"zeroRecords": "Ningun resultado encontrado",
                "sLoadingRecords": "Obteniendo la información, espere un momento...",
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
    });
});
