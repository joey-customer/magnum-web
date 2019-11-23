// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function() {
	$('.view-modal').on('click', function(e){
		e.preventDefault();
		$($(e.currentTarget).data('target')).modal();
	})
	$('.close').on('click', function(){
		$('.modal').removeClass('show');
	})

	$("#contact").validate({
		rules: {
			Name: "required",
			Subject: "required",
			Email: {
				required: true,
				email: true
			},
			Message: "required",
		},
		messages: {
			Name: {
				required: "Please enter your name",
		 	},
		 	Subject: {
		  		required: "Please enter your subject",
		 	},
		 	Email: {
				required: "Please enter email address",
				email: "Please enter a valid email address.",
			 },
			 Message: {
				required: "Please enter your message",
		   },
		},
	  });

	  $("#verifyproduct").validate({
		rules: {
			SerialNumber: "required",
			Pin: "required",
		},
		messages: {
			SerialNumber: {
				required: "Please enter serial number",
		 	},
		 	Pin: {
		  		required: "Please enter pin code",
		 	},
		},
	  });
})