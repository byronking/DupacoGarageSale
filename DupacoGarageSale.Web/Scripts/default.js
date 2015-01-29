$(document).ready(function () {

    // Show the error message if there is an existing account
    if ($("#hdnExistingAccountError").val() == "true") {
        $("#existingAccountError").removeClass('hidden');
    }

    // Show the home page content on click
    $("#linkCategory1").click(function (e) {
        var categoryId = $("#hdnCategory1").val();
        $.get('/GarageSale/GetItemsByCategory?categoryId=' + categoryId, function (data) {
            $("#categoryContent").html(data);
        });

        e.preventDefault();
        $("#featuredContent").addClass('invisible');
        $("#categoryContent").removeClass('invisible');
    });

    $("#linkCategory2").click(function (e) {
        var categoryId = $("#hdnCategory2").val();
        $.get('/GarageSale/GetItemsByCategory?categoryId=' + categoryId, function (data) {
            $("#categoryContent").html(data);
        });

        e.preventDefault();
        $("#featuredContent").addClass('invisible');
        $("#categoryContent").removeClass('invisible');
    });

    $("#linkCategory3").click(function (e) {
        var categoryId = $("#hdnCategory3").val();
        $.get('/GarageSale/GetItemsByCategory?categoryId=' + categoryId, function (data) {
            $("#categoryContent").html(data);
        });

        e.preventDefault();
        $("#featuredContent").addClass('invisible');
        $("#categoryContent").removeClass('invisible');
    });

    $("#linkCategory4").click(function (e) {
        var categoryId = $("#hdnCategory4").val();
        $.get('/GarageSale/GetItemsByCategory?categoryId=' + categoryId, function (data) {
            $("#categoryContent").html(data);
        });

        e.preventDefault();
        $("#featuredContent").addClass('invisible');
        $("#categoryContent").removeClass('invisible');
    });

    $("#linkCategory5").click(function (e) {
        var categoryId = $("#hdnCategory5").val();
        $.get('/GarageSale/GetItemsByCategory?categoryId=' + categoryId, function (data) {
            $("#categoryContent").html(data);
        });

        e.preventDefault();
        $("#featuredContent").addClass('invisible');
        $("#categoryContent").removeClass('invisible');
    });

    $("#linkCategory6").click(function (e) {
        var categoryId = $("#hdnCategory6").val();
        $.get('/GarageSale/GetItemsByCategory?categoryId=' + categoryId, function (data) {
            $("#categoryContent").html(data);
        });

        e.preventDefault();
        $("#featuredContent").addClass('invisible');
        $("#categoryContent").removeClass('invisible');
    });

    $("#linkCategory7").click(function (e) {
        var categoryId = $("#hdnCategory7").val();
        $.get('/GarageSale/GetItemsByCategory?categoryId=' + categoryId, function (data) {
            $("#categoryContent").html(data);
        });

        e.preventDefault();
        $("#featuredContent").addClass('invisible');
        $("#categoryContent").removeClass('invisible');
    });

    $("#linkCategory8").click(function (e) {
        var categoryId = $("#hdnCategory8").val();
        $.get('/GarageSale/GetItemsByCategory?categoryId=' + categoryId, function (data) {
            $("#categoryContent").html(data);
        });

        e.preventDefault();
        $("#featuredContent").addClass('invisible');
        $("#categoryContent").removeClass('invisible');
    });

    $("#linkCategory9").click(function (e) {
        var categoryId = $("#hdnCategory9").val();
        $.get('/GarageSale/GetItemsByCategory?categoryId=' + categoryId, function (data) {
            $("#categoryContent").html(data);
        });

        e.preventDefault();
        $("#featuredContent").addClass('invisible');
        $("#categoryContent").removeClass('invisible');
    });

    $("#linkCategory10").click(function (e) {
        var categoryId = $("#hdnCategory10").val();
        $.get('/GarageSale/GetItemsByCategory?categoryId=' + categoryId, function (data) {
            $("#categoryContent").html(data);
        });

        e.preventDefault();
        $("#featuredContent").addClass('invisible');
        $("#categoryContent").removeClass('invisible');
    });

    $("#linkCategory11").click(function (e) {
        var categoryId = $("#hdnCategory11").val();
        $.get('/GarageSale/GetItemsByCategory?categoryId=' + categoryId, function (data) {
            $("#categoryContent").html(data);
        });

        e.preventDefault();
        $("#featuredContent").addClass('invisible');
        $("#categoryContent").removeClass('invisible');
    });

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
        var max = 100;
        var len = $(this).val().length;
        //alert('length: ' + len);
        if (len >= max) {
            $("#charNumDescription").text(' you have reached the limit');
        } else {
            var char = max - len;
            $("#charNumDescription").text(char + ' characters left');
        }
    });

    // This validates the garage sale for required dates/times.
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
        // Disabling this per JH.
        //if ($('#txtBlogPostTitle').val() == "") {
        //    $('#titleValidationMessage').removeClass('invisible');

        //    e.preventDefault();
        //}
        //else {
        //    $('#titleValidationMessage').addClass('invisible');
        //}

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

    // Hide the map and filter tool if there are no results.
    if ($("#hdnSearchResultsCount").val() !== undefined) {
        if ($("#hdnSearchResultsCount").val() == "0") {
            $("#map-canvas").addClass('hidden');
            $("#results-filter").addClass('hidden');
        }
    }

    // Show the map if there are results.
    if ($("#hdnShowMap").val() !== undefined) {
        if ($("#hdnShowMap").val() == "true") {

            //alert('show map: ' + $("#hdnShowMap").val());
            $("#map-canvas").removeClass('hidden');
        }
        else {
            $("#map-canvas").addClass('hidden');
        }
    }

    $("#btnSearchBarFind").click(function () {
        $("#hdnSearchBarCriteria").val($("#txtSearchCriteria").val());

        //alert('you searched for: ' + $("#hdnSearchBarCriteria").val());
    });

    $("#btnMapItinerary").click(function () {    
        $("#driving-directions").removeClass('hidden');
    });

    if ($("#hdnRequestTooOld").val() == "visible") {
        $("#resetErrorMessage").removeClass('hidden');
    }

    if ($("#hdnPasswordResetSuccessful").val() == "visible") {
        $("#passwordResetSuccessful").removeClass('hidden');
        $("#loginAlert").addClass('hidden');
    }

    // Monitor the number of characters in the message textarea.
    $("#txtMessage").keyup(function () {
        var max = 140;
        var len = $(this).val().length;
        if (len >= max) {
            $("#charNumMessage").text(' you have reached the limit');
        } else {
            var char = max - len;
            $("#charNumMessage").text(char + ' characters left');
        }
    });
});



function debugObject(inputobject) {
    obj = inputobject;
    for (x in obj) {
        alert(x + ": " + obj[x]);
    }
}