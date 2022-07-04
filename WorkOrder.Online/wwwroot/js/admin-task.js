$(document).ready(function () {

    var ajaxUrl = $('#HidRootUrl').val();
    var table;

    //$('select[name="SelectedOrganizationId"]').select2();

    $('select[name="SelectedOrganizationId"]').on('change', function () {
        var selectedOrganization = $(this).val();
        $('#hidSelectedOrganizationId').val(selectedOrganization);
        $('input[name="selectedOrganizationId"]').val(selectedOrganization);
        UpdateTaskList(selectedOrganization);
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
                "aTargets": [0, 7],
                "visible": false
            },
            {
                "aTargets": [1],
                "width": "20%"
            },
            {
                "aTargets": [2],
                "width": "50%"
            },
            {
                "aTargets": [4,5],
                "width": "10%",
            },
            {
                "aTargets": [6],
                "width": "10%",
                "className": "dt-center"
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
                    $('#taskError').hide();
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
                    $('#editForm input[name=code]').val(data.code);
                    $('#editForm input[name=description_fr]').val(data.description_fr);
                    $('#editForm input[name=description_en]').val(data.description_en);
                    $('#editForm input[name=cost]').val(data.cost);
                    $('#editForm input[name=retail]').val(data.retail);
                    $('#editForm input[name=isFlatRate]').prop('checked', data.isflatrate == 'True' ? true : false);
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
                                url: ajaxUrl + '/Tasks/Delete',
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
                'code': {
                    required: true,
                    noSpace: true
                },
                'description_fr': {
                    required: true,
                    noSpace: true
                },
                 'description_en': {
                    required: true,
                    noSpace: true
                },
                'cost': {
                    required: true,
                    noSpace: true
                },
                'retail': {
                    required: true,
                    noSpace: true
                }
            },
            messages: {
                'code': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'description_fr': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'description_en': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'cost': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'retail': {
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
                Code: $('#addForm input[name=code]').val(),
                Description_Fr: $('#addForm input[name=description_fr]').val(),
                Description_En: $('#addForm input[name=description_en]').val(),
                Cost: $('#hidLanguage').val().toUpperCase() == 'FR' ? $('#addForm input[name=cost]').val().replace('.', ',') : $('#addForm input[name=cost]').val().replace(',', '.'),
                Retail: $('#hidLanguage').val().toUpperCase() == 'FR' ? $('#addForm input[name=retail]').val().replace('.', ',') : $('#addForm input[name=retail]').val().replace(',', '.'),
                IsFlatRate: $('#addForm input[name=isFlatRate]').prop("checked"),
                OrganizationId: $('#hidSelectedOrganizationId').val()
            }

            $.ajax({
                url: ajaxUrl + '/Tasks/Create',
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
                'code': {
                    required: true,
                    noSpace: true
                },
                'description_fr': {
                    required: true,
                    noSpace: true
                },
                'description_en': {
                    required: true,
                    noSpace: true
                },
                'cost': {
                    required: true,
                    noSpace: true
                },
                'retail': {
                    required: true,
                    noSpace: true
                }
            },
            messages: {
                'code': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'description_fr': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'description_en': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'cost': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'retail': {
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
                Code: $('#editForm input[name=code]').val(),
                Description_Fr: $('#editForm input[name=description_fr]').val(),
                Description_En: $('#editForm input[name=description_en]').val(),
                Cost: $('#hidLanguage').val().toUpperCase() == 'FR' ? $('#editForm input[name=cost]').val().replace('.', ',') : $('#editForm input[name=cost]').val().replace(',', '.'),
                Retail: $('#hidLanguage').val().toUpperCase() == 'FR' ? $('#editForm input[name=retail]').val().replace('.', ',') : $('#editForm input[name=retail]').val().replace(',', '.'),
                IsFlatRate: $('#editForm input[name=isFlatRate]').prop("checked"),
                OrganizationId: $('#hidSelectedOrganizationId').val()
            }

            $.ajax({
                url: ajaxUrl + '/Tasks/Update',
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

        table = $('#tasksTable').DataTable(tableSettings);

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

        table.on('select deselect', function (e, dt, type, indexes) {
            var rowData = table.rows(indexes).data().toArray();

            //Cannot delete SuperAdmin/Administrator/Mobile Role
            if (rowData[0]['name'] === 'SuperAdmin' || rowData[0]['name'] === 'Administrator') {
                table.button(1).enable(false);
                table.button(2).enable(false);
            }
            else if (rowData[0]['usercount'] > 0) //Cannot delete if user in group
                table.button(2).enable(false);
            else {
                table.button(1).enable(true);
                table.button(2).enable(true);
            }
        });
    }

    function UpdateTaskList() {
        $.ajax({
            url: ajaxUrl + '/Tasks/list',
            type: "GET",
            data: {
                organizationId: $('#hidSelectedOrganizationId').val()
            },
            dataType: "html",
            async: false,
            success: function (response) {
                $('#task-list').empty().append(response);
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
        UpdateTaskList();
    }

    function addFail(xhr, status, error) {
        $('#taskError').html(xhr.responseText || error).fadeIn();
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
        UpdateTaskList();
    }

    function editFail(xhr, status, error) {
        $('#taskError').html(xhr.responseText || error).fadeIn();
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
        UpdateTaskList();
    }

    function delFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }

});
