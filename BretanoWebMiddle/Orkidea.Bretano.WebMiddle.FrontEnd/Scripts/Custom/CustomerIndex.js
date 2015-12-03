$(document).ready(function () {
    var table = $('#customers').DataTable({
        //processing: true,
        "ajax": '../SalesOrder/AsyncCustomersList',
        "columnDefs": [{
            "targets": -1,
            "data": null,
            "defaultContent": "<button data-dismiss='modal' class='btn btn-info'>Editar</button>"
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
        var id = data[0];
        
        var url = "../../customer/edit/" + id;

        window.location = url;

    });
});