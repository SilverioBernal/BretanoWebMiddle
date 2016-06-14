$(document).ready(function () {
    var table = $('#customers').DataTable({
        //processing: true,
        "ajax": '../../SalesOrder/AsyncCustomersList',
        "columnDefs": [{
            "targets": -1,
            "data": null,
            "defaultContent": "<button data-dismiss='modal'>Ver</button>"
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

        var url = '../../Customer/AsyncCustomerDetails/' + data[0];

        $.getJSON(url, function (result) {
            var cupoRiesgo = "";
            var cupoAsegurado = "";
            var pagare = "";
            var cupoTotal = format1(result.creditLine, "");

            for (i = 0; i < result.userDefinedFields.length; i++) {
                if (result.userDefinedFields[i].name === 'CSS_CupoRiesgo') {
                    if (result.userDefinedFields[i].value == "")
                        cupoRiesgo = 0;
                    else
                        cupoRiesgo = format1(result.userDefinedFields[i].value, "");
                }

                if (result.userDefinedFields[i].name === 'CSS_CupoAprobado') {
                    if (result.userDefinedFields[i].value == "")
                        cupoAsegurado = 0;
                    else
                        cupoAsegurado = format1(result.userDefinedFields[i].value, "");
                }

                if (result.userDefinedFields[i].name === 'CSS_EsPagare') {
                    if (result.userDefinedFields[i].value == "")
                        pagare = "NO";
                    else
                        pagare = result.userDefinedFields[i].value;
                }
            }

            $('#cupoRiesgo').val(cupoRiesgo);
            $('#cupoAsegurado').val(cupoAsegurado);
            $('#cupoTotal').val(cupoTotal);
            $('#pagare').val(pagare);
        });
    });

    $('#btnSearch').click(function () {
        $.getJSON("../../Customer/AsyncPaymentReportType", function (result) {
            var url = ''
            if (result == "Si") {
                url = '../../Customer/smallPaymentAge/' + $('#cardCode').val();
            }
            else {
                url = '../../Customer/paymentAge/' + $('#cardCode').val();
            }

            $(".paymentAgesTable").load(url);
        });
    });


    function format1(n, currency) {
        n = parseFloat(n);
        return currency + " " + n.toFixed(0).replace(/./g, function (c, i, a) {
            return i > 0 && c !== "," && (a.length - i) % 3 === 0 ? "." + c : c;
        });
    }
});