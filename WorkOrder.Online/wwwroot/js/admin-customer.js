$(document).ready(function () {

    var ajaxUrl = $('#HidRootUrl').val();
    var table;
    var tableResponsible;
    //$('select[name="SelectedOrganizationId"]').select2();

    $('select[name="SelectedOrganizationId"]').on('change', function () {
        var selectedOrganization = $(this).val();
        $('#hidSelectedOrganizationId').val(selectedOrganization);
        $('input[name="selectedOrganizationId"]').val(selectedOrganization);
        UpdateCustomerList(selectedOrganization);
    });

    var state_key = "_" + $('#hidUserId').val();

    var tableSettings = {
        dom: 'Bfrtip',
        retrieve: true,
        stateSave: true,
        stateDuration: 0,
        stateSaveCallback: function (settings, data) {
            localStorage.setItem('DataTables_' + settings.sInstance + state_key, JSON.stringify(data))
        },
        stateLoadCallback: function (settings) {
            return JSON.parse(localStorage.getItem('DataTables_' + settings.sInstance + state_key))
        },
        select: {
            style: 'single',
            info: false
        },
        select: {
            style: 'single',
            info: false
        },
        "aoColumnDefs": [
            {
                "aTargets": [0, 6, 7, 8, 9],
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
                    $('#editForm input[name=responsible]').val(data.responsible);
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
            },
            'pageLength'
        ],
        language: {
            buttons: {
                pageLength: { "-1": "Show All", "_": "%d rows" }
            }
        }
    };

    var responsibleTableSettings = {
        dom: 'Bfrtip',
        retrieve: true,
        autoWidth: false,
        select: {
            style: 'single',
            info: false
        },
        "aoColumnDefs": [
            {
                "aTargets": [0, 4],
                "visible": false
            }
        ],
        buttons: [
            {
                text: $('#hidNewButton').val(),
                name: 'addResponsibleButton',
                className: 'btn-primary',
                action: function (e, dt, button, config) {
                    $('#responsibleForm').trigger('reset');
                    $('#responsibleForm input[name=id]').val(0);
                    $('#responsibleForm').slideDown();
                }
            },
            {
                extend: "selectedSingle",
                text: $('#hidEditButton').val(),
                name: 'editResponsibleButton',
                className: 'btn-primary',
                action: function (e, dt, button, config) {
                    var data = dt.row({ selected: true }).data();
                    $('#editError').hide();
                    $('#tabs a:first').tab('show');
                    $('#responsibleForm').trigger('reset');

                    $('#responsibleForm input[name=id]').val(data.id);
                    $('#responsibleForm input[name=customerId]').val(data.customerId);
                    $('#responsibleForm input[name=name]').val(data.name);
                    $('#responsibleForm input[name=cellphone]').val(data.cellphone);
                    $('#responsibleForm input[name=email]').val(data.email);

                    $('#responsibleForm').slideDown();
                }
            },
            {
                extend: "selectedSingle",
                text: $('#hidDeleteButton').val(),
                name: 'deleteResponsibleButton',
                className: 'btn-primary',
                action: function (e, dt, button, config) {
                    var data = dt.row({ selected: true }).data();

                    swal.fire({
                        title: $('#hidDeleteResponsibleTitle').val(),
                        text: $('#hidDeleteResponsibleText').val(),
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#DD6B55',
                        confirmButtonText: $('#hidDeleteButton').val()
                    }).then(function (result) {
                        if (result.value) {

                            $.ajax({
                                type: 'DELETE',
                                url: ajaxUrl + '/Customers/Responsible/Delete',
                                data: { id: data.id },
                                success: function (response) {
                                    UpdateResponsibleList();
                                },
                                error: function (xhr, status, error) {
                                    alert(xhr.responseText || error);
                                }
                            });
                        }
                    });
                }
            }
        ]
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
                'responsible': {
                    required: true,
                    noSpace: true
                },
                'address': {
                    required: true,
                    noSpace: true
                },
                'city': {
                    required: true,
                    noSpace: true
                },
                'state': {
                    required: true,
                    noSpace: true
                },
                'postalCode': {
                    required: true,
                    noSpace: true
                },
                'phone': {
                    required: true,
                    noSpace: true
                },
                'email': {
                    required: true,
                    noSpace: true,
                    email: true
                }
            },
            messages: {
                'name': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'responsible': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'address': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'city': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'state': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'postalCode': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'phone': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'email': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val(),
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
                'responsible': {
                    required: true,
                    noSpace: true
                },
                'address': {
                    required: true,
                    noSpace: true
                },
                'city': {
                    required: true,
                    noSpace: true
                },
                'state': {
                    required: true,
                    noSpace: true
                },
                'postalCode': {
                    required: true,
                    noSpace: true
                },
                'phone': {
                    required: true,
                    noSpace: true
                },
                'email': {
                    required: true,
                    noSpace: true,
                    email: true
                }
            },
            messages: {
                'name': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'responsible': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'address': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'city': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'state': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'postalCode': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'phone': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'email': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val(),
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

    $('#submitResponsibleForm').on('click', function () {
        var form = $('#responsibleForm');

        form.validate({
            rules: {
                'name': {
                    required: true,
                    noSpace: true
                },
                'cellphone': {
                    required: true,
                    noSpace: true
                },
                'email': {
                    required: true,
                    noSpace: true,
                    email: true
                }
            },
            messages: {
                'name': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'cellphone': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'email': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val(),
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
                url: ajaxUrl + '/Customers/Responsible/Save',
                type: "POST",
                dataType: "json",
                data: formData,
                async: false,
                success: function (response) {
                    UpdateResponsibleList();
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText || error);
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
            .addClass('btn-primary');

        table
            .button('editButton:name')
            .nodes()
            .removeClass('btn-secondary')
            .addClass('btn-primary');

        table
            .button('deleteButton:name')
            .nodes()
            .removeClass('btn-secondary')
            .addClass('btn-primary');

        new $.fn.dataTable.Buttons(table, {
            name: 'customButton',
            buttons: [{
                extend: "selectedSingle",
                text: $('#hidAuthorized').val(),
                name: 'responsibleButton',
                className: 'btn-primary',
                attr: {
                    'data-bs-toggle': 'modal',
                    'data-bs-target': '#responsibleModal'
                },
                action: function (e, dt, button, config) {
                    var data = dt.row({ selected: true }).data();

                    $.ajax({
                        type: 'GET',
                        url: ajaxUrl + '/Customers/Responsibles/list',
                        data: { customerId: data.id },
                        success: function (response) {
                            $('#responsible-list').empty().append(response);
                            $('#responsibleForm input[name=customerId]').val(data.id);
                            initResponsibleTable();
                        },
                        error: function (xhr, status, error) {
                            alert('Error');
                        }
                    })


                }
            },
            {
                text: '<i class="fas fa-user-plus"></i> ' + $('#hidImportCustomers').val(),
                name: 'importButton',
                className: 'btn-primary',
                action: function (e, dt, button, config) {
                    var data = dt.row({ selected: true }).data();

                    Swal.fire({
                        title: 'Select image',
                        html: 'You can upload multiple customers here',
                        input: 'file',
                        inputAttributes: {
                            'id': 'customerFile',
                            'accept': '.csv',
                            'aria-label': 'Upload your CSV customers file'
                        }
                    }).then((file) => {
                        if (!file.isDismissed) {
                            handleFileSelect(file);
                        }
                    });

                }
            }]
        });

        table
            .buttons('customButton', null)
            .containers()
            .insertBefore('.dataTables_filter');

        table
            .button('responsibleButton:name')
            .nodes()
            .removeClass('btn-secondary')
            .addClass('btn-primary')
            .addClass('margin-button');

        table
            .button('importButton:name')
            .nodes()
            .removeClass('btn-secondary')
            .addClass('btn-primary');
    }

    function handleFileSelect(file) {

        // Create FormData object  
        var formData = new FormData();

        formData.append("importFile", file.value);
        formData.append("organizationId", $('#hidSelectedOrganizationId').val());

        $.ajax({
            url: ajaxUrl + '/Customers/Import',
            type: "POST",
            dataType: "JSON",
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                Swal.fire({
                    icon: 'success',
                    title: $('#hidImportedCustomerSuccess').val(),
                    showCancelButton: false,
                    showConfirmButton: false,
                    timer: 1000,
                    timerProgressBar: true
                });
                UpdateCustomerList();
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }

    function initResponsibleTable() {

        if ($('#hidLanguage').val().toUpperCase() === "FR") {
            responsibleTableSettings.language = JSON.parse(datables_french());
        }

        tableResponsible = $('#responsiblesTable').DataTable(responsibleTableSettings);

        tableResponsible
            .button('addResponsibleButton:name')
            .nodes()
            .removeClass('btn-secondary')
            .addClass('btn-primary');

        tableResponsible
            .button('editResponsibleButton:name')
            .nodes()
            .removeClass('btn-secondary')
            .addClass('btn-primary');

        tableResponsible
            .button('deleteResponsibleButton:name')
            .nodes()
            .removeClass('btn-secondary')
            .addClass('btn-primary');

        tableResponsible.on('deselect', function (e, dt, type, indexes) {
            $('#responsibleForm').slideUp();
        });
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

    function UpdateResponsibleList() {
        $.ajax({
            url: ajaxUrl + '/Customers/Responsibles/list',
            type: "GET",
            data: {
                customerId: $('#responsibleForm input[name=customerId]').val()
            },
            dataType: "html",
            async: false,
            success: function (response) {
                $('#responsible-list').empty().append(response);
                $('#responsibleForm').slideUp();
                initResponsibleTable();
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
