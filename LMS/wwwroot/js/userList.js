function actionFormatter(cellValue, options, row) {

    var token = $('input[name="__RequestVerificationToken"]').val();

    return `
<div style="white-space:nowrap;">
    <a href="/User/EditUser?q=${Encrypt('userID='+ row.userID)}"
       class="btn btn-sm btn-warning me-1"
       title="Edit User">
        <i class="bi bi-pencil-square"></i>
    </a>

    <form method="post"
          action="/User/DeleteUser"
          style="display:inline;">
        <input type="hidden" name="id" value="${row.userID}" />
        <input type="hidden" name="__RequestVerificationToken" value="${token}" />
        <button type="submit"
                class="btn btn-sm btn-danger"
                title="Delete User">
            <i class="bi bi-trash"></i>
        </button>
    </form>
</div>
`;
}
function statusFormatter(value) {
    return value
        ? "<span class='badge bg-success'><i class='bi bi-check-circle'></i> Active</span>"
        : "<span class='badge bg-danger'><i class='bi bi-x-circle'></i> Inactive</span>";
}

$(function () {

    $("#userGrid").jqGrid({
        url: '/User/GetUsersForGrid',
        datatype: "json",
        mtype: "GET",

        colModel: [
            {
                label: "Action",
                name: "action",
                width: 100,
                fixed: true,
                align: "center",
                sortable: false,
                search: false,
                formatter: actionFormatter,
                cellattr: () => "style='white-space:nowrap;'"
            },
            { label: "ID", name: "userID", key: true, hidden: true },
            { label: "Name", name: "userName", width: 150, sortable: true },
            { label: "Email", name: "email", width: 220, sortable: true },
            { label: "Mobile", name: "mobileNumber", width: 120 },
            { label: "Role", name: "roleName", width: 120 },
            { label: "Address", name: "address", width: 200 },
            {
                label: "Status",
                name: "status",
                width: 100,
                align: "center",
                formatter: statusFormatter,
                stype: "select",
                searchoptions: {
                    value: ":All;true:Active;false:Inactive"
                }
            }
        ],

        pager: "#userPager",
        rowNum: 10,
        rowList: [10, 20, 50],
        autowidth: true,
        shrinkToFit: true,
        height: "auto",
        loadonce: true,
        viewrecords: true,
        caption: "<i class='bi bi-people'></i> User List",

        jsonReader: {
            root: "rows",
            records: "records",
            repeatitems: false,
            id: "userID"
        }
    });

    $("#userGrid").jqGrid('filterToolbar', {
        stringResult: true,
        searchOnEnter: false,
        defaultSearch: "cn"
    });

});
