// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function() {
    $('.view-modal').on('click', function(e){
        $($(e.currentTarget).data('target')).modal();
    })
    $('.close').on('click', function(){
        $('.modal').removeClass('show');
    })
})