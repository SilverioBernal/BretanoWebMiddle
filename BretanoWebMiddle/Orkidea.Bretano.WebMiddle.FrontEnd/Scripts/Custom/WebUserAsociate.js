$(document).ready(function () {
    var WebUserId = $("#WebUserId").val();
    var urlD = "../../WebUser/AsyncFreeCompanyList/" + WebUserId;
    var urlU = "../../WebUser/AsyncUsedCompanyList/" + WebUserId;

    var companyId = $('#companyId');
    var slpCode = $('#slpCode');

    companyId.empty();
    slpCode.empty();

    companyId.append(
                   $('<option/>', {
                       value: ''
                   }).html('')
               );

    slpCode.append(
                   $('<option/>', {
                       value: ''
                   }).html('')
               );

    $.getJSON(urlD, function (result) {
        $(result).each(function () {
            companyId.append(
                $('<option/>', {
                    value: this.id
                }).html(this.name)
            );
        });

        var UserId = $("#WebUserId").val();

        $('#userCompanies').DataTable({
            "ajax": urlU,            
            "columnDefs": [ {
                "targets": -1,
                "data": "download_link",
                "render": function ( data, type, full, meta ) {
                    return '<a href="../../webuser/deasociate/' + UserId + '|' + full[0] + '">Eliminar</a>';
                }
            } ],
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

    $.getJSON('../../customer/AsyncSalesPersonList/', function (result) {
        $(result).each(function () {
            slpCode.append(
                $('<option/>', {
                    value: this.slpCode
                }).html(this.slpName)
            );
        });        
    });

    
});