$(document).ready(function () {

    $('#login-form').validate({
        rules: {
            'Input.Email': {
                required: true,
                email: true,
            },
            'Input.Password': {
                required: true,
                minlength: 5
            }
        },
        messages: {
            'Input.Email': {
                required: "Please enter a email address",
                email: "Please enter a vaild email address"
            },
            'Input.Password': {
                required: "Please provide a password",
                minlength: "Your password must be at least 5 characters long"
            }
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-floating').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });
});