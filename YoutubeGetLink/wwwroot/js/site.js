// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var Core = function () {

    function _showAlert(message, isSuccess) {
        if (isSuccess != undefined)
            is_success_message = isSuccess;
        if (isSuccess) {
            $('.alert.alert-dismissible').removeClass('alert-danger');
            $('.alert.alert-dismissible').addClass('alert-success');
        } else {
            $('.alert.alert-dismissible').removeClass('alert-success');
            $('.alert.alert-dismissible').addClass('alert-danger');
        }
        $(".alert .alert-message").html(message);
        $('.alert').fadeIn(500);
        setTimeout(function () {
            $('.alert').fadeOut(1000);
        }, 4000);
    }

    function _loadPartialView(elementName, url, method,  data, errorFn) {
        $.ajax({
            url: url,
            type: method || 'GET',
            data: data,
            success: function (response) {
                $(elementName).html(response);
            },
            error: function () {
                if (errorFn != undefined && typeof (errorFn) == 'function') {
                    errorFn();
                }
            }
        });
    }

    return {
        alert: _showAlert,
        loadPartial: _loadPartialView
    }
}();

var storage = function () {

    function _getItem(name) {
        return JSON.parse(localStorage.getItem(name));
    }

    function _setItem(name, item) {
        return localStorage.setItem(name, JSON.stringify(item));
    }
    return {
        getItem: _getItem,
        setItem: _setItem
    }
}();

$(function () {
    $('.alert-dismissible .close').click(function () {
        $('.alert-dismissible').hide();
    });
});