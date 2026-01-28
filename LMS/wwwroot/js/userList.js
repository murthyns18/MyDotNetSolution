/* ================= ACTION & STATUS FORMATTERS ================= */

function actionFormatter(cellValue, options, row) {
    return `
        <div class="text-nowrap">
            <button class="btn btn-sm btn-warning me-1 btn-edit" data-id="${row.userID}">
                <i class="bi bi-pencil-square"></i>
            </button>
            <button class="btn btn-sm btn-danger btn-delete"
                    data-id="${row.userID}" data-name="${row.userName}">
                <i class="bi bi-trash"></i>
            </button>
        </div>`;
}

function statusFormatter(cellValue) {
    if (typeof isExport !== "undefined" && isExport) {
        return cellValue ? "Active" : "Inactive";
    }
    return cellValue
        ? "<span class='badge bg-success'>Active</span>"
        : "<span class='badge bg-danger'>Inactive</span>";
}

/* ================= MODAL HANDLING ================= */

function openAddUserModal() {
    $('#userForm')[0].reset();
    $('#UserID').val(0);

    // Reset location
    $('#CountryId, #StateId, #CityId').val('');
    $('#stateColumn, #cityColumn').addClass('d-none');

    loadCountries();

    $('#passwordContainer, #confirmPasswordContainer').removeClass('d-none');
    $('#statusContainer').addClass('d-none');

    $('#userModalTitle').text('Add User');
    $('#userModal').modal('show');
}

function openEditUserModal(userId) {
    $.get('/User/EditUser', { userID: userId })
        .done(function (data) {

            $('#UserID').val(data.userID);
            $('#UserName').val(data.userName);
            $('#Email').val(data.email);
            $('#MobileNumber').val(data.mobileNumber);
            $('#Address').val(data.address);
            $('#RoleID').val(data.roleID);
            $('input[name="Status"][value="' + data.status + '"]').prop('checked', true);

            // Reset location
            $('#CountryId, #StateId, #CityId').val('');
            $('#stateColumn, #cityColumn').addClass('d-none');
            loadCountries();

            // Drill-down selection
            setTimeout(() => {
                $('#countryList li[data-id="' + data.countryId + '"]').trigger('click');
                setTimeout(() => {
                    $('#stateList li[data-id="' + data.stateId + '"]').trigger('click');
                    setTimeout(() => {
                        $('#cityList li[data-id="' + data.cityId + '"]').addClass('active');
                        $('#CityId').val(data.cityId);
                    }, 300);
                }, 300);
            }, 300);

            $('#passwordContainer, #confirmPasswordContainer').addClass('d-none');
            $('#statusContainer').removeClass('d-none');

            $('#userModalTitle').text('Edit User');
            $('#userModal').modal('show');
        })
        .fail(() => App.alert('Failed to load user details.'));
}

/* ================= DELETE ================= */

function deleteUser(userId, name) {
    confirm(`Are you sure you want to delete "${name}"?`, function () {
        $.ajax({
            url: '/User/DeleteUser',
            type: 'POST',
            data: {
                id: userId,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: res => {
                App.alert(res.message);
                $("#userGrid").jqGrid('delRowData', userId);
            },
            error: () => App.alert("Delete failed")
        });
    });
}

/* ================= EVENTS ================= */

$(document).on('click', '.btn-edit', function () {
    openEditUserModal($(this).data('id'));
});

$(document).on('click', '.btn-delete', function () {
    deleteUser($(this).data('id'), $(this).data('name'));
});

/* ================= ROLE FILTER ================= */

function getRoleFilter() {
    let result = ":All";
    $.ajax({
        url: apiURL + "Role/GetRoles",
        data: { roleId: -1 },
        async: false,
        beforeSend: xhr => xhr.setRequestHeader("Authorization", "Bearer " + TOKEN),
        success: data => {
            data.forEach(r => {
                result += `;${r.roleName}:${r.roleName}`;
            });
        }
    });
    return result;
}

/* ================= JQGRID ================= */

$(function () {
    const colModels = [
        { label: "Action", name: "action", width: 90, align: "center", formatter: actionFormatter, search: false },
        { name: "userID", key: true, hidden: true },
        { label: "Name", name: "userName", width: 150 },
        { label: "Email", name: "email", width: 220 },
        { label: "Mobile", name: "mobileNumber", width: 120, align: "right" },
        {
            label: "Role",
            name: "roleName",
            width: 150,
            stype: "select",
            searchoptions: { value: getRoleFilter(), sopt: ["eq"] }
        },
        { label: "Address", name: "address", width: 200 },
        {
            label: "Status",
            name: "status",
            width: 90,
            align: "center",
            formatter: statusFormatter,
            stype: "select",
            searchoptions: { value: ":All;true:Active;false:Inactive", sopt: ["eq"] }
        }
    ];

    App.CreateJQGrid(
        '#userGrid',
        apiURL + 'User/UserList',
        'json',
        [],
        colModels,
        TOKEN,
        true,
        false,
        "55vh"
    );
});

/* ================= FORM SUBMIT ================= */

submitModalForm({
    formSelector: '#userForm',
    modalSelector: '#userModal',
    onSuccess: function (res) {
        if (!res.data) return;

        const userId = res.data.userID;
        if ($('#UserID').val() == 0) {
            $("#userGrid").jqGrid('addRowData', userId, res.data, "first");
        } else {
            $("#userGrid").jqGrid('setRowData', userId, res.data);
        }
    }
});

/* ================= LOCATION MENU ================= */

/* LOAD COUNTRIES */
function loadCountries() {
    $.ajax({
        url: apiURL + "User/GetCountries",
        beforeSend: xhr => xhr.setRequestHeader("Authorization", "Bearer " + TOKEN),
        success: function (data) {
            let html = '';
            data.forEach(c => {
                html += `<li data-id="${c.countryId}">${c.countryName}</li>`;
            });
            $('#countryList').html(html);
        }
    });
}

/* COUNTRY → STATE */
$('#countryList').on('click', 'li', function () {
    const countryId = $(this).data('id');
    $('#CountryId').val(countryId);

    $('#countryList li').removeClass('active');
    $(this).addClass('active');

    $('#stateColumn').removeClass('d-none');
    $('#cityColumn').addClass('d-none');
    $('#StateId, #CityId').val('');

    $.ajax({
        url: apiURL + "User/GetStates",
        data: { countryId },
        beforeSend: xhr => xhr.setRequestHeader("Authorization", "Bearer " + TOKEN),
        success: function (states) {
            let html = '';
            states.forEach(s => {
                html += `<li data-id="${s.stateId}">${s.stateName}</li>`;
            });
            $('#stateList').html(html);
        }
    });
});

/* STATE → CITY */
$('#stateList').on('click', 'li', function () {
    const stateId = $(this).data('id');
    $('#StateId').val(stateId);

    $('#stateList li').removeClass('active');
    $(this).addClass('active');

    $('#cityColumn').removeClass('d-none');
    $('#CityId').val('');

    $.ajax({
        url: apiURL + "User/GetCities",
        data: { stateId },
        beforeSend: xhr => xhr.setRequestHeader("Authorization", "Bearer " + TOKEN),
        success: function (cities) {
            let html = '';
            cities.forEach(c => {
                html += `<li data-id="${c.cityId}">${c.cityName}</li>`;
            });
            $('#cityList').html(html);
        }
    });
});

/* FINAL CITY */
$('#cityList').on('click', 'li', function () {
    $('#CityId').val($(this).data('id'));
    $('#cityList li').removeClass('active');
    $(this).addClass('active');
});
