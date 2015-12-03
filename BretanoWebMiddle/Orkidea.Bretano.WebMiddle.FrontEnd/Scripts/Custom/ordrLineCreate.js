$(document).ready(function () {

    if ($('#editaPrecio').val() == "False") {
        document.getElementById("price").readOnly = true;
    }

    $('#ordrLines').DataTable({
        "ajax": '../../SalesOrder/AsyncOrderLinesList/' + $('#orderId').val(),
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
        },

        footerCallback: function (row, data, start, end, display) {
            var api = this.api();
            // Remove the formatting to get integer data for summation
            var intVal = function (i) {

                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                    i : 0;

            };

            // Total over all pages

            if (api.column(3).data().length) {
                var total = api
                .column(3)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                })
            }
            else { total = 0 };


            // Total over this page

            if (api.column(3).data().length) {
                var pageTotal = api
                    .column(3, { page: 'current' })
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    })
            }
            else { pageTotal = 0 };

            // Update footer
            $(api.column(3).footer()).html(
                '$' + pageTotal
            );
        }



    });

    var itemsTable = $('#itemsTable').DataTable({
        "ajax": '../../SalesOrder/AsyncItemsList',
        "columnDefs": [{
            "targets": -1,
            "data": null,
            "defaultContent": "<button data-dismiss='modal'>Seleccionar</button>"
        }],
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

    var taxCode = $('#taxCode');
    taxCode.empty();
    taxCode.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );

    var ocrCode = $('#ocrCode');
    ocrCode.empty();
    ocrCode.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );

    var uCssEnvaseDevol = $('#uCssEnvaseDevol');
    uCssEnvaseDevol.empty();
    uCssEnvaseDevol.append(
                $('<option/>', {
                    value: 'SI'
                }).html('SI')
            );
    uCssEnvaseDevol.append(
                $('<option/>', {
                    value: 'NO'
                }).html('NO')
            );

    $.getJSON('../../Customer/AsyncSalesTaxCodeList', function (result) {
        $(result).each(function () {
            taxCode.append(
                $('<option/>', {
                    value: this.code
                }).html(this.name)
            );
        });
    });



    $.getJSON('../../SalesOrder/AsyncGetDistributionRulesList', function (result) {
        $(result).each(function () {
            ocrCode.append(
                $('<option/>', {
                    value: this.prcCode
                }).html(this.prcName)
            );
        });
    });

    $('#itemsTable tbody').on('click', 'button', function () {
        var data = itemsTable.row($(this).parents('tr')).data();
        $('#itemCode').val(data[0]);
        $('#itemName').val(data[1]);

        var item = data[0] + '|' + $('#listNum').val();

        $.getJSON('../../SalesOrder/AsyncGetItemPrice/' + item, function (result) {
            $('#price').val(result);
        });
    });

    $('#whsLink').click(function () {
        var url = '../../SalesOrder/itemIndex/' + $('#itemCode').val();

        $(".stockBody").load(url);
        $('#whsModal').modal('show')
    })

    $('#whsModal').click(function (e) {
        var whsCode = e.target.parentNode.parentNode.childNodes[0];
        var whsName = e.target.parentNode.parentNode.childNodes[1]
        $('#whsCode').val(whsCode.textContent);
        $('#whsName').val(whsName.textContent);
    });
});
