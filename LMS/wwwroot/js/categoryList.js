
function categoryActionFormatter(cellValue, options, row) {

    var token = $('input[name="__RequestVerificationToken"]').val();

    return `
<div style="white-space:nowrap;">
    <a href="/Category/EditCategory?q=${Encrypt('categoryID='+ row.categoryID)}"
       class="btn btn-sm btn-warning me-1"
       title="Edit Category">
        <i class="bi bi-pencil-square"></i>
    </a>

    <form method="post" action="/Category/DeleteCategory" style="display:inline;">
        <input type="hidden" name="id" value="${row.categoryID}" />
        <input type="hidden" name="__RequestVerificationToken" value="${token}" />
        <button type="submit" class="btn btn-sm btn-danger" title="Delete Category">
            <i class="bi bi-trash"></i>
        </button>
    </form>


</div>
`;
}

function categoryStatusFormatter(value) {
    return value
        ? "<span class='badge bg-success'><i class='bi bi-check-circle'></i> Active</span>"
        : "<span class='badge bg-danger'><i class='bi bi-x-circle'></i> Inactive</span>";
}

function categoryStatusUnformatter(cellValue) {
    return cellValue.indexOf("Active") !== -1;
}

$(function () {

    $("#categoryGrid").jqGrid({
        url: '/Category/GetCategoriesForGrid',
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
                formatter: categoryActionFormatter,
                cellattr: () => "style='white-space:nowrap;'"
            },

            { label: "ID", name: "categoryID", key: true, hidden: true },

            {
                label: "Category Name",
                name: "categoryName",
                width: 220,
                sortable: true,
                search: true
            },
            {
                label: "Status",
                name: "isActive",
                width: 120,
                align: "center",
                formatter: categoryStatusFormatter,
                unformat: categoryStatusUnformatter,
                stype: "select",
                searchoptions: {
                    value: ":All;true:Active;false:Inactive",
                    sopt: ["eq"]
                }
            }
        ],

        pager: "#categoryPager",
        rowNum: 10,
        rowList: [10, 20, 50],
        autowidth: true,
        shrinkToFit: true,
        height: "auto",
        loadonce: true,
        viewrecords: true,
        caption: "<i class='bi bi-tags'></i> Category List",

        jsonReader: {
            root: "rows",
            records: "records",
            repeatitems: false,
            id: "categoryID"
        }
    });

    $("#categoryGrid").jqGrid('filterToolbar', {
        stringResult: true,
        searchOnEnter: false,
        defaultSearch: "cn"
    });
});
