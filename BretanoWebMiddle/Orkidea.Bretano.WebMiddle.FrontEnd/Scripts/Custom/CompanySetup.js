$(document).ready(function () {
    $('#btnSave').click(function () {
        if ($('.setupRes').css('display') != 'none') {
            $('.setupRes').fadeToggle();
        }

        var controls = $(".form-control");
        var CompanyParameters = [];        

        for (var i = 0; i < controls.length; i++) {
            var companyId = $("#companyId").val();
            var parameterId = controls[i].id.replace("P-", "");
            var paramValue = controls[i].value;

            CompanyParameters.push({
                idCompany: companyId,
                idParameter: parameterId,
                value: paramValue
            })
        }

        $.ajax({
            url: "../../company/SaveCompanySettings",
            data: JSON.stringify(CompanyParameters),
            success: res,
            //error: error,
            type: 'POST',
            contentType: 'application/json, charset=utf-8',
            dataType: 'json'
        });

        function res(data) {
            if (data == "OK") {
                $(".alert").removeClass('alert-danger').addClass('alert-success');
            }
            else {
                $(".alert").removeClass('alert-success').addClass('alert-danger');
                $("#res").html(data);
            }
            
            $(".setupRes").toggle("slow");
        }
    });
});
