$(document).ready(function () {

    var ajaxUrl = $('#HidRootUrl').val();
    var table;

    //$('select[name="SelectedOrganizationId"]').select2();

    $('select[name="SelectedOrganizationId"]').on('change', function () {
        var selectedOrganization = $(this).val();
        $('#hidSelectedOrganizationId').val(selectedOrganization);
        $('input[name="selectedOrganizationId"]').val(selectedOrganization);
        UpdateCustomerList(selectedOrganization);
    });

    var tableSettings = {
        dom: 'Bfrtip',
        retrieve: true,
        select: {
            style: 'single',
            info: false
        },
        "aoColumnDefs": [
            {
                "aTargets": [0,6,7,8,9],
                "visible": false
            }
        ],
        buttons: [
            {
                text: $('#hidNewButton').val(),
                name: 'addButton',
                className: 'btn-primary',
                attr: {
                    'data-bs-toggle': 'modal',
                    'data-bs-target': '#addModal'
                },
                action: function (e, dt, button, config) {
                    $('#customerError').hide();
                    $('#addForm').trigger('reset');
                }
            },
            {
                extend: "selectedSingle",
                text: $('#hidEditButton').val(),
                name: 'editButton',
                className: 'btn-primary',
                attr: {
                    'data-bs-toggle': 'modal',
                    'data-bs-target': '#editModal'
                },
                action: function (e, dt, button, config) {
                    var data = dt.row({ selected: true }).data();
                    $('#editError').hide();
                    $('#tabs a:first').tab('show');
                    $('#editForm').trigger('reset');

                    $('#editForm input[name=id]').val(data.id);
                    $('#editForm input[name=name]').val(data.name);
                    $('#editForm input[name=responsable]').val(data.responsable);
                    $('#editForm input[name=address]').val(data.address);
                    $('#editForm input[name=city]').val(data.city);
                    $('#editForm input[name=state]').val(data.state);
                    $('#editForm input[name=postalcode]').val(data.postalcode);
                    $('#editForm input[name=phone]').val(data.phone);
                    $('#editForm input[name=cellphone]').val(data.cellphone);
                    $('#editForm input[name=email]').val(data.email);
                }
            },
            {
                extend: "selectedSingle",
                text: $('#hidDeleteButton').val(),
                name: 'deleteButton',
                className: 'btn-primary',
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
                                url: ajaxUrl + '/Customers/Delete',
                                data: { id: data.id }
                            })
                                .done(delDone)
                                .fail(delFail);
                        }
                    });
                }
            }
        ],

    };

    initTable();

    
    $('#submitAddForm').on('click', function () {
        var form = $('#addForm');
                
        form.validate({
            rules: {
                'name': {
                    required: true,
                    noSpace: true
                },
                'email': {
                    email: true
                }
            },
            messages: {
                'name': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'email': {
                    email: $('#hidInvalidEmail').val()
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
            formData = formData + '&OrganizationId=' + $('#hidSelectedOrganizationId').val();

            $.ajax({
                url: ajaxUrl + '/Customers/Create',
                type: "POST",
                dataType: "json",
                data: formData,
                async: false,
                success: function (response) {
                    addDone();
                },
                error: function (xhr, status, error) {
                    addFail(xhr, status, error);
                }
            });
        }
    });

    $('#submitEditForm').on('click', function () {
        var form = $('#editForm');

        form.validate({
            rules: {
                'name': {
                    required: true,
                    noSpace: true
                },
                'email': {
                    email: true
                }
            },
            messages: {
                'name': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'email': {
                    email: $('#hidInvalidEmail').val()
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

            $.ajax({
                url: ajaxUrl + '/Customers/Update',
                type: "POST",
                dataType: "json",
                data: formData,
                async: false,
                success: function (response) {
                    editDone();
                },
                error: function (xhr, status, error) {
                    editFail(xhr, status, error);
                }
            });
        }
    });

    function initTable() {

        if ($('#hidLanguage').val().toUpperCase() === "FR") {
            tableSettings.language = JSON.parse(datables_french());
        }

        table = $('#customersTable').DataTable(tableSettings);

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
    }

    function UpdateCustomerList() {
        $.ajax({
            url: ajaxUrl + '/Customers/list',
            type: "GET",
            data: {
                organizationId: $('#hidSelectedOrganizationId').val()
            },
            dataType: "html",
            async: false,
            success: function (response) {
                $('#customer-list').empty().append(response);
                initTable();
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });
    }

    function addDone(data, status, xhr) {
        $('#addModal').modal('hide');
        Swal.fire({
            icon: 'success',
            title: $('#hidCreateSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 1000,
            timerProgressBar: true
        });
        UpdateCustomerList();
    }

    function addFail(xhr, status, error) {
        $('#customerError').html(xhr.responseText || error).fadeIn();
    }

    function editDone(data, status, xhr) {
        $('#editModal').modal('hide');
        Swal.fire({
            icon: 'success',
            title: $('#hidUpdateSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 1000,
            timerProgressBar: true
        });
        UpdateCustomerList();
    }

    function editFail(xhr, status, error) {
        $('#customerError').html(xhr.responseText || error).fadeIn();
    }

    function delDone(data, status, xhr) {
        Swal.fire({
            icon: 'success',
            title: $('#hidDeleteSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 1000,
            timerProgressBar: true
        });
        UpdateCustomerList();
    }

    function delFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }

});
