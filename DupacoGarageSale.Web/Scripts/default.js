$(document).ready(function () {

    jQuery.support.cors = true;

    // Handle the character count for the garage sale description for IE browsers < 10.
    //$().maxlength();

    var ie = (function () {
        var undef,
            v = 3,
            div = document.createElement('div'),
            all = div.getElementsByTagName('i');

        while (
            div.innerHTML = '<!--[if gt IE ' + (++v) + ']><i></i><![endif]-->',
            all[0]
        );

        return v > 4 ? v : undef;
    }());

    //alert('ie version: ' + ie);

    if (ie < 8) {
        alert('The Dupaco Garage Sales site runs best in a modern browser.  Please update your browser for the best experience! Update to the latest version of Internet Explorer or try Mozilla Firefox (https://www.mozilla.org/en-US/firefox/new/) or Google Chrome (http://www.google.com/chrome/)');
        location.href = 'http://www.dupaco.com';

        //alert('Hello, ' + ie);
    }

    // Preserve the address when searching.
    if ($('#hdnSavedAddress').val() !== "") {
        $('#txtAddress').text($('#hdnSavedAddress').val());
    }

    // Show the error message if there is an existing account
    if ($("#hdnExistingAccountError").val() == "true") {
        $("#existingAccountError").removeClass('hidden');
    }

    // Show an error if a user does not enter a user name or password.
    $("#btnSignIn").click(function () {
        if ($("#txtUserName").val() == "")
        {
            $("#txtUserName").popover({
                content: "Enter your user name!", placement: "bottom",
                template: '<div class="popover alert alert-danger alert-dismissible fade in" role="alert" role="tooltip"><div class="arrow"></div><div class="popover-content"></div></div>'
            });
            $("#txtUserName").popover('show');
        }

        if ($("#txtPassword").val() == "") {
            $("#txtPassword").popover({
                content: "Enter your password!", placement: "bottom",
                template: '<div class="popover alert alert-danger alert-dismissible fade in" role="alert" role="tooltip"><div class="arrow"></div><div class="popover-content"></div></div>'
            });
            $("#txtPassword").popover('show');
        }
    });

    // This handles the profile picture upload.
    function readImage(file) {

        var reader = new FileReader();
        var image = new Image();

        reader.readAsDataURL(file);
        reader.onload = function (_file) {
            image.src = _file.target.result;              // url.createObjectURL(file);
            image.onload = function () {
                var w = this.width,
                    h = this.height,
                    t = file.type,                           // ext only: // file.type.split('/')[1],
                    n = file.name,
                    s = ~~(file.size / 1024) + 'KB';
                $('#uploadPreview').append('<img src="' + this.src + '"> ' + w + 'x' + h + ' ' + s + ' ' + t + ' ' + n + '<br>');
            };

            //alert(file.size / 1024);

            if ((file.size / 1024) > 5000) {
                //alert('too big!');
                $("#picSizeWarning").removeClass("hidden");
                $("#btnSaveProfile").attr("disabled", true);
            }
            else {
                $("#btnSaveProfile").attr("disabled", false);
            }

            image.onerror = function () {
                alert('Invalid file type: ' + file.type);
            };
        };
    }

    $("#fileUpload").change(function (e) {
        if (this.disabled) return alert('File upload not supported!');
        var F = this.files;
        if (F && F[0]) for (var i = 0; i < F.length; i++) readImage(F[i]);

        if (ie < 10) {

            // Show the old browser alert.
            $("#oldBlowserAlert").removeClass('hidden');
        }       
    });

    // This provides validation for the user profile fields.
    $("#btnSaveProfile").click(function (e) {
        if ($('#txtFirstName').val() == "") {
            $("#txtFirstName").popover({
                content: "Please enter a first name", placement: "top",
                template: '<div class="popover alert alert-danger alert-dismissible fade in" role="alert" role="tooltip"><div class="arrow"></div><div class="popover-content"></div></div>'
            });
            $("#txtFirstName").popover('show');
            e.preventDefault();
        }

        if ($('#txtLastName').val() == "") {
            $("#txtLastName").popover({
                content: "Please enter a last name", placement: "top",
                template: '<div class="popover alert alert-danger alert-dismissible fade in" role="alert" role="tooltip"><div class="arrow"></div><div class="popover-content"></div></div>'
            });
            $("#txtLastName").popover('show');
            e.preventDefault();
        }
        if ($('#txtAddress1').val() == "") {
            $("#txtAddress1").popover({
                content: "Please enter a house number & street", placement: "top",
                template: '<div class="popover alert alert-danger alert-dismissible fade in" role="alert" role="tooltip"><div class="arrow"></div><div class="popover-content"></div></div>'
            });
            $("#txtAddress1").popover('show');
            e.preventDefault();
        }

        if ($('#txtCity').val() == "") {
            $("#txtCity").popover({
                content: "Please enter a city", placement: "top",
                template: '<div class="popover alert alert-danger alert-dismissible fade in" role="alert" role="tooltip"><div class="arrow"></div><div class="popover-content"></div></div>'
            });
            $("#txtCity").popover('show');
            e.preventDefault();
        }

        if ($('#ddlStatesList option:selected').text() == "Select...") {
            $("#ddlStatesList").popover({
                content: "Please enter a state", placement: "top",
                template: '<div class="popover alert alert-danger alert-dismissible fade in" role="alert" role="tooltip"><div class="arrow"></div><div class="popover-content"></div></div>'
            });
            $("#ddlStatesList").popover('show');
            e.preventDefault();
        }

        if ($('#txtZip').val() == "") {
            $("#txtZip").popover({
                content: "Please enter a zip code", placement: "top",
                template: '<div class="popover alert alert-danger alert-dismissible fade in" role="alert" role="tooltip"><div class="arrow"></div><div class="popover-content"></div></div>'
            });
            $("#txtZip").popover('show');
            e.preventDefault();
        }
    });

    // This provides validation for the contact us form.
    $("#btnSendContactUsMessage").click(function (e) {
        if ($('#txtContactName').val() == "") {
            $('#nameValidationMessage').removeClass('invisible');
            e.preventDefault();
        }
        else {
            $('#nameValidationMessage').addClass('invisible');
        }

        if ($('#txtContactEmail').val() == "") {
            $('#emailValidationMessage').removeClass('invisible');
            e.preventDefault();
        }
        else {
            $('#emailValidationMessage').addClass('invisible');
        }
        if ($('#txtContactUsMessage').val() == "") {
            $('#contactValidationMessage').removeClass('invisible');
            e.preventDefault();
        }
        else {
            $('#contactValidationMessage').addClass('invisible');
        }
    });

    $("#txtContactUsMessage").keyup(function () {
        var max = 140;
        var len = $(this).val().length;
        //alert('length: ' + len);
        if (len >= max) {
            $("#charNumContactMessage").text(' you have reached the limit');
        } else {
            var char = max - len;
            $("#charNumContactMessage").text(char + ' characters left');
        }
    });

    // This clears the address search field when there's no saved address.
    //alert($("#hdnNoAddress").val());
    if ($("#hdnNoAddress").val() == "true") {
        $("#txtAddress").val('');
    }

    // This shows a popover message if the user does not enter an address when searching.
    $("#btnFilter").click(function (e) {
        if ($("#txtSearchCriteria").val() == "") {
            $("#txtSearchCriteria").popover({
                content: "Enter search criteria", placement: "bottom",
                template: '<div class="popover alert alert-danger alert-dismissible fade in" role="alert" role="tooltip"><div class="arrow"></div><div class="popover-content"></div></div>'
            });
            $("#txtSearchCriteria").popover('show');
            $("#txtSearchCriteria").focus();
            e.preventDefault();
        }

        if ($("#txtAddress").val() == "") {
            $("#txtAddress").popover({
                content: "Enter a starting point", placement: "bottom",
                template: '<div class="popover alert alert-danger alert-dismissible fade in" role="alert" role="tooltip"><div class="arrow"></div><div class="popover-content"></div></div>'
            });
            $("#txtAddress").popover('show');
            $("#txtAddress").focus();
            e.preventDefault();
        }

        if ($("#from").val() == "") {
            $("#from").popover({
                content: "Enter a start date", placement: "bottom",
                template: '<div class="popover alert alert-danger alert-dismissible fade in" role="alert" role="tooltip"><div class="arrow"></div><div class="popover-content"></div></div>'
            });
            $("#from").popover('show');
            $("#from").focus();
            e.preventDefault();
        }

        if ($("#to").val() == "") {
            $("#to").popover({
                content: "Enter an end date", placement: "bottom",
                template: '<div class="popover alert alert-danger alert-dismissible fade in" role="alert" role="tooltip"><div class="arrow"></div><div class="popover-content"></div></div>'
            });
            $("#to").popover('show');
            $("#to").focus();
            e.preventDefault();
        }        
    });

    // Focus on the search area of the page.
    //alert('value: ' + $('#hdnSearchButtonClicked').val());
    if ($('#hdnSearchButtonClicked').val() == "true") {
        window.scrollTo(0, 1250);
    }

    // Show the successful save message.
    if (($("#hdnSaveMessage").val() !== undefined)) {
        //alert($("#hdnSaveMessage").val());

        if ($("#hdnSaveMessage").val() == "false") {
            $("#divSaveMessage").removeClass("hidden");
            location.href = "#hotpicks";
            //$("#hotpicks").focus();
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

    $("#from").datepicker({
        defaultDate: "+1w",
        changeMonth: true,
        numberOfMonths: 1,
        onClose: function (selectedDate) {
            $("#to").datepicker("option", "minDate", selectedDate);
        }
    });
    $("#to").datepicker({
        defaultDate: "+1w",
        changeMonth: true,
        numberOfMonths: 1,
        onClose: function (selectedDate) {
            $("#from").datepicker("option", "maxDate", selectedDate);
        }
    });

    // Handle validation for selecting the dates.
    $("#ddlCommunity").change(function () {
        if ($("#ddlCommunity option:selected").text() == "Dubuque, IA") {
            $("#txtSaleDateOne").val("4/23/2015");
            $("#txtSaleDateTwo").val("4/24/2015");
            $("#txtSaleDateThree").val("4/25/2015");
            $("#txtSaleDateFour").val("4/26/2015");
        }
        else if ($("#ddlCommunity option:selected").text() == "Manchester, IA") {
            $("#txtSaleDateOne").val("4/23/2015");
            $("#txtSaleDateTwo").val("4/24/2015");
            $("#txtSaleDateThree").val("4/25/2015");
            $("#txtSaleDateFour").val("4/26/2015");
        }
        else if ($("#ddlCommunity option:selected").text() == "Platteville, WI") {
            $("#txtSaleDateOne").val("5/7/2015");
            $("#txtSaleDateTwo").val("5/8/2015");
            $("#txtSaleDateThree").val("5/9/2015");
            $("#txtSaleDateFour").val("5/10/2015");
        }
        else {
            $("#txtSaleDateOne").val(' ');
            $("#txtSaleDateTwo").val(' ');
            $("#txtSaleDateThree").val(' ');
            $("#txtSaleDateFour").val(' ');
        }
    });

    // Set the range of available times.
    $('#dayOneStart').timepicker({ 'scrollDefault': '800' });
    $('#dayOneEnd').timepicker({ 'scrollDefault': '800' });
    $('#dayTwoStart').timepicker({ 'scrollDefault': '800' });
    $('#dayTwoEnd').timepicker({ 'scrollDefault': '800' });
    $('#dayThreeStart').timepicker({ 'scrollDefault': '800' });
    $('#dayThreeEnd').timepicker({ 'scrollDefault': '800' });
    $('#dayFourStart').timepicker({ 'scrollDefault': '800' });
    $('#dayFourEnd').timepicker({ 'scrollDefault': '800' });

    // Character count for the garage sale description.
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

    // Validation for the garage sale dates and times.
    $("#btnSaveGargeSale").click(function (e) {

        if (($('#dayOneStart').val() == "" || $('#dayOneEnd').val() == "") && ($('#dayTwoStart').val() == "" || $('#dayTwoEnd').val() == "") && ($('#dayThreeStart').val() == "" || $('#dayThreeEnd').val() == "") && ($('#dayFourStart').val() == "" || $('#dayFourEnd').val() == "")) {
            $("#alertDatesTimes").html('Please enter at least one starting and ending date for your sale');
            $("#alertDatesTimes").removeClass("hidden");
            $("#ddlCommunity").focus();
            e.preventDefault();
        }
        else {
            $("#alertDatesTimes").addClass("hidden");
        }

        if ($("#ddlCommunity").val() == 0) {
            $("#alertDatesTimes").html('Please select a community');
            $("#alertDatesTimes").removeClass("hidden");
            $("#ddlCommunity").focus();
            e.preventDefault();
        }
    });

    // Set the selected item categories.
    if ($("#hdnSelectedCategories").val() !== undefined) {
        var selectedCategories = new Array();
        selectedCategories = $("#hdnSelectedCategories").val().split(',');
        //alert(selectedCategories);

        for (x in selectedCategories) {
            $("input:checkbox[value=" + selectedCategories[x] + "]").attr("checked", true);

            var panelCollapse = $("input:checkbox[value=" + selectedCategories[x] + "]").closest('div.panel-collapse').attr('id');
            var panelName = $(panelCollapse).selector;
            $(document.getElementById(panelName)).collapse('show');
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

    // This provides validation for the hot picks fields. 
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

                        e.preventDefault();
                        return true;
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

    // Character counter for the hot pick description.
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

    // Validation for the blog post.
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
    
    // This counts the characters for the blog post message.
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
            $('#passwordResetUserNameAlert').removeClass('hidden');
            e.preventDefault();
        }
        else {
            $('#passwordResetUserNameAlert').addClass('hidden');
        }

        if ($('#txtPasswordResetEmail').val() == "") {
            $('#passwordResetEmailAlert').removeClass('hidden');
            e.preventDefault();
        }
        else {
            $('#passwordResetEmailAlert').addClass('hidden');
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

    // This clears the checked selections.
    $("#btnClearSearch").click(function () {
        $("#accordion input[type=checkbox]").each(function () {
            $(this).prop("checked", false);
        });
    });

    // Show an error if the user doesn't enter text when using the search bar.
    $("#btnSearchBarFind").click(function (e) {
        if ($("#txtSearchCriteria").val() == "") {
            $("#searchValidation").removeClass('hidden');
            e.preventDefault();
        }
        else {
            $("#hdnSearchBarCriteria").val($("#txtSearchCriteria").val());
        }
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

    // Validation for the change password modal.
    $("#btnChangePassword").click(function (e) {
        if ($("#txtPassword").val() == "") {
            $("#lblPasswordError").removeClass('hidden');
            e.preventDefault();
        }
        else {
            $("#lblPasswordError").addClass('hidden');
        }

        if ($("#txtConfirmPassword").val() == "") {
            $("#lblConfirmPasswordError").removeClass('hidden');
            e.preventDefault();
        }
        else {
            $("#lblConfirmPasswordError").addClass('hidden');
        }

        if ($("#txtPassword").val() != $("#txtConfirmPassword").val()) {
            $("#lblConfirmPasswordError").html('The two passwords must match.');
            $("#lblConfirmPasswordError").removeClass('hidden');
            //alert('nope!');
            e.preventDefault();
        }
    });
});

// This adds the Dupaco locations to the map.
function AddDupacoLocations() {

    //var addresses = ['3421 W 9th St Waterloo IA 50702', '1334 Flammang Dr Waterloo IA 50702', '218 W Mullan Ave Waterloo IA 50701', '3301 Cedar Heights Dr Cedar Falls IA 50613',
    //    '110 35th St Dr SE Cedar Rapids IA 52403', '3131 Williams Blvd SW Cedar Rapids IA 52404', '501 4th Ave SE Cedar Rapids IA 52401', '3999 Pennsylvania Ave Dubuque IA 52002',
    //    '3299 Hillcrest Rd Dubuque IA 52004', '1465 Sycamore St Dubuque IA 52001', '2245 Flint Hill Dr Dubuque IA 52003', '5865 Saratoga Rd Asbury IA 52002',
    //    '400 S Locust St Dubuque IA 52003', '807 9th St SE Dyersville IA 52040', '1200 W Main St Manchester IA 52057', '11375 Oldenburg Ln Galena IL 61036',
    //    '1100 E. Business Hwy. 151 Platteville WI 53818', '503 U.S. 30 Carroll IA 51401'];

    var addresses = { };
    addresses[0] = { address: '3421 West 9th Street Waterloo IA 50702', lat: 42.461057, long: -92.346031, page: 'https://www.dupaco.com/access/dupaco-locations/3421-w-9th-st-waterloo-ia.html' };
    addresses[1] = { address: '3999 Pennsylvania Ave Dubuque IA 52002', lat: 42.501910, long: -90.739733, page: 'https://www.dupaco.com/access/dupaco-locations/3999-pennsylvania-ave-dubuque-ia.html' };
    addresses[2] = { address: '11375 Oldenburg Ln Galena IL 61036', lat: 42.436796, long: -90.450703, page: 'https://www.dupaco.com/access/dupaco-locations/11375-oldenburg-ln-galena-il.html' };
    addresses[3] = { address: '5865 Saratoga Rd Asbury IA 52002', lat: 42.509365, long: -90.757738, page: 'https://www.dupaco.com/access/dupaco-locations/5865-saratoga-rd-ia.html' };
    addresses[4] = { address: '400 S Locust St Dubuque IA 52003', lat: 42.486759, long: -90.663007, page: 'https://www.dupaco.com/access/dupaco-locations/400-s-locust-st-dubuque-ia.html' };
    //addresses[5] = { address: '1334 Flammang Dr Waterloo IA 50702', lat: 42.458591, long: -92.330485, page: 'https://www.dupaco.com/access/dupaco-locations/1334-flammang-dr-waterloo-ia.html' };
    //addresses[6] = { address: '218 W Mullan Ave Waterloo IA 50701', lat: 42.498419, long: -92.346106, page: 'https://www.dupaco.com/access/dupaco-locations/218-w-mullan-ave-waterloo-ia.html' };
    //addresses[7] = { address: '3301 Cedar Heights Dr Cedar Falls IA 50613', lat: 42.508496, long: -92.411652, page: 'https://www.dupaco.com/access/dupaco-locations/3301-cedar-heights-dr-cedar-falls-ia.html' };
    //addresses[8] = { address: '110 35th St Dr SE Cedar Rapids IA 52403', lat: 42.013743, long: -91.634661, page: 'https://www.dupaco.com/access/dupaco-locations/110-35th-st-dr-se-cedar-rapids-ia.html' };
    //addresses[9] = { address: '3131 Williams Blvd SW Cedar Rapids IA 52404', lat: 41.959942, long: -91.712232, page: 'https://www.dupaco.com/access/dupaco-locations/3131-williams-blvd-sw-cedar-rapids-ia.html' };
    //addresses[10] = { address: '501 4th Ave SE Cedar Rapids IA 52401', lat: 41.978031, long: -91.662357, page: 'https://www.dupaco.com/access/dupaco-locations/501-4th-ave-se-cedar-rapids-ia.html"' };
    //addresses[11] = { address: '3299 Hillcrest Rd Dubuque IA 52004', lat: 42.506469, long: -90.719520, page: 'https://www.dupaco.com/access/dupaco-locations/3299-hillcrest-rd-dubuque-ia.html' };
    //addresses[12] = { address: '1465 Sycamore St Dubuque IA 52001', lat: 42.508943, long: -90.660819, page: 'https://www.dupaco.com/access/dupaco-locations/1465-sycamore-st-dubuque-ia.html' };
    //addresses[13] = { address: '2245 Flint Hill Dr Dubuque IA 52003', lat: 42.459508, long: -90.675154, page: 'https://www.dupaco.com/access/dupaco-locations/2245-flint-hill-dr-dubuque-ia.html' };
    //addresses[14] = { address: '807 9th St SE Dyersville IA 52040', lat: 42.478004, long: -91.113549, page: 'https://www.dupaco.com/access/dupaco-locations/807-9th-st-se-dyersville-ia.html' };
    //addresses[15] = { address: '1200 W Main St Manchester IA 52057', lat: 42.483192, long: -91.474226, page: 'https://www.dupaco.com/access/dupaco-locations/1200-w-main-st-manchester-ia.html' };
    //addresses[16] = { address: '1100 E. Business Hwy. 151 Platteville WI 53818', lat: 42.732090, long: -90.460371, page: 'https://www.dupaco.com/access/dupaco-locations/1100-e-business-hwy-151-platteville-wi.html' };
    //addresses[17] = { address: '503 U.S. 30 Carroll IA 51401', lat: 42.064114, long: -94.861233, page: 'https://www.dupaco.com/access/dupaco-locations/503-us-30-carroll-ia.html' };

    return addresses;
}

var maxLength = 100;
function enforceMaxLength(ta) {
    if (ta.value.length > maxLength) {
        ta.value = ta.value.substring(0, maxLength);
    }
}

function debugObject(inputobject) {
    obj = inputobject;
    for (x in obj) {
            alert(x + ": " + obj[x]);
    }
}