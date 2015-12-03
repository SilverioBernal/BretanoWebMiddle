$(document).ready(function () {
    var bpGroupCode = $('#groupCode');
    var currency = $('#currency');
    var salesPerson = $('#slpCode');
    var paymentTerms = $('#groupNum');
    var dunTerm = $('#dunTerm');
    var uBpcoRt = $('#uBpcoRt');
    var uBpcoTdc = $('#uBpcoTdc');
    var uBpcoCs = $('#uBpcoCs');
    var uBpcoTp = $('#uBpcoTp');
    var uCssIva = $('#uCssIva');
    var uCssAcceptInvoice = $('#uCssAcceptInvoice');
    var uQcaSegment = $('#uQcaSegment');
    var props = $('input[type=checkbox]');

    bpGroupCode.empty();
    currency.empty();
    salesPerson.empty();
    dunTerm.empty();
    uBpcoRt.empty();
    uBpcoTdc.empty();
    uBpcoCs.empty();
    uBpcoTp.empty();
    uCssIva.empty();
    uCssAcceptInvoice.empty();
    uQcaSegment.empty();

    bpGroupCode.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );

    currency.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );

    salesPerson.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );
    dunTerm.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );

    uBpcoRt.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );

    uBpcoTdc.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );
    uBpcoCs.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );
    uBpcoTp.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );
    uCssIva.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );
    uCssAcceptInvoice.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );
    uQcaSegment.append(
                $('<option/>', {
                    value: ''
                }).html('')
            );    

    $.getJSON('/Customer/AsyncCustomerGroupList', function (result) {
        $(result).each(function () {
            bpGroupCode.append(
                $('<option/>', {
                    value: this.groupCode
                }).html(this.groupName)
            );
        });
    });

    $.getJSON('/Customer/AsyncCurrencieList', function (result) {
        $(result).each(function () {
            currency.append(
                $('<option/>', {
                    value: this.currCode
                }).html(this.currName)
            );
        });
    });

    $.getJSON('/Customer/AsyncSalesPersonList', function (result) {
        $(result).each(function () {
            salesPerson.append(
                $('<option/>', {
                    value: this.slpCode
                }).html(this.slpName)
            );
        });
    });

    $.getJSON('/Customer/AsyncPaymentTermList', function (result) {
        $(result).each(function () {
            paymentTerms.append(
                $('<option/>', {
                    value: this.groupNum
                }).html(this.pymntGroup)
            );
        });
    });

    $.getJSON('/Customer/AsyncDunningTermsList', function (result) {
        $(result).each(function () {
            dunTerm.append(
                $('<option/>', {
                    value: this.termCode
                }).html(this.termName)
            );
        });
    });

    $.getJSON('/Customer/AsyncGetTributaryRegList', function (result) {
        $(result).each(function () {
            uBpcoRt.append(
                $('<option/>', {
                    value: this.fldValue
                }).html(this.descr)
            );
        });
    });

    $.getJSON('/Customer/AsyncGetDocumentTypeList', function (result) {
        $(result).each(function () {
            uBpcoTdc.append(
                $('<option/>', {
                    value: this.fldValue
                }).html(this.descr)
            );
        });
    });

    $.getJSON('/Customer/AsyncGetCityMagneticMediaList', function (result) {
        $(result).each(function () {
            uBpcoCs.append(
                $('<option/>', {
                    value: this.fldValue
                }).html(this.descr)
            );
        });
    });

    $.getJSON('/Customer/AsyncGetPersonTypeList', function (result) {
        $(result).each(function () {
            uBpcoTp.append(
                $('<option/>', {
                    value: this.fldValue
                }).html(this.descr)
            );
        });
    });

    $.getJSON('/Customer/AsyncGetBusinessPartnerIvaClassList', function (result) {
        $(result).each(function () {
            uCssIva.append(
                $('<option/>', {
                    value: this.fldValue
                }).html(this.descr)
            );
        });
    });
    
    $.getJSON('/Customer/AsyncGetBusinessPartnerSegmentList', function (result) {
        $(result).each(function () {
            uQcaSegment.append(
                $('<option/>', {
                    value: this.fldValue
                }).html(this.descr)
            );
        });
    });

    $.getJSON('/Customer/AsyncGetBusinessPartnerPropList', function (result) {
        for (var i = 0; i < result.length; i++) {
            var groupCode = result[i].groupCode;
            var groupName = result[i].groupName;
            var propCtrl = $('#qryGroup' + groupCode);

            var text = $(propCtrl)[0];
            text.parentElement.childNodes[3].nodeValue = groupName;                   
        }
    });

    for (var i = 1; i < 31; i++) {
        uCssAcceptInvoice.append(
                $('<option/>', {
                    value: i
                }).html(i)
            );
    }
});

