$(document).ready(function () {
    
    // Show the user edit fields.
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

    // This responds to clicking the reply link on the message center page.
    $('#grdContactUsMessages').on('click', 'a.replyLink', function () {
        var value = $(this).attr('data-value');
        //alert('value: ' + value);

        $("#hdnMessageId").val(value);
    });

    //$("#linkReplyToModal").click(function (e) {
    //    //alert('hi');
    //    //$('div[id^="list_"]')
    //    var hdnMessageId = $('input[id^="hdnMessageId"').val();
    //    $("#txtMessageId").text(hdnMessageId);
    //    alert(hdnMessageId);
    //    e.preventDefault();
    //});

    // Set the range of available times.
    $('#dayOneStart').timepicker({ 'scrollDefault': '800' });
    $('#dayOneEnd').timepicker({ 'scrollDefault': '2000' });
    $('#dayTwoStart').timepicker({ 'scrollDefault': '800' });
    $('#dayTwoEnd').timepicker({ 'scrollDefault': '2000' });
    $('#dayThreeStart').timepicker({ 'scrollDefault': '800' });
    $('#dayThreeEnd').timepicker({ 'scrollDefault': '2000' });
    $('#dayFourStart').timepicker({ 'scrollDefault': '800' });
    $('#dayFourEnd').timepicker({ 'scrollDefault': '2000' });

    // Set the selected item categories.
    if ($("#hdnSelectedCategories").val() !== undefined) {
        var selectedCategories = new Array();
        selectedCategories = $("#hdnSelectedCategories").val().split(',');

        for (x in selectedCategories) {
            $("input:checkbox[value=" + selectedCategories[x] + "]").attr("checked", true);
            var panelCollapse = $("input:checkbox[value=" + selectedCategories[x] + "]").closest('div.panel-collapse').attr('id');
            var panelName = $(panelCollapse).selector;
            $(document.getElementById(panelName)).collapse('show');
        }
    }
});

function debugObject(inputobject) {
    obj = inputobject;
    for (x in obj) {
        alert(x + ": " + obj[x]);
    }
}
