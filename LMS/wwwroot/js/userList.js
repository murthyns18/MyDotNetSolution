function actionFormatter(cellValue, options, row) {
    return `
        <div class="text-nowrap">
            <button class="btn btn-sm btn-warning me-1 btn-edit" data-id="${row.userID}">
                <i class="bi bi-pencil-square"></i>
            </button>

            <button class="btn btn-sm btn-danger btn-delete" data-id="${row.userID}" data-name="${row.userName}">
                <i class="bi bi-trash"></i>
            </button>
        </div>`;
}

function statusFormatter(cellValue) {
    if (isExport) {
        return cellValue ? "Active" : "Inactive";
    }
    return cellValue
        ? "<span class='badge bg-success'>Active</span>"
        : "<span class='badge bg-danger'>Inactive</span>";
}


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


function deleteUser(userId, name) {
    confirm(`Are you sure you want to delete "${name}"?`, function () {
        $.ajax({
            url: '/User/DeleteUser',
            type: 'POST',
            data: {
                id: userId,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (result) {

                if (result.success) {
                    App.alert(result.message);

                    $("#userGrid").jqGrid('delRowData', userId);
                } else {
                    App.alert(result.message);
                }
            },
            error: function () {
                App.alert("Delete failed");
            }
        });
    });
}


$(document).on('click', '.btn-edit', function () {
    openEditUserModal($(this).data('id'));
});

$(document).on('click', '.btn-delete', function () {
    deleteUser($(this).data('id'), $(this).data('name'));
});


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

$(function () {

    const colModels = [
        {
            label: "Action",
            name: "action",
            width: 90,
            sortable: false,
            exportcol: false,
            search: false,
            align: "center",
            formatter: actionFormatter
        },
        { name: "userID", key: true, hidden: true },
        { label: "Name", name: "userName", width: 150 },
        { label: "Email", name: "email", width: 220 },
        { label: "Mobile", name: "mobileNumber", width: 120, align:"right" },
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

submitModalForm({
    formSelector: '#userForm',
    modalSelector: '#userModal',
    onSuccess: function (res) {

        if (!res.data) return;

        const userId = res.data.userID;

        if ($('#UserID').val() == 0) {
            $("#userGrid").jqGrid('addRowData', userId, res.data, "first");
        }
        else { 
            $("#userGrid").jqGrid('setRowData', userId, res.data);
        }
    }

});
