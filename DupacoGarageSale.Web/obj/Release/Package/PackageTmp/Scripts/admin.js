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

    // Show the blog post view fields.
    if ($("#hdnShowBlogPosts").val() == "true") {
        //alert('value: ' + $("#hdnShowBlogPosts").val());
        $("#showBlogPostDiv").removeClass('hidden');
    }

    // Show the successful save message.
    if (($("#hdnSaveMessage").val() !== undefined)) {
        //alert($("#hdnSaveMessage").val());

        if ($("#hdnSaveMessage").val() == "false") {
            $("#divSaveMessage").removeClass("hidden");
        }
    }

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

    // Character counter for the admin headline news.
    $("#txtHeadlineNews").keyup(function () {
        var max = 500;
        var len = $(this).val().length;
        //alert('length: ' + len);
        if (len >= max) {
            $("#charNumHeadlineNews").text(' you have reached the limit');
        } else {
            var char = max - len;
            $("#charNumHeadlineNews").text(char + ' characters left');
        }
    });

    // Validation for the headline news
    $("#btnPublishMessage").click(function (e) {
        if ($("#txtHeadlineNews").val() == "")
        {
            $("#headlineNewsValidationMessage").removeClass('invisible');
            e.preventDefault();
        }
    });
});
