$(document).ready(function () {
    var props = $('input[type=checkbox]');

    $.getJSON('/Customer/AsyncGetBusinessPartnerPropList', function (result) {
        for (var i = 0; i < result.length; i++) {
            var groupCode = result[i].groupCode;
            var groupName = result[i].groupName;
            var propCtrl = $('#qryGroup' + groupCode);

            var text = $(propCtrl)[0];
            text.parentElement.childNodes[3].nodeValue = groupName;
        }
    });
});