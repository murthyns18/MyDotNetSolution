
function roleActionFormatter(cellValue, options, row) {

    var token = $('input[name="__RequestVerificationToken"]').val();

    return `
<div style="white-space:nowrap;">
    <a href="/Role/EditRole?q=${Encrypt('roleID='+row.roleID)}"
       class="btn btn-sm btn-warning me-1"
       title="Edit Role">
        <i class="bi bi-pencil-square"></i>
    </a>

    <form method="post"
          action="/Role/DeleteRole"
          style="display:inline;">
        <input type="hidden" name="roleID" value="${row.roleID}" />
        <input type="hidden" name="__RequestVerificationToken" value="${token}" />
        <button type="submit"
                class="btn btn-sm btn-danger"
                title="Delete Role">
            <i class="bi bi-trash"></i>
        </button>
    </form>
</div>
`;
}

function roleStatusFormatter(value) {
    return value
        ? "<span class='badge bg-success'><i class='bi bi-check-circle'></i> Active</span>"
        : "<span class='badge bg-danger'><i class='bi bi-x-circle'></i> Inactive</span>";
}

$(function () {

    $("#roleGrid").jqGrid({
        url: '/Role/GetRolesForGrid',
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
                formatter: roleActionFormatter,
                cellattr: () => "style='white-space:nowrap;'"
            },
            { label: "ID", name: "roleID", key: true, hidden: true },
            { label: "Role Name", name: "roleName", width: 200, sortable: true },
            {
                label: "Status",
                name: "isActive",
                width: 100,
                align: "center",
                formatter: roleStatusFormatter,
                stype: "select",
                searchoptions: {
                    value: ":All;true:Active;false:Inactive"
                }
            }
        ],

        pager: "#rolePager",
        rowNum: 10,
        rowList: [10, 20, 50],
        autowidth: true,
        shrinkToFit: true,
        height: "auto",
        loadonce: true,
        viewrecords: true,
        caption: "<i class='bi bi-shield-lock'></i> Role List",

        jsonReader: {
            root: "rows",
            records: "records",
            repeatitems: false,
            id: "roleID"
        }
    });

    $("#roleGrid").jqGrid('filterToolbar', {
        stringResult: true,
        searchOnEnter: false,
        defaultSearch: "cn"
    });

    setTimeout(function () {
        $(".ui-search-input input").attr("placeholder", "🔍 Search");
    }, 200);
});
