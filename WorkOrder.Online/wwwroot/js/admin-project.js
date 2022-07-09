$(document).ready(function () {

    var ajaxUrl = $('#HidRootUrl').val();
    var table;

    $('select[name="SelectedOrganizationId"]').on('change', function () {
        var selectedOrganization = $(this).val();
        $('#hidSelectedOrganizationId').val(selectedOrganization);
        $('input[name="selectedOrganizationId"]').val(selectedOrganization);
        UpdateProjectList(selectedOrganization);
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
                "aTargets": [0,4],
                "visible": false
            },
            {
                "aTargets": [2],
                "width": "60%"
            },
            {
                "aTargets": [3],
                "className": "text-center",
                "width": "18%"
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
                    $('#projectError').hide();
                    $('#addForm').trigger('reset');

                    $.ajax({
                        url: ajaxUrl + '/Projects/NextSequence/' + $('#hidSelectedOrganizationId').val(),
                        type: "GET",
                        async: false,
                        success: function (response) {
                            $('#addForm input[name=projectNo]').val(response);
                        },
                        error: function (xhr, status, error) {
                            alert('Error');
                        }
                    });
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
                    $('#editForm input[name=projectNo]').val(data.projectNo);
                    $('#editForm input[name=description]').val(data.description);
                    $('#editForm select[name=SelectedCustomerId]').val(data.customerId);
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
                                url: ajaxUrl + '/Projects/Delete',
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
                'projectNo': {
                    required: true,
                    noSpace: true
                },
                'SelectedCustomerId': {
                    required: true,
                    noSpace: true
                }
            },
            messages: {
                'projectNo': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'SelectedCustomerId': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
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

            var data = {
                projectNo: $('#addForm input[name=projectNo]').val(),
                Description: $('#addForm input[name=description]').val(),
                OrganizationId: $('#hidSelectedOrganizationId').val(),
                CustomerId: $('#addForm select[name=SelectedCustomerId]').val()
            }

            $.ajax({
                url: ajaxUrl + '/Projects/Create',
                type: "POST",
                data: data,
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
                'projectNo': {
                    required: true,
                    noSpace: true
                }
            },
            messages: {
                'projectNo': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                }
            }, rules: {
                'projectNo': {
                    required: true,
                    noSpace: true
                },
                'SelectedCustomerId': {
                    required: true,
                    noSpace: true
                }
            },
            messages: {
                'projectNo': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'SelectedCustomerId': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
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

            var data = {
                Id: $('#editForm input[name=id]').val(),
                ProjectNo: $('#editForm input[name=projectNo]').val(),
                Description: $('#editForm input[name=description]').val(),
                OrganizationId: $('#hidSelectedOrganizationId').val(),
                CustomerId: $('#editForm select[name=SelectedCustomerId]').val()
            }

            $.ajax({
                url: ajaxUrl + '/Projects/Update',
                type: "POST",
                data: data,
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

        table = $('#projectsTable').DataTable(tableSettings);

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

        //table.on('select deselect', function (e, dt, type, indexes) {
        //    var rowData = table.rows(indexes).data().toArray();

        //    //Cannot delete SuperAdmin/Administrator/Mobile Role
        //    if (rowData[0]['name'] === 'SuperAdmin' || rowData[0]['name'] === 'Administrator') {
        //        table.button(1).enable(false);
        //        table.button(2).enable(false);
        //    }
        //    else if (rowData[0]['usercount'] > 0) //Cannot delete if user in group
        //        table.button(2).enable(false);
        //    else {
        //        table.button(1).enable(true);
        //        table.button(2).enable(true);
        //    }
        //});
    }

    function UpdateProjectList(selectedOrganization) {
        $.ajax({
            url: ajaxUrl + '/Projects/List',
            type: "GET",
            data: {
                organizationId: $('#hidSelectedOrganizationId').val()
            },
            dataType: "html",
            async: false,
            success: function (response) {
                $('#project-list').empty().append(response);
                initTable();
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });
    }

    function UpdateCustomerList(selectedOrganization) {
        $.ajax({
            url: ajaxUrl + '/Projects/Customers',
            type: "GET",
            data: {
                organizationId: $('#hidSelectedOrganizationId').val()
            },
           // dataType: "html",
            async: false,
            success: function (response) {
                $('select[name="SelectedCustomerId"]').empty().append('<option value="">' + $('#hidSelectOption').val() + '</option>');
                $.each(response, function (index, item) {
                    $('select[name="SelectedCustomerId"]').append(
                        $('<option value="' + item.value + '">' + item.text + '</option>')
                    );
                });
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
        UpdateProjectList();
    }

    function addFail(xhr, status, error) {
        $('#projectError').html(xhr.responseText || error).fadeIn();
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
        UpdateProjectList();
    }

    function editFail(xhr, status, error) {
        $('#projectError').html(xhr.responseText || error).fadeIn();
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
        UpdateProjectList();
    }

    function delFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }

});
