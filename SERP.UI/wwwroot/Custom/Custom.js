function Success(response) {
    debugger;
    alertify.set('notifier', 'position', 'top-right');
    alertify.success(response);
    //$("#form")[0].reset();
}


function CustomDelete(id, url) {
    alertify.set('notifier', 'position', 'top-right');
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
    alertify.set('notifier', 'position', 'top-right');
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
    }).done(function () {
       
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
        setTimeout(function () {
            table.columns.adjust().responsive.recalc();
        }, 500)
        
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
    debugger;
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