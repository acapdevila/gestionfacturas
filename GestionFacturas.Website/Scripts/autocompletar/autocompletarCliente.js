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

function completarDatosCliente(ui) {
    $("#Factura_IdComprador").val(ui.item.Id);
    $("#Factura_CompradorNombreOEmpresa").val(ui.item.NombreOEmpresa);
    $("#Factura_CompradorNumeroIdentificacionFiscal").val(ui.item.NumeroIdentificacionFiscal);
    $("#Factura_CompradorDireccion").val(ui.item.Direccion);
    $("#Factura_CompradorLocalidad").val(ui.item.Localidad);
    $("#Factura_CompradorProvincia").val(ui.item.Provincia);
    $("#Factura_CompradorCodigoPostal").val(ui.item.CodigoPostal);
    $("#Factura_CompradorEmail").val(ui.item.Email);
}

