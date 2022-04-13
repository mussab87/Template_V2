// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});

showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    })
}

jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all').html(res.html)
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

jQueryAjaxDelete = form => {
    Swal.fire({
        title: 'هل أنت متأكد من عملية الحذف ؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'الغاء',
        confirmButtonText: 'نعم'
    }).then((result) => {
        if (result.isConfirmed) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    Swal.fire({
                        title: "تم الحذف بنجاح",
                        icon: 'success',
                        //confirmButtonText: 'موافق',
                        showConfirmButton: false,
                        showCloseButton: false
                    })
               location.reload();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
        }

    })


    //prevent default form submit event
    return false;
}

//Manage Roles******************************************

$(function () {
    var placeHolderElement = $('#PlaceHolderHere');

    $('button[data-toggle="ajax-modal"').click(function (event) {
        //get url action from data-url from the button tag
        var url = $(this).data('url');
        var decodedUrl = decodeURIComponent(url);
        $.get(decodedUrl).done(function (data) {
            placeHolderElement.html(data);
            placeHolderElement.find('.modal').modal('show');
        })

    })

    placeHolderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');

        var actionName = form.attr('action');
        var controllerName = form.attr('controller');

        var actionUrl = "/" + controllerName + "/" + actionName;

        var sendData = form.serialize();
        var userId = document.getElementById("SelecteduserId").value;


        $.post(actionUrl, sendData + "&userId=" + userId).done(function (data) {

            window.location.reload();
            placeHolderElement.find('.modal').modal('hide');
        })
    })
})

//*******************************************************

//Manage Users******************************************

$(function () {
    var placeHolderElement = $('#PlaceHolderHereUser');

    $('button[data-toggle="ajax-modal"').click(function (event) {
        //get url action from data-url from the button tag
        var url = $(this).data('url');
        var decodedUrl = decodeURIComponent(url);
        $.get(decodedUrl).done(function (data) {
            placeHolderElement.html(data);
            placeHolderElement.find('.modal').modal('show');
        })

    })

    placeHolderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');

        var actionName = form.attr('action');
        var controllerName = form.attr('controller');

        var actionUrl = "/" + controllerName + "/" + actionName;

        var sendData = form.serialize();
        var roleId = document.getElementById("SelecteduserId").value;


        $.post(actionUrl, sendData + "&userId=" + roleId).done(function (data) {
            alert("تم إضافة الصلاحيات بنجاح")
            window.location.reload();
            placeHolderElement.find('.modal').modal('hide');
        })
    })
})

//*******************************************************
