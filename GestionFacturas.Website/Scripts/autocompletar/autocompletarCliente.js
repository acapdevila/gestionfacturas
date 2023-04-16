jQuery.fn.extend({
    autocompletarCampoCliente: function (urlAutocompletar) {
        autocompleteJqueryUiCliente(this.selector, urlAutocompletar);       
    }
});


function autocompleteJqueryUiCliente(idCampo, urlAutocompletar) {
    $("#" + idCampo).autocomplete({
        source: urlAutocompletar,
        messages: {
            noResults: '',
            results: function () { }
        },
        select: function (event, ui) {
            completarDatosCliente(ui);
            return false;
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
            .append("<a>" + item.Id + " - " + item.NombreOEmpresa + "<br>" + item.NumeroIdentificacionFiscal + "</a>")
            .appendTo(ul);
    };
}

function configurarAutocompleteUnico(idControl, callbackUrl) {


    $("#" + idControl)
        .autocomplete({
            source: callbackUrl,
            type: "GET",
            dataType: "json",
            minLength: 3,
            select: function (event, ui) {
                $("#" + idControl).val(ui.item.value);
                return false;
            }
        });

}

function configurarAutocompleteId(idControlValue, idControlId, callbackUrl) {


    $("#" + idControlValue)
        .autocomplete({
            source: callbackUrl,
            type: "GET",
            dataType: "json",
            minLength: 3,
            select: function (event, ui) {
                $("#" + idControlValue).val(ui.item.value);
                $("#" + idControlId).val(ui.item.id);
                return false;
            }
        });

}

function completarDatosCliente(ui) {
    $("#Factura_IdComprador").val(ui.item.Id);
    $("#Factura_CompradorNombreOEmpresa").val(ui.item.NombreOEmpresa);
    $("#Factura_CompradorNumeroIdentificacionFiscal").val(ui.item.NumeroIdentificacionFiscal);
    $("#Factura_CompradorDireccion1").val(ui.item.Direccion1);
    $("#Factura_CompradorDireccion2").val(ui.item.Direccion2);
    $("#Factura_CompradorLocalidad").val(ui.item.Localidad);
    $("#Factura_CompradorProvincia").val(ui.item.Provincia);
    $("#Factura_CompradorCodigoPostal").val(ui.item.CodigoPostal);
    $("#Factura_CompradorEmail").val(ui.item.Email);
}

