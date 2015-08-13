$(document).ready(function () {

    $("#linkAddToItinerary").click(function (e) {
        $('#addToItineraryModal').modal('show');
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

    // Show the itinerary deleted message.
    if (($("#hdnItineraryDeleted").val() !== undefined)) {

        if ($("#hdnItineraryDeleted").val() == "true") {
            $("#divItineraryDeleted").removeClass("hidden");
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

    // Reload the current page after dismissing the modal.
    $('#addToFavesModal').on('hidden.bs.modal', function () {
        location.reload();
    })

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

    // Reload the current page after dismissing the modal.
    $('#removeFromFavesModal').on('hidden.bs.modal', function () {
        location.reload();
    })

    // This handles sorting the itinerary routes.
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

    // This loads the add stopover modal
    $('#btnAddStopover').click(function () {
        //alert('hi');
        var url = $('#addStopoverToItineraryModal').data('url');

        $.get(url, function (data) {
            $('#gameContainer').html(data);

            $('#addStopoverToItineraryModal').modal('show');
        });
    });

    // Show this when a user removes a sale from faves    
    if (($("#hdnFaveRemovedSuccess").val() !== undefined)) {
        //alert($("#hdnFaveRemovedSuccess").val());
        if ($("#hdnFaveRemovedSuccess").val() == "true") {
            $("#divFaveRemovedSuccess").removeClass("hidden");
        }
    }

    // Show the quick view modal
    $('#itineraryLegControls').on('click', 'a.quickView', function () {
        var saleId = $(this).attr('data-saleid');
        $('#txtSaleId').val(saleId);

        var url = "/Itinerary/QuickViewGarageSale/" + saleId;
        $.ajax({
            type: "get",
            data: JSON.stringify({
                id: saleId
            }),
            url: url,
            //dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                $('#quickSaleModal').modal('show');
                $('#saleData').html(data);
            },
            error: function (data) {
                // Do something
                //debugObject(data);
                alert('There was a problem displaying the garage sale.');
            }
        });        
    });

    // Respond to the click of the update itinerary note button
    $('#itineraryLegControls').on('click', 'a.updateItinerary', function (e) {

        e.preventDefault();
        var itinerary_leg_id = $(this).attr('data-itinerary-leg-id');
        var itinerary_waypoint_id = $(this).attr('data-itinerary-waypoint-id');
        var note;
        //alert('itinerary_leg_id: ' + itinerary_leg_id);
        //alert('waypoint_leg_id: ' + waypoint_leg_id);

        if (itinerary_leg_id == undefined) {
            // this is a waypoint
            //alert('waypoint text field id: ' + $("#txtItineraryNote" + itinerary_waypoint_id).attr('id'));
            note = $("#txtItineraryNote" + itinerary_waypoint_id).val();

            var url = "/Itinerary/UpdateWaypointNote/";
            $.ajax({
                type: "post",
                data: JSON.stringify({
                    waypoint_id: itinerary_waypoint_id,
                    note: note
                }),
                url: url,
                //dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $('#updateNoteModal').modal('show');
                    //alert('waypoint note updated!');
                },
                error: function (data) {
                    // Do something
                    debugObject(data);
                    alert('There was a problem updating the waypoint note.');
                }
            });
        }

        if (itinerary_waypoint_id == undefined) {
            //this is an itinerary leg
            //alert('itinerary leg text field id: ' + $("#txtItineraryNote" + itinerary_leg_id).attr('id'));
            note = $("#txtItineraryNote" + itinerary_leg_id).val();

            var url = "/Itinerary/UpdateItineraryNote/";
            $.ajax({
                type: "post",
                data: JSON.stringify({
                    itinerary_leg_id: itinerary_leg_id,
                    note: note
                }),
                url: url,
                //dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $('#updateNoteModal').modal('show');
                    //alert('itinerary leg note updated!');
                },
                error: function (data) {
                    // Do something
                    //debugObject(data);
                    alert('There was a problem updating the itinerary leg note.');
                }
            });
        }
    });
});

$(function () {
    $('[data-toggle="popover"]').popover()
})

// Show the add stopover modal.
function ShowStopoverModal() {
    $('#addStopoverModal').modal('show');
}

function debugObject(inputobject) {
    obj = inputobject;
    for (x in obj) {
        //if (x == "responseText") {
            alert(x + ": " + obj[x]);
        //}
    }
}