$(document).ready(function () {
    
    if ($("#hdnShowEditUser").val() == "true") {
        //alert('true');
        $("#editUserDiv").removeClass('hidden');
    }

    if ($("#hdnShowEditGarageSale").val() == "true") {
        $("#editGarageSaleDiv").removeClass('hidden');
    }
});
