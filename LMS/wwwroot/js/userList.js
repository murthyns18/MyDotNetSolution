function actionFormatter(cellValue, options, row) {
    return `
            <div class="jqgrid-action">
                <a href="/User/EditUser/${row.userID}" class="btn btn-sm btn-info text-white">Edit</a>
 
                <form method="post" action="/User/DeleteUser" style="display:inline;"">
                    <input type="hidden" name="id" value="${row.userID}" />
                    <button type="submit" class="btn btn-sm btn-danger">Delete</button>
                </form>
            </div>
        `;
}

function statusFormatter(value) {
    return value ? "<span class='badge bg-success'>Active</span>" : "<span class='badge bg-danger'>Inactive</span>";
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
                width: 140,
                fixed: true,
                align: "center",
                formatter: actionFormatter,
                cellattr: function () {
                    return "style='white-space:nowrap;'";
                }
            }
            ,
            { label: "ID", name: "userID", key: true, width: 50, align: "center", hidden: true },
            { label: "Name", name: "userName", width: 150 },
            { label: "Email", name: "email", width: 220 },
            { label: "Mobile", name: "mobileNumber", width: 120 },
            { label: "Role", name: "roleName", width: 120 },
            { label: "Address", name: "address", width: 200 },
            {
                label: "Status",
                name: "status",
                width: 90,
                align: "center",
                formatter: statusFormatter
            }
        ],

        pager: "#userPager",
        rowNum: 10,
        rowList: [10, 20, 50, 60],
        autowidth: true,
        shrinkToFit: true,
        height: "auto",
        viewrecords: true,
        caption: "User List",

        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            id: "userID"
        }
    });
});