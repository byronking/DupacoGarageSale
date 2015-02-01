$(document).ready(function () {
    $.ajaxSetup({ cache: true });
    $.getScript('//connect.facebook.net/en_UK/all.js', function () {
        FB.init({
            appId: '1538676693053081',
        });
        $('#loginbutton,#feedbutton').removeAttr('disabled');
        //FB.getLoginStatus(updateStatusCallback);
        //FB.getLoginStatus(statusChangeCallback);
        checkLoginState();
    });

    // Log out the Facebook user.
    $("#btnLogOut").click(function () {
        $("#hdnLogOut").val("true");
        var testy = $("#hdnLogOut").val();
        alert('testy: ' + testy);
        logOutFacebookUser();
    });
});

// This is called with the results from from FB.getLoginStatus().
function statusChangeCallback(response) {
    console.log('statusChangeCallback');
    console.log(response);
    // The response object is returned with a status field that lets the
    // app know the current login status of the person.
    // Full docs on the response object can be found in the documentation
    // for FB.getLoginStatus().
    alert('callback status: ' + response.status);
    if (response.status === 'connected') {
        // Logged into your app and Facebook.
        //var logout = document.getElementById('hdnLogOut').value;
        //alert(logout);

        //if (logout !== 'true') {
            signInFbUser();
        //}
    } else if (response.status === 'not_authorized') {
        // The person is logged into Facebook, but not your app.
        //document.getElementById('status').innerHTML = 'Please log in to this app.';
    } else {
        // The person is not logged into Facebook, so we're not sure if
        // they are logged into this app or not.
        //document.getElementById('status').innerHTML = 'Please log in to Facebook.';            
    }
}

// This function is called when someone finishes with the Login
// Button.  See the onlogin handler attached to it in the sample
// code below.
function checkLoginState() {
    FB.getLoginStatus(function (response) {
        alert('checkLoginState');
        statusChangeCallback(response);
    });
}

function signInFbUser() {
    alert('signing in');
    console.log('Welcome!  Fetching your information.... ');
    FB.api('/me', function (response) {
        console.log('Successful login for: ' + response.name);

        var id = response.id;
        var first_name = response.first_name;
        var last_name = response.last_name;
        var email = response.email;

        $.ajax({
            url: '/Accounts/SignInFacebookUser/',
            type: 'POST',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: { id: id, first_name: first_name, last_name: last_name, email: email },
            dataType: 'json',
            success: function (data) {
                if (data.ok)
                    window.location = data.newurl;
                else {
                    alert(data.message);
                }
            }
        });
    });
}

function logOutFacebookUser() {
    alert('logging out');
    FB.logout(function (response) {
        // User is now logged out            
    });
}