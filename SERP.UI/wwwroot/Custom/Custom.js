function Success(response) {
    if (response == "Server error please contact admin team") {
        alertify.set('notifier', 'position', 'top-center');
        alertify.error(response);
    }
    else {
        alertify.set('notifier', 'position', 'top-center');
        alertify.success(response);
    }

    $("#form")[0].reset();//reset the form controll.
    $(".form-control").val('');//Clear the controll which is present inside the form.
    $("select").each(function () { this.selectedIndex = 0 });
}


function CustomDelete(id, url) {
    alertify.set('notifier', 'position', 'top-center');
    var defered = $.Deferred();
    alertify.confirm("Are you sure want to Inactivate this record ?", function () {
        $.get(url, { Id: id }, function (response) {
            alertify.success('Record deactivated successfully')
            return defered.promise();

        });
    }, function () {
        alertify.error('There is some server Error please contact admin team .')
    });
}

function CustomDeleteRecord(id, getUrl, deleteUrl, event) {
    alertify.set('notifier', 'position', 'top-center');
    alertify.confirm("Are you sure want to delete the record ?", function () {

        var thisId = $(event);
        thisId.button('loading');

        $.get(deleteUrl, { id: id }, function (response) {
            alertify.success("Record deleted successfully");
        }).done(function () {
            $.get(getUrl, function (response) {
                $("#divPartialData").html(response);
            }).done(function () {
                $("#tblDataList").DataTable({
                    fixedHeader: true,
                    select: true,
                    responsive: true
                });
            });

            $(".form-control").val('');//Clear the controll which is present inside the form.
        });
    }, function () {
        alertify.warning("You cancel the delete.");
    });
}

function UpdateCustomRecord(id, updateUrl, event) {
    var thisId = $(event);
    thisId.button('loading');
    $.get(updateUrl, { id: id }, function (response) {
        $("#divSERP").html(response);
        $("#divModalPop").modal('hide');
        $('.modal-backdrop').remove();
        $("#divSerpBody").removeClass('modal-open');    
    }).done(function () {
        $("#form")[0].reset();
    });
}

function GetCustomRecord(getUrl) {
    var table;
    $("#loading").removeAttr('style');
    $.get(getUrl, function (response) {
        $("#divPartialData").html(response);
        $("#divModalPop").modal('show');
        setTimeout(function () {
          table=  $("#tblDataList").DataTable({
                fixedHeader: true,
                select: true,
                responsive: true
            });
        }, 300)
    }).done(function () {
        $("#loading").attr('style', 'display:none');
    });
}

function NewCustomRecord(url) {
    $.get(url, function (response) {
        $("#divSERP").html(response);
        $("#divModalPop").modal('hide');
        $('.modal-backdrop').remove();
    });
}

function AjaxOnBegin() {
    
    $("#divSERP").addClass('loading');
    $("#loading").removeClass('loading');
}
function AjaxComplete() {
    $("#divSERP").removeClass('loading');
    $("#loading").addClass('loading');
}

function CustomFormSubmitBegin() {
    $("#divSERP").addClass('loading');
    $("#loading").removeClass('loading');
}

function CustomFormSubmitComplete() {
    $("#divSERP").removeClass('loading');
    $("#loading").addClass('loading');
   
}

function GetInfo() {
    alert("Information")
}

//use onfocus in textbox
function RemoveZero(e) {
    if (e.value == "0") {
        e.value = "";
    }
}
//use onblur in textbox
function SetZeroIfEmpty(e) {
    if (e.value == "") {
        e.value = "0";
    }
}

