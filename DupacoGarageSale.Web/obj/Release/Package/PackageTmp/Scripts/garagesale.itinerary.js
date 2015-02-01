$(document).ready(function () {

    $("#linkAddToItinerary").click(function (e) {
        var sale_id = $("#hdnGarageSaleId").val();
        var user_id = $("#hdnUserId").val();
        var url = "/GarageSale/AddToItinerary/" + sale_id;
        $.ajax({
            type: "post",
            data: JSON.stringify({
                saleId: sale_id,
                userId: user_id
            }),
            url: url,
            //dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $('#addToItineraryModal').modal('show');
            },
            error: function (data) {
                // Do something
                //debugObject(data);
                alert('There was a problem adding your itinerary item.');
            }
        });
    });

    //$("#linkTesty").click(function

});

function debugObject(inputobject) {
    obj = inputobject;
    for (x in obj) {
        //if (x == "responseText") {
            alert(x + ": " + obj[x]);
        //}
    }
}