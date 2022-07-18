$(document).ready(function () {

    var ajaxUrl = $('#HidRootUrl').val();

    $.validator.addMethod("userNotExist", function (value, element) {
        var result = false;

        $.ajax({
            url: ajaxUrl + '/Users/UserExist',
            type: "GET",
            async: false,
            data: {
                username: value
            },
            success: function (response) {
                result = response ? false : true;
            },
            error: function (xhr, status, error) {
                return false;
            }
        });

        return result;
    }, "User Exist");

    initTable();

    $('select[name="SelectedCategoryId"]').on('change', function () {
        var selectedCategoryId = $(this).val();
        if (selectedCategoryId === '') return;
        var selectedCategory = $("option:selected", this);

        $('#tbodyCategories').append(`<tr draggable="true" style="border-bottom:1px solid #e2e5e8; height: 55px; cursor:all-scroll;" ondragstart="start()" ondragover="dragover()">
                                    <td class="hidden">${selectedCategoryId}</td>
                                    <td>${selectedCategory[0].innerText}</td>
                                    <td style="text-align:center;cursor:default;"><i class="far fa-trash-alt fa-lg"></i></td>
                                </tr >
            `);

        $('#tbodyCategoriesEdit').append(`<tr draggable="true" style="border-bottom:1px solid #e2e5e8; height: 55px; cursor:all-scroll;" ondragstart="start()" ondragover="dragover()">
                                    <td class="hidden">${selectedCategoryId}</td>
                                    <td>${selectedCategory[0].innerText}</td>
                                    <td style="text-align:center;cursor:default;"><i class="far fa-trash-alt edit fa-lg"></i></td>
                                </tr >
            `);

        $('#tblCategories').removeClass('hidden');
        $('#tblCategoriesEdit').removeClass('hidden');
    });

    $(document).on("click", '.fa-trash-alt', function () {
        $(this).closest('tr').remove();

        if ($(this).hasClass('edit')) {
            var rowCount = $('#tbodyCategoriesEdit tr').length;
            if (rowCount === 0) {
                $('#tblCategoriesEdit').addClass('hidden');
            }
        }
        else {
            var rowCount = $('#tbodyCategories tr').length;
            if (rowCount === 0) {
                $('#tblCategories').addClass('hidden');
            }

        }
    });

    $('#submitAddForm').on('click', function () {
        var form = $('#addUserForm');

        var categories = [];

        $('#tblCategories tbody tr').each(function () {
            $this = $(this);
            var categoryId = $this.find("td:eq(0)").text().trim();

            categories.push(categoryId);
        });

        var validator = form.validate({
            rules: {
                'username': {
                    required: true,
                    noSpace: true,
                    userNotExist: true
                },
                'firstname': {
                    required: true,
                    noSpace: true
                },
                'lastname': {
                    required: true,
                    noSpace: true
                },
                'SelectedOrganizationId': {
                    required: true
                },
                'email': {
                    required: true,
                    noSpace: true,
                    email: true
                },
                'roles[]': {
                    required: true
                }
            },
            messages: {
                'username': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val(),
                    userNotExist: $('#hidUsernameExist').val()
                },
                'firstname': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'lastname': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'SelectedOrganizationId': {
                    required: $('#hidRequired').val()
                },
                'email': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val(),
                    email: $('#hidInvalidEmail').val()
                },
                'roles[]': {
                    required: $('#hidRoleSelectionRequired').val()
                }
            },
            errorElement: 'span',
            errorPlacement: function (error, element) {
                error.addClass('invalid-feedback');
                element.closest('.form-group').append(error);
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass('is-invalid');
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass('is-invalid');
            }
        });

        if (form.valid()) {

            var disabled = form.find(':input:disabled').removeAttr('disabled');
            var formData = $(form).serialize();
            formData = formData + '&categories=' + categories;

            disabled.attr('disabled', 'disabled');

            // Submit the form using AJAX.
            $.ajax({
                url: ajaxUrl + '/Users/CreateUser',
                type: "POST",
                dataType: "json",
                data: formData,
                async: false,
                success: function (response) {
                    validator.resetForm();
                    userDone();
                },
                error: function (xhr, status, error) {
                    validator.resetForm();
                    userFail(xhr, status, error);
                }
            });
        }
    });

  

    $('#submitEditForm').on('click', function () {

        var form = $('#editUserForm');

        var categories = [];

        $('#tblCategoriesEdit tbody tr').each(function () {
            $this = $(this);
            var categoryId = $this.find("td:eq(0)").text().trim();

            categories.push(categoryId);
        });

        var validator = form.validate({
            rules: {
                'username': {
                    required: true,
                    noSpace: true
                },
                'firstname': {
                    required: true,
                    noSpace: true
                },
                'lastname': {
                    required: true,
                    noSpace: true
                },
                'SelectedOrganizationId': {
                    required: true
                },
                'email': {
                    required: true,
                    noSpace: true,
                    email: true
                },
                'roles[]': {
                    required: true
                }
            },
            messages: {
                'username': {
                    required: $('#hidUsernameRequired').val(),
                    noSpace: $('#hidUsernameRequired').val()
                },
                'firstname': {
                    required: $('#hidFirstnameRequired').val(),
                    noSpace: $('#hidUsernameRequired').val()
                },
                'lastname': {
                    required: $('#hidLastnameRequired').val(),
                    noSpace: $('#hidUsernameRequired').val()
                },
                'SelectedOrganizationId': {
                    required: $('#hidOrganizationRequired').val()
                },
                'email': {
                    required: $('#hidEmailRequired').val(),
                    noSpace: $('#hidUsernameRequired').val(),
                    email: $('#hidInvalidEmail').val()
                },
                'roles[]': {
                    required: $('#hidRoleSelectionRequired').val()
                }
            },
            errorElement: 'span',
            errorPlacement: function (error, element) {
                error.addClass('invalid-feedback');
                element.closest('.form-group').append(error);
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass('is-invalid');
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass('is-invalid');
            }
        });

        if (form.valid()) {
            var disabled = form.find(':input:disabled').removeAttr('disabled');
            var formData = $(form).serialize();
            formData = formData + '&categories=' + categories;

            disabled.attr('disabled', 'disabled');

            // Submit the form using AJAX.
            $.ajax({
                url: ajaxUrl + '/Users/UpdateUser',
                type: "POST",
                dataType: "json",
                data: formData,
                async: false,
                success: function (response) {
                    validator.resetForm();
                    editDone();
                },
                error: function (xhr, status, error) {
                    validator.resetForm();
                    editFail(xhr, status, error);
                }
            });
        }
    });

    $('#submitPwdForm').on('click', function () {
        var form = $('#pwdForm');

        var validator = form.validate({
            rules: {
                'password': {
                    required: true,
                    noSpace: true
                },
                'verify': {
                    required: true,
                    noSpace: true,
                    equalTo: "#password"
                }
            },
            messages: {
                'password': {
                    required: $('#hidPasswordRequired').val(),
                    noSpace: $('#hidPasswordRequired').val()
                },
                'verify': {
                    required: $('#hidVerifyPasswordRequired').val(),
                    noSpace: $('#hidPasswordRequired').val(),
                    equalTo: $('#hidPasswordNotEqual').val()
                }
            },
            errorElement: 'span',
            errorPlacement: function (error, element) {
                error.addClass('invalid-feedback');
                element.closest('.form-group').append(error);
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass('is-invalid');
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass('is-invalid');
            }
        });

        if (form.valid()) {
            var formData = $(form).serialize();

            // Submit the form using AJAX.
            $.ajax({
                url: ajaxUrl + '/Users/ResetPassword',
                type: "POST",
                dataType: "json",
                data: formData,
                async: false,
                success: function (response) {
                    pwdDone();
                },
                error: function (xhr, status, error) {
                    pwdFail(xhr, status, error);
                }
            });
        }
    });

    function updateUserList() {
        $.ajax({
            url: ajaxUrl + '/Users/list',
            type: "GET",
            dataType: "html",
            async: false,
            success: function (response) {
                $('#user-list').empty().html(response);
                initTable();
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });
    }

    function initTable() {

        var tableSettings = {
            dom: 'Bfrtip',
            select: {
                style: 'single',
                info: false
            },
            "aoColumnDefs": [
                {
                    "aTargets": [0, 5, 8, 9, 10, 11, 12, 13],
                    "visible": false
                },
                {
                    "aTargets": [7],
                    "className": 'text-center',
                    "width": "100px"
                }
            ],
            buttons: [
                {
                    text: $('#hidNewButton').val(),
                    name: 'addButton',
                    attr: {
                        'data-bs-toggle': 'modal',
                        'data-bs-target': '#addModal'
                    },
                    action: function (e, dt, button, config) {
                        $('#userError').hide();
                        $('#addUserForm').trigger('reset');
                        $('#tbodyCategoriesEdit').empty();
                    }
                },
                {
                    extend: "selectedSingle",
                    text: $('#hidEditButton').val(),
                    name: 'editButton',
                    attr: {
                        'data-bs-toggle': 'modal',
                        'data-bs-target': '#editModal'
                    },
                    action: function (e, dt, button, config) {
                        var data = dt.row({ selected: true }).data();

                        $('#editError').hide();
                        $('#userError').hide();
                        $('#editUserForm').trigger('reset');

                        $('#editUserForm input[name=username]').val(data.username);
                        $('#editUserForm input[name=email]').val(data.email);
                        $('#editUserForm input[name=firstname]').val(data.firstname);
                        $('#editUserForm input[name=lastname]').val(data.lastname);
                        $('#editUserForm input[name=cellphone]').val(data.cellphone);
                        $('#editUserForm input[name=costHour]').val(data.costHour);
                        $("#editUserForm select[name='SelectedOrganizationId']").val(data.organizationId).trigger('change');
                       

                        $('#editUserForm input[name=locked]').prop('checked', data.locked === 'True' ? true : false);
                        $('#editUserForm button[name=locked]').prop('checked', data.locked === 'True' ? true : false);
                        $('#editUserForm input[name=id]').val(data.id);

                        if (data.roles !== '') {
                            var array = data.roles.split('|');
                            $.each(array, function (index, value) {
                                $('#editUserForm :checkbox[value=' + "'" + value + "'" + ']').prop('checked', true);
                            });
                        }

                        $.ajax({
                            url: ajaxUrl + '/Users/UserCategories/',
                            type: "GET",
                            data: {
                                organizationId: data.organizationId,
                                categories: data.usercategories,
                            },
                            async: false,
                            success: function (response) {
                                $('#tbodyCategoriesEdit').empty();
                                $.each(response, function (index, item) {
                                    $('#tbodyCategoriesEdit').append(`<tr draggable="true" style="border-bottom:1px solid #e2e5e8; height: 55px; cursor:all-scroll;" ondragstart="start()" ondragover="dragover()">
                                    <td class="hidden">${item.id}</td>
                                    <td>${$('#hidLanguage').val().toUpperCase() == 'FR' ? item.description_Fr : item.description_En}</td>
                                    <td style="text-align:center;cursor:default;"><i class="far fa-trash-alt edit fa-lg"></i></td>
                                </tr >
                                `);
                                });
                                $('#tblCategoriesEdit').removeClass('hidden');
                            },
                            error: function (xhr, status, error) {
                                alert('Error');
                            }
                        });

                        //Trigger DDL change event to display current time
                        $('#editModal').on('shown.bs.modal', function () {
                            $("#editUserForm select[id='tz-selector-edit']").trigger('change');
                        });

                    }
                },
                {
                    extend: "selectedSingle",
                    text: $('#hidDeleteButton').val(),
                    name: 'deleteButton',
                    action: function (e, dt, button, config) {
                        var data = dt.row({ selected: true }).data();

                        swal.fire({
                            title: $('#hidDeleteTitle').val(),
                            text: $('#hidDeleteText').val(),
                            icon: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#DD6B55',
                            confirmButtonText: $('#hidDeleteButton').val()
                        }).then(function (result) {

                            if (result.value) {
                                $.ajax({
                                    type: 'DELETE',
                                    url: ajaxUrl + '/Users/DeleteUser',
                                    data: { id: data.id }
                                })
                                    .done(delDone)
                                    .fail(delFail);
                            }
                        });
                    }
                },
                {
                    extend: "selectedSingle",
                    text: $('#hidResetPasswordButton').val(),
                    attr: {
                        'data-bs-toggle': 'modal',
                        'data-bs-target': '#pwdModal'
                    },
                    name: 'resetPasswordButton',
                    action: function (e, dt, button, config) {
                        var data = dt.row({ selected: true }).data();
                        $('#pwdError').hide();
                        $('#pwdForm').trigger('reset');
                        $('#pwdForm input[name=id]').val(data.id);
                    }
                },
                'pageLength'
            ],
            language: {
                buttons: {
                    pageLength: '%d'
                }
            }
        };

        if ($('#hidLanguage').val().toUpperCase() === "FR") {
            tableSettings.language = JSON.parse(datables_french());
        }

        table = $('#usersTable').DataTable(tableSettings);

        table
            .button('addButton:name')
            .nodes()
            .removeClass('btn-secondary')
            .addClass('btn-primary mr-1');

        table
            .button('editButton:name')
            .nodes()
            .removeClass('btn-secondary')
            .addClass('btn-primary mr-1');

        table
            .button('deleteButton:name')
            .nodes()
            .removeClass('btn-secondary')
            .addClass('btn-primary mr-1');

        table
            .button('resetPasswordButton:name')
            .nodes()
            .removeClass('btn-secondary')
            .addClass('btn-primary mr-1');

        // Disable ADD button if max number of user reached
        var remainingUsers = parseInt($('#hidRemainingUsers').val());

        if (remainingUsers === 0) {
            table.button(0).enable(false);
            $('#MaxUserWarning').removeClass('hidden');
        }
        else {
            table.button(0).enable(true);
            $('#MaxUserWarning').addClass('hidden');
        }
    }

    $('#addModal').on('hidden.bs.modal', function () {
        $("#addUserForm").validate().resetForm();

        // get errors that were created using jQuery.validate.unobtrusive
        $("#addUserForm").find(".is-invalid").removeClass('is-invalid');
    });

    $('#editModal').on('hidden.bs.modal', function () {
        $("#editUserForm").validate().resetForm();

        // get errors that were created using jQuery.validate.unobtrusive
        $("#editUserForm").find(".is-invalid").removeClass('is-invalid');
    });

    $('#pwdModal').on('hidden.bs.modal', function () {
        $("#pwdForm").validate().resetForm();

        // get errors that were created using jQuery.validate.unobtrusive
        $("#pwdForm").find(".is-invalid").removeClass('is-invalid');
    });

    function userDone(data, status, xhr) {
        $('#addModal').modal('hide');
        Swal.fire({
            icon: 'success',
            title: $('#hidCreateUserSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 1000,
            timerProgressBar: true
        });

        //Update remainingUsers
        var total = parseInt($('#hidRemainingUsers').val());
        $('#hidRemainingUsers').val(total-1);

        updateUserList();
    }

    function resendEmailDone(data, status, xhr) {
        $('#userModal').modal('hide');
        toastr.success($('#hidSendEmailSuccess').val());
    }

    function userFail(xhr, status, error) {
        $('#userError').html(xhr.responseText || error).fadeIn();
    }

    function editDone(data, status, xhr) {
        $('#editModal').modal('hide');
        Swal.fire({
            icon: 'success',
            title: $('#hidUpdateUserSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 1000,
            timerProgressBar: true
        });
        updateUserList();
    }

    function editFail(xhr, status, error) {
        $('#editError').html(xhr.responseText || error).fadeIn();
    }

    function delDone(data, status, xhr) {
        Swal.fire({
            icon: 'success',
            title: $('#hidDeleteUserSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 1000,
            timerProgressBar: true
        });
        updateUserList();
    }

    function delFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }

    function pwdDone(data, status, xhr) {
        Swal.fire({
            icon: 'success',
            title: $('#hidChangePasswordSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 1000,
            timerProgressBar: true
        });
        $('#pwdModal').modal('hide');
    }

    function pwdFail(xhr, status, error) {
        $('#pwdError').html(xhr.responseText || error).fadeIn();
    }
});
