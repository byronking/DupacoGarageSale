$(document).ready(function () {

    $("#linkAddToItinerary").click(function (e) {
        $('#addToItineraryModal').modal('show');

        //var sale_id = $("#hdnGarageSaleId").val();
        //var user_id = $("#hdnUserId").val();
        //var url = "/GarageSale/AddToItinerary/" + sale_id;
        //$.ajax({
        //    type: "post",
        //    data: JSON.stringify({
        //        saleId: sale_id,
        //        userId: user_id
        //    }),
        //    url: url,
        //    //dataType: 'json',
        //    contentType: 'application/json; charset=utf-8',
        //    success: function (data) {
        //        $('#addToItineraryModal').modal('show');
        //    },
        //    error: function (data) {
        //        // Do something
        //        //debugObject(data);
        //        alert('There was a problem adding your itinerary item.');
        //    }
        //});
    });

    $("#linkRemoveFromItinerary").click(function (e) {
        var sale_id = $("#hdnGarageSaleId").val();
        var itinerary_id = $("#hdnItineraryId").val();
        var url = "/Itinerary/RemoveFromItinerary/";
        $.ajax({
            type: "post",
            data: JSON.stringify({
                saleId: sale_id,
                itineraryId: itinerary_id
            }),
            url: url,
            //dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $('#removeFromItineraryModal').modal('show');
            },
            error: function (data) {
                // Do something
                //debugObject(data);
                alert('There was a problem removing your itinerary item.');
            }
        });
    });

    // Show the itinerary leg deleted message.
    if (($("#hdnLegDeletedMessage").val() !== undefined)) {

        if ($("#hdnLegDeletedMessage").val() == "false") {
            $("#divLegDeletedMessage").removeClass("hidden");
        }
    }

    // Add the garage sale to the users' faves.
    $("#linkAddToFaves").click(function (e) {
        var sale_id = $("#hdnGarageSaleId").val();
        var user_id = $("#hdnUserId").val();
        var url = "/GarageSale/AddSaleToFaves/" + sale_id;
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
                $('#addToFavesModal').modal('show');
                //location.href = "/GarageSale/ViewGarageSale/" + sale_id;
            },
            error: function (data) {
                // Do something
                //debugObject(data);
                alert('There was a problem adding the garage sale to faves.');
            }
        });
    });

    // Remove the garage sale from the users' faves.
    $("#linkRemoveFromFaves").click(function (e) {
        var sale_id = $("#hdnGarageSaleId").val();
        var user_id = $("#hdnUserId").val();
        var fave_id = $("#hdnFaveSaleId").val();
        var url = "/GarageSale/RemoveSaleFromFaves/" + sale_id;
        $.ajax({
            type: "post",
            data: JSON.stringify({
                saleId: sale_id,
                faveId: fave_id
            }),
            url: url,
            //dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $('#removeFromFavesModal').modal('show');
                //location.href = "/GarageSale/ViewGarageSale/" + sale_id;
            },
            error: function (data) {
                // Do something
                //debugObject(data);
                alert('There was a problem removing the garage sale from your faves.');
            }
        });
    });

    $(function () {
        $("#sortable").sortable({
            stop: function (event, ui) {
                var data = "";
                var newWayPointsList = [];

                $("#sortable li").each(function (i, el) {
                    var p = $(el).html();

                    //var field = $(this).find("input").attr('id');
                    var field = $(this).find("input:hidden").val();
                    //alert(field);

                    newWayPointsList.push(field);
                });

                $("#hdnNewItineraryLegs").val(newWayPointsList);
                //alert('new order: ' + $("#hdnNewItineraryLegs").val());
            }
        })
    });
});

$(function () {
    $('[data-toggle="popover"]').popover()
})

function debugObject(inputobject) {
    obj = inputobject;
    for (x in obj) {
        //if (x == "responseText") {
            alert(x + ": " + obj[x]);
        //}
    }
}