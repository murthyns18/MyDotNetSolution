function actionFormatter(cellValue, options, row) {
    return `
        <div class="text-nowrap">
            <button class="btn btn-sm btn-warning me-1 btn-edit" data-id="${row.userID}">
                <i class="bi bi-pencil-square"></i>
            </button>

            <button class="btn btn-sm btn-danger btn-delete"
                    data-id="${row.userID}"
                    data-name="${row.userName}">
                <i class="bi bi-trash"></i>
            </button>
        </div>`;
}

function statusFormatter(value) {
    return value
        ? "<span class='badge bg-success'>Active</span>"
        : "<span class='badge bg-danger'>Inactive</span>";
}

function reloadUserGrid() {
    $("#userGrid")
        .jqGrid('setGridParam', { page: 1 })
        .trigger('reloadGrid');
}

/* ---------------- ADD ---------------- */
function openAddUserModal() {
    $('#userForm')[0].reset();
    $('#UserID').val(0);

    $('#passwordContainer').removeClass('d-none');
    $('#confirmPasswordContainer').removeClass('d-none');
    $('#statusContainer').addClass('d-none');

    $('#userModalTitle').text('Add User');
    $('#userForm').attr('action', '/User/AddUser');
    $('#userModal').modal('show');
}

/* ---------------- EDIT ---------------- */
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

            $('#passwordContainer').addClass('d-none');
            $('#confirmPasswordContainer').addClass('d-none');
            $('#statusContainer').removeClass('d-none');

            $('#userModalTitle').text('Edit User');
            $('#userModal').modal('show');
        })
        .fail(function () {
            App.alert('Failed to load user details.');
        });
}

/* ---------------- DELETE ---------------- */
function deleteUser(userId, name) {
    confirm(`Are you sure you want to delete "${name}"?`, function () {
        $.ajax({
            url: '/User/DeleteUser',
            type: 'POST',
            data: {
                id: userId,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function () {
                App.alert("User deleted successfully");
                reloadUserGrid();
            }
        });
    });
}

/* ---------------- EVENTS ---------------- */
$(document).on('click', '.btn-edit', function () {
    openEditUserModal($(this).data('id'));
});

$(document).on('click', '.btn-delete', function () {
    deleteUser($(this).data('id'), $(this).data('name'));
});

/* ---------------- ROLE FILTER (same as Publisher/Category) ---------------- */
function getRoleFilter() {
    let result = ":All";
    $.ajax({
        url: apiURL + "Role/GetRoles",
        data: { roleId: -1 },
        async: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + TOKEN);
        },
        success: function (data) {
            data.forEach(r => {
                result += `;${r.roleName}:${r.roleName}`;
            });
        }
    });
    return result;
}

/* ---------------- GRID ---------------- */
$(function () {

    const colModels = [
        {
            label: "Action",
            name: "action",
            width: 90,
            sortable: false,
            search: false,
            align: "center",
            formatter: actionFormatter
        },
        { name: "userID", key: true, hidden: true },
        { label: "Name", name: "userName", width: 150 },
        { label: "Email", name: "email", width: 220 },
        { label: "Mobile", name: "mobileNumber", width: 120 },
        {
            label: "Role",
            name: "roleName",
            width: 150,
            stype: "select",
            searchoptions: {
                value: getRoleFilter(),
                sopt: ["eq"]
            }
        },
        { label: "Address", name: "address", width: 200 },
        {
            label: "Status",
            name: "status",
            width: 90,
            align: "center",
            formatter: statusFormatter,
            stype: "select",
            searchoptions: {
                value: ":All;true:Active;false:Inactive",
                sopt: ["eq"]
            }
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
