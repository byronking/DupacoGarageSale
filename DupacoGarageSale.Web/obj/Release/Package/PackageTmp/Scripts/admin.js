$(document).ready(function () {
    
    // Shoe the user edit fields.
    if ($("#hdnShowEditUser").val() == "true") {
        //alert('true');
        $("#editUserDiv").removeClass('hidden');
    }

    // Show the garage sale view fields.
    if ($("#hdnShowEditGarageSale").val() == "true") {
        $("#editGarageSaleDiv").removeClass('hidden');
    }

    // Show the successful save message.
    if (($("#hdnSaveMessage").val() !== undefined)) {
        //alert($("#hdnSaveMessage").val());

        if ($("#hdnSaveMessage").val() == "false") {
            $("#divSaveMessage").removeClass("hidden");
        }
    }
});
