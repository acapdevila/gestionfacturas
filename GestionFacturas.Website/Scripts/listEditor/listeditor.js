function removeNestedForm(element, container, deleteElement) {
    $container = $(element).parents(container);
    $container.find(deleteElement).val('True');
    
    //El elemento se esconde y hay que tratar las validaciones
    $container.hide();

    $container.find(":input[data-val=true]").each(function () {
      $(this).removeAttr('data-val');
    });

    resetValidation();
 }

function addNestedForm(container, counter, ticks, content, focusId) {
    var nextIndex = $(counter).length;
    var pattern = new RegExp(ticks, "gi");
    content = content.replace(pattern, nextIndex);
    $(container).append(content);
    $(focusId.replace(pattern, nextIndex)).focus();
    resetValidation();
}


function resetValidation() {
    // can't add validation to a form, must clear validation and rebuild
    $("form").removeData("validator");
    $("form").removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse("form");
}