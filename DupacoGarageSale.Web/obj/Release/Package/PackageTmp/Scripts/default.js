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
            var specialChars = "<>@!#$%^&*()_+[]{}?:;|'\"\\,/~`-="
            var check = function (string) {
                for (i = 0; i < specialChars.length; i++) {
                    if (string.indexOf(specialChars[i]) > -1) {
                        return true
                    }
                }
                return false;
            }

            if (check($('#txtPrice').val()) == true) {
                //alert('Your search string contains illegal characters.');
                //$('#priceValidationMessage').val("Enter only numbers for the item price.");
                $('#priceValidationMessage').removeClass('invisible');
            }
            else {
                $('#priceValidationMessage').addClass('invisible');
            }
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
    
    // Respond to the changing of the optionsMedia radio buttons.
    if ($("#radioImage").is(':checked')) {
        //alert('image');
        $("#divImage").removeClass('hidden');
        $("#divYouTube").addClass('hidden');
        $("#divVine").addClass('hidden');
    }
    else if ($("#radioYouTube").is(':checked')) {
        //alert('youtube');
        $("#divImage").addClass('hidden');
        $("#divYouTube").removeClass('hidden');
        $("#divVine").addClass('hidden');
    }
    else if ($("#radioVine").is(':checked')) {
        //alert('vine');
        $("#divImage").addClass('hidden');
        $("#divYouTube").addClass('hidden');
        $("#divVine").removeClass('hidden');
    }

    $("#radioImage").change(function () {
        $("#divImage").removeClass('hidden');
        $("#divYouTube").addClass('hidden');
        $("#divVine").addClass('hidden');
    });

    $("#radioYouTube").change(function () {
        $("#divImage").addClass('hidden');
        $("#divYouTube").removeClass('hidden');
        $("#divVine").addClass('hidden');
    });

    $('#radioVine').change(function () {
        $("#divImage").addClass('hidden');
        $("#divYouTube").addClass('hidden');
        $("#divVine").removeClass('hidden');
    });

    // Validation for the blog post
    $("#btnSaveBlogPost").click(function (e) {
        if ($('#txtBlogPostTitle').val() == "") {
            $('#titleValidationMessage').removeClass('invisible');

            e.preventDefault();
        }
        else {
            $('#titleValidationMessage').addClass('invisible');
        }

        if ($('#txtBlogPost').val() == "") {
            $('#postValidationMessage').removeClass('invisible');

            e.preventDefault();
        }
        else {
            $('#postValidationMessage').addClass('invisible');
        }
    });
    
    $("#txtBlogPost").keyup(function () {
        var max = 300;
        var len = $(this).val().length;
        if (len >= max) {
            $("#charNumBlogPost").text(' you have reached the limit');
        } else {
            var char = max - len;
            $("#charNumBlogPost").text(char + ' characters left');
        }
    });

    // Validation for the password reset page.
    $("#btnResetPassword").click(function (e) {
        if ($('#txtPasswordResetUserName').val() == "") {
            $('#validateUserName').removeClass('invisible');

            e.preventDefault();
        }
        else {
            $('#validateUserName').addClass('invisible');
        }

        if ($('#txtPasswordResetEmail').val() == "") {
            $('#validateEmail').removeClass('invisible');

            e.preventDefault();
        }
        else {
            $('#validateEmail').addClass('invisible');
        }
    });

    // Validation for the search screen.
    $("#btnSearch").click(function (e) {
        if ($('#txtSearch').val() == "") {
            $('#validateSearchText').removeClass('hidden');

            e.preventDefault();
        }
        else {
            $('#validateSearchText').addClass('hidden');
        }
        
        if ($('#ddlCategories option:selected').val() == 0) {
            $('#validateSearchCategories').removeClass('hidden');

            e.preventDefault();
        }
        else {
            $('#validateSearchCategories').addClass('hidden');
        }
    });
});