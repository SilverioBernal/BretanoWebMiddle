$(document).ready(function () {
    var cardCode = $("#cardCode").val();
    var urlD = "/Customer/AsyncWithholdingTaxList/" + cardCode;
    var urlU = "/Customer/AsyncBusinessPartnerWithholdingTaxList/" + cardCode;
    var wt = $('#wtCode');
    wt.empty();

    wt.append(
                   $('<option/>', {
                       value: ''
                   }).html('')
               );

    $.getJSON(urlD, function (result) {
        $(result).each(function () {
            wt.append(
                $('<option/>', {
                    value: this.wtCode
                }).html(this.wtName)
            );
        });

        $('#customerTaxes').DataTable({
            "ajax": urlU,
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