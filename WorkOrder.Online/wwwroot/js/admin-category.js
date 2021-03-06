$(document).ready(function () {

    var ajaxUrl = $('#HidRootUrl').val();
    var table;

    //$('select[name="SelectedOrganizationId"]').select2();

    $('select[name="SelectedOrganizationId"]').on('change', function () {
        var selectedOrganization = $(this).val();
        $('#hidSelectedOrganizationId').val(selectedOrganization);
        $('input[name="selectedOrganizationId"]').val(selectedOrganization);
        UpdateCategoryList(selectedOrganization);
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
                "aTargets": [0],
                "visible": false
            },
            {
                "aTargets": [1],
                "width": "60%"
            },
            {
                "aTargets": [2],
                "width": "20%"
            },
            {
                "aTargets": [3],
                "width": "20%",
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
                    $('#categoryError').hide();
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
                    $('#editForm input[name=description_fr]').val(data.description_fr);
                    $('#editForm input[name=description_en]').val(data.description_en);
                    $('#editForm input[name=cost]').val(data.cost);
                    $('#editForm input[name=retail]').val(data.retail);
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
                                url: ajaxUrl + '/Categories/Delete',
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

    initTable();

    
    $('#submitAddForm').on('click', function () {
        var form = $('#addForm');
                
        form.validate({
            rules: {
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
                Description_Fr: $('#addForm input[name=description_fr]').val(),
                Description_En: $('#addForm input[name=description_en]').val(),
                Cost: $('#hidLanguage').val().toUpperCase() == 'FR' ? $('#addForm input[name=cost]').val().replace('.', ',') : $('#addForm input[name=cost]').val().replace(',', '.'),
                Retail: $('#hidLanguage').val().toUpperCase() == 'FR' ? $('#addForm input[name=retail]').val().replace('.', ',') : $('#addForm input[name=retail]').val().replace(',', '.'),
                OrganizationId: $('#hidSelectedOrganizationId').val()
            }

            $.ajax({
                url: ajaxUrl + '/Categories/Create',
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
                Description_Fr: $('#editForm input[name=description_fr]').val(),
                Description_En: $('#editForm input[name=description_en]').val(),
                Cost: $('#hidLanguage').val().toUpperCase() == 'FR' ? $('#editForm input[name=cost]').val().replace('.', ',') : $('#editForm input[name=cost]').val().replace(',', '.'),
                Retail: $('#hidLanguage').val().toUpperCase() == 'FR' ? $('#editForm input[name=retail]').val().replace('.', ',') : $('#editForm input[name=retail]').val().replace(',', '.'),
                OrganizationId: $('#hidSelectedOrganizationId').val()
            }

            $.ajax({
                url: ajaxUrl + '/Categories/Update',
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

        table = $('#categoryTable').DataTable(tableSettings);

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

    function UpdateCategoryList() {
        $.ajax({
            url: ajaxUrl + '/Categories/list',
            type: "GET",
            data: {
                organizationId: $('#hidSelectedOrganizationId').val()
            },
            dataType: "html",
            async: false,
            success: function (response) {
                $('#category-list').empty().append(response);
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
        UpdateCategoryList();
    }

    function addFail(xhr, status, error) {
        $('#categoryError').html(xhr.responseText || error).fadeIn();
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
        UpdateCategoryList();
    }

    function editFail(xhr, status, error) {
        $('#categoryError').html(xhr.responseText || error).fadeIn();
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
        UpdateCategoryList();
    }

    function delFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }

});
