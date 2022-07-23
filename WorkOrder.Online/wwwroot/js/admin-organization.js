$(document).ready(function () {

    var ajaxUrl = $('#HidRootUrl').val();
    var table;
    var buttonArray = [];

    BuildButtonArray();

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
        "aoColumnDefs": [
            {
                "aTargets": [0],
                "visible": false
            },
            {
                "aTargets": [3, 4, 5],
                "className": 'text-center',
            },
            {
                "aTargets": [6],
                "className": 'text-center',
                "width": "100px",
                "orderable": false
            }
        ],
        buttons: buttonArray,
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
                'name': {
                    required: true,
                    noSpace: true
                },
                'address': {
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
                },
                'SelectedLanguageCode': {
                    required: true
                },
                'nbrUsers': {
                    required: true
                }
            },
            messages: {
                'name': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'address': {
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
                },
                'SelectedLanguageCode': {
                    required: $('#hidRequired').val()
                },
                'nbrUsers': {
                    required: $('#hidRequired').val()
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

            var res = formData.split("&");
            for (var i = 0; i < res.length; i++) {
                if (res[i].indexOf("IsWoPriceHidden") == 0) {
                    res[i] = 'IsWoPriceHidden=' + $('#addForm input[name=IsWoPriceHidden]').prop('checked');
                }
            }

            formData = res.join("&");

            $.ajax({
                url: ajaxUrl + '/Organizations/Create',
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
                'address': {
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
                },
                'SelectedLanguageCode': {
                    required: true
                },
                'nbrUsers': {
                    required: true
                }
            },
            messages: {
                'name': {
                    required: $('#hidRequired').val(),
                    noSpace: $('#hidRequired').val()
                },
                'address': {
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
                },
                'SelectedLanguageCode': {
                    required: $('#hidRequired').val()
                },
                'nbrUsers': {
                    required: $('#hidRequired').val()
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

            var res = formData.split("&");
            for (var i = 0; i < res.length; i++) {
                if (res[i].indexOf("isActive") == 0) {
                    res[i] = 'isActive=' + $('#editForm input[name=isActive]').prop('checked');
                }
                if (res[i].indexOf("IsWoPriceHidden") == 0) {
                    res[i] = 'IsWoPriceHidden=' + $('#editForm input[name=IsWoPriceHidden]').prop('checked');
                }
            }

            formData = res.join("&");

            $.ajax({
                url: ajaxUrl + '/Organizations/Update',
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

        table = $('#organizationsTable').DataTable(tableSettings);

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

        table.on('select', function (e, dt, type, indexes) {
            var rowData = table.rows(indexes).data().toArray();

            //Cannot delete Base Organization
            if (rowData[0]['id'] === '1') {
                table.button(1).enable(false);
                table.button(2).enable(false);
            }
            else {
                table.button(1).enable(true);
                table.button(2).enable(true);
            }
        });
    }

    function UpdateOrganizationList() {
        $.ajax({
            url: ajaxUrl + '/Organizations/list',
            type: "GET",
            dataType: "html",
            async: false,
            success: function (response) {
                $('#org-list').empty().append(response);
                initTable();
            },
            error: function (xhr, status, error) {
                alert('Error');
            }
        });
    }

    function BuildButtonArray() {
        if ($('#hidIsSuperAdmin').val() == 'True') {
            buttonArray.push({
                text: $('#hidNewButton').val(),
                name: 'addButton',
                className: 'btn-primary',
                attr: {
                    'data-bs-toggle': 'modal',
                    'data-bs-target': '#addModal'
                },
                action: function (e, dt, button, config) {
                    $('#addError').hide();
                    $('#addForm').trigger('reset');
                }
            });
        }

        buttonArray.push({
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

                $.ajax({
                    url: ajaxUrl + '/Organizations/' + data.id,
                    type: "GET",
                    async: false,
                    success: function (response) {
                        $('#editForm input[name=name]').val(response.name);
                        $('#editForm input[name=id]').val(response.id);
                        $('#editForm input[name=address]').val(response.address);
                        $('#editForm input[name=city]').val(response.city);
                        $('#editForm input[name=province]').val(response.province);
                        $('#editForm input[name=postalCode]').val(response.postalCode);
                        $('#editForm input[name=phone]').val(response.phone);
                        $('#editForm input[name=email]').val(response.email);
                        $('#editForm select[name=language]').val(response.language);
                        $('#editForm input[name=nbrUsers]').val(response.nbrUsers);
                        $('#editForm textarea[name=Notes]').val(response.notes);
                        $('#editForm input[name=isActive]').prop('checked', response.isActive);
                        $('#editForm input[name=ProjectStartSequence]').val(response.projectStartSequence);
                        $('#editForm input[name=IsWoPriceHidden]').prop('checked', response.isWoPriceHidden);
                    },
                    error: function (xhr, status, error) {
                        alert('Error');
                    }
                });
            }
        });

        if ($('#hidIsSuperAdmin').val() == 'True') {
            buttonArray.push({
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
                                url: ajaxUrl + '/Organizations/Delete',
                                data: { id: data.id }
                            })
                                .done(delDone)
                                .fail(delFail);
                        }
                    });
                }
            });
        }
        buttonArray.push('pageLength');
    }

    function addDone(data, status, xhr) {
        var addModal = document.getElementById('addModal')
        var modal = bootstrap.Modal.getInstance(addModal)
        modal.hide();

        $('#addModal').modal('hide');
        Swal.fire({
            icon: 'success',
            title: $('#hidCreateSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 1000,
            timerProgressBar: true
        });
        UpdateOrganizationList();
    }

    function addFail(xhr, status, error) {
        $('#addError').html(xhr.responseText || error).fadeIn();
    }

    function editDone(data, status, xhr) {
        var addModal = document.getElementById('editModal')
        var modal = bootstrap.Modal.getInstance(addModal)
        modal.hide();

        Swal.fire({
            icon: 'success',
            title: $('#hidUpdateSuccess').val(),
            showCancelButton: false,
            showConfirmButton: false,
            timer: 1000,
            timerProgressBar: true
        });
        UpdateOrganizationList();
    }

    function editFail(xhr, status, error) {
        $('#editError').html(xhr.responseText || error).fadeIn();
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
        UpdateOrganizationList();
    }

    function delFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }

});
