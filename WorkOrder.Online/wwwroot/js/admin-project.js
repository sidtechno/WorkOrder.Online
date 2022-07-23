$(document).ready(function () {

    var ajaxUrl = $('#HidRootUrl').val();
    var table;

    $('select[name="SelectedOrganizationId"]').on('change', function () {
        var selectedOrganization = $(this).val();
        $('#hidSelectedOrganizationId').val(selectedOrganization);
        $('input[name="selectedOrganizationId"]').val(selectedOrganization);
        UpdateProjectList(!$('#displayDeleted').is(':checked'));
        UpdateCustomerList(selectedOrganization);
        UpdateCategoryList(selectedOrganization);
    });

    $('select[name="SelectedCategoryId"]').on('change', function () {
        var selectedCategoryId = $(this).val();
        if (selectedCategoryId === '') return;
        var selectedCategory = $("option:selected", this);

        $('#tbodyCategories').append(`<tr draggable="true" style="border-bottom:1px solid #e2e5e8; height: 55px; cursor:all-scroll;" ondragstart="start()" ondragover="dragover()">
                                    <td class="hidden">${selectedCategoryId}</td>
                                    <td class="index"></td>
                                    <td>${selectedCategory[0].innerText}</td>
                                    <td><input id="txtHours" type="text" class="form-control hours" /></td>
                                    <td style="text-align:center;cursor:default;"><i class="far fa-trash-alt fa-lg"></i></td>
                                </tr >
            `);

        $('#tbodyCategoriesEdit').append(`<tr draggable="true" style="border-bottom:1px solid #e2e5e8; height: 55px; cursor:all-scroll;" ondragstart="start()" ondragover="dragover()">
                                    <td class="hidden">${selectedCategoryId}</td>
                                    <td class="indexEdit"></td>
                                    <td>${selectedCategory[0].innerText}</td>
                                    <td><input id="txtHours" type="text" class="form-control hours" /></td>
                                    <td style="text-align:center;cursor:default;"><i class="far fa-trash-alt edit fa-lg"></i></td>
                                </tr >
            `);

        $('#tblCategories').removeClass('hidden');
        $('#tblCategoriesEdit').removeClass('hidden');
        updateIndex();
        updateIndexEdit();
    });

    $(document).on("click", '.fa-trash-alt', function () {
        $(this).closest('tr').remove();

        if ($(this).hasClass('edit')) {
            var rowCount = $('#tbodyCategoriesEdit tr').length;
            if (rowCount === 0) {
                $('#tblCategoriesEdit').addClass('hidden');
            }
            updateIndexEdit();
        }
        else {
            var rowCount = $('#tbodyCategories tr').length;
            if (rowCount === 0) {
                $('#tblCategories').addClass('hidden');
            }
            updateIndex();
        }
    });

    $(document).on("change", '#displayDeleted', function () {
        if (this.checked) {
            UpdateProjectList(false);
        }
        else {
            UpdateProjectList(true);
        }
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
        "aoColumnDefs": [
            {
                "aTargets": [0, 4],
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
                    $('#tbodyCategories').empty();
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

                    $.ajax({
                        url: ajaxUrl + '/Projects/ProjectCategories/' + data.id,
                        type: "GET",
                        async: false,
                        success: function (response) {
                            $('#tbodyCategoriesEdit').empty();
                            $.each(response, function (index, item) {
                                $('#tbodyCategoriesEdit').append(`<tr draggable="true" style="border-bottom:1px solid #e2e5e8; height: 55px; cursor:all-scroll;" ondragstart="start()" ondragover="dragover()">
                                    <td class="hidden">${item.categoryId}</td>
                                    <td class="indexEdit"></td>
                                    <td>${$('#hidLanguage').val().toUpperCase() == 'FR' ? item.description_Fr : item.description_En}</td>
                                    <td><input id="txtHours" type="text" class="form-control hours" value="${item.hours}" /></td>
                                    <td style="text-align:center;cursor:default;"><i class="far fa-trash-alt edit fa-lg"></i></td>
                                </tr >
                                `);
                            });
                            updateIndexEdit();
                        },
                        error: function (xhr, status, error) {
                            alert('Error');
                        }
                    });
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
        var projectCategories = [];
        var sequence = 1;

        $('#tblCategories tbody tr').each(function () {
            $this = $(this);
            var categoryId = $this.find("td:eq(0)").text().trim();
            var hours = $this.find("input.hours").val();

            var projectCategoryViewModel = {
                ProjectId: 0, //doesn't exist
                CategoryId: categoryId,
                Hours: hours,
                Sequence: sequence++
            };

            projectCategories.push(projectCategoryViewModel);
        });

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
                CustomerId: $('#addForm select[name=SelectedCustomerId]').val(),
                ProjectsCategories: projectCategories
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
        var projectCategories = [];
        var sequence = 1;

        $('#tblCategoriesEdit tbody tr').each(function () {
            $this = $(this);
            var categoryId = $this.find("td:eq(0)").text().trim();
            var hours = $this.find("input.hours").val();

            var projectCategoryViewModel = {
                ProjectId: $('#editForm input[name=id]').val(),
                CategoryId: categoryId,
                Hours: hours,
                Sequence: sequence++
            };

            projectCategories.push(projectCategoryViewModel);
        });

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
                CustomerId: $('#editForm select[name=SelectedCustomerId]').val(),
                ProjectsCategories: projectCategories
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
    }

    function UpdateProjectList(activeOnly) {
        $.ajax({
            url: ajaxUrl + '/Projects/List',
            type: "GET",
            data: {
                organizationId: $('#hidSelectedOrganizationId').val(),
                activeOnly: activeOnly 
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

    function UpdateCategoryList(selectedOrganization) {
        $.ajax({
            url: ajaxUrl + '/Projects/Categories',
            type: "GET",
            data: {
                organizationId: $('#hidSelectedOrganizationId').val()
            },
            // dataType: "html",
            async: false,
            success: function (response) {
                $('select[name="SelectedCategoryId"]').empty().append('<option value="">' + $('#hidSelectOption').val() + '</option>');
                $.each(response, function (index, item) {
                    $('select[name="SelectedCategoryId"]').append(
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
        UpdateProjectList(!$('#displayDeleted').is(':checked'));
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
        UpdateProjectList(!$('#displayDeleted').is(':checked'));
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
        UpdateProjectList(!$('#displayDeleted').is(':checked'));
    }

    function delFail(xhr, status, error) {
        alert(xhr.responseText || error);
    }

});
