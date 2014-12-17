$(document).ready(function () {

    // Show the successful save message.
    if (($("#hdnSaveMessage").val() !== undefined)) {
        //alert($("#hdnSaveMessage").val());

        if ($("#hdnSaveMessage").val() == "false") {
            $("#divSaveMessage").removeClass("hidden");
        }
    }

    // Make sure a user cannot click the upload button when there is a file attached.
    $("#btnUploadProfileImage").click(function (e) {
        var fileName = $('#txtFileName').val();

        if (fileName == "") {
            //alert('nope');
            $("#lblUploadMessage").removeClass("invisible");
            e.preventDefault();
        }
    });

    $('#dayOneStart').timepicker({ 'scrollDefault': '800' });
    $('#dayOneEnd').timepicker({ 'scrollDefault': '2000' });
    $('#dayTwoStart').timepicker({ 'scrollDefault': '800' });
    $('#dayTwoEnd').timepicker({ 'scrollDefault': '2000' });
    $('#dayThreeStart').timepicker({ 'scrollDefault': '800' });
    $('#dayThreeEnd').timepicker({ 'scrollDefault': '2000' });
    $('#dayFourStart').timepicker({ 'scrollDefault': '800' });
    $('#dayFourEnd').timepicker({ 'scrollDefault': '2000' });

    $("#txtDescription").keyup(function () {
        var max = 300;
        var len = $(this).val().length;
        //alert('length: ' + len);
        if (len >= max) {
            $("#charNumDescription").text(' you have reached the limit');
        } else {
            var char = max - len;
            $("#charNumDescription").text(char + ' characters left');
        }
    });

    $("#btnAddSaleItems").click(function (e) {
        if ($("#txtSaleName").val() == "") {
            alert('You have not added the dates/times of your sale!');
            e.preventDefault();
        }
    });

    // Set the selected item categories.
    if ($("#hdnSelectedCategories").val() !== undefined) {
        var selectedCategories = new Array();
        selectedCategories = $("#hdnSelectedCategories").val().split(',');

        for (x in selectedCategories) {
            $("input:checkbox[value=" + selectedCategories[x] + "]").attr("checked", true);
        }
    }

    $(document).on('change', '.btn-file :file', function () {
        var input = $(this),
        numFiles = input.get(0).files ? input.get(0).files.length : 1,
        label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [numFiles, label]);

        $('#txtFileName').val(label);
        $('#txtSpecialItemFileName').val(label);
        $('#txtVideoFileName').val(label);
    });

    $("#btnSaveSpecialItem").click(function (e) {
        if ($('#txtSpecialItemFileName').val() == "") {
            $('#imgUploadValidationMessage').removeClass('invisible');
            
            e.preventDefault();
        }
        else {
            $('#imgUploadValidationMessage').addClass('invisible');
        }

        if ($('#ddlCategories option:selected').val() == 0) {
            $('#categoryValidationMessage').removeClass('invisible');

            e.preventDefault();
        }
        else {
            $('#categoryValidationMessage').addClass('invisible');
        }

        if ($('#txtTitle').val() == "") {
            $('#titleValidationMessage').removeClass('invisible');

            e.preventDefault();
        }
        else {
            $('#titleValidationMessage').addClass('invisible');
        }

        if ($('#txtPrice').val() == "") {
            $('#priceValidationMessage').removeClass('invisible');

            e.preventDefault();
        }
        else {
            $('#priceValidationMessage').addClass('invisible');
        }

        if ($('#txtSpecialItemDescription').val() == "") {
            $('#descriptionValidationMessage').removeClass('invisible');

            e.preventDefault();
        }
        else {
            $('#descriptionValidationMessage').addClass('invisible');
        }
    });

    $("#txtSpecialItemDescription").keyup(function () {
        var max = 300;
        var len = $(this).val().length;
        if (len >= max) {
            $("#charSpecialItemDescription").text(' you have reached the limit');
        } else {
            var char = max - len;
            $("#charSpecialItemDescription").text(char + ' characters left');
        }
    });
    
    //$("#linkReplacePicture").click(function (e) {
    //    $("#pictureUpload").removeClass('hidden');
    //    e.preventDefault();
    //});
});