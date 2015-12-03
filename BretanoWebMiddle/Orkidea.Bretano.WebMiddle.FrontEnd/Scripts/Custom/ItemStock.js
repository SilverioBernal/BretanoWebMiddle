$(document).ready(function () {
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


    $('#itemsTable tbody').on('click', 'button', function () {
        var data = itemsTable.row($(this).parents('tr')).data();
        $('#itemCode').val(data[0]);
        $('#itemName').val(data[1]);                
    });

    $('#btnSearch').click(function () {
        var url = '../../SalesOrder/itemIndex/' + $('#itemCode').val();

        $(".stockBody").load(url);
    });

});