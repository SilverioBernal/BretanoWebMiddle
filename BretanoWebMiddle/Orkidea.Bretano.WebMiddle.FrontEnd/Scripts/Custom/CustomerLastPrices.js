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

    var table = $('#customers').DataTable({
        //processing: true,
        "ajax": '../SalesOrder/AsyncCustomersList',
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
    });

    $('#btnSearch').click(function () {
        var start = $('#start').val();
        var end = $('#end').val();
        var cardCode = $('#cardCode').val();

        var id = start + '|' + end + '|' + cardCode;

        var url = '../../customer/searchlastprices/' + id;

        $(".pricesTable").load(url);

    });

});