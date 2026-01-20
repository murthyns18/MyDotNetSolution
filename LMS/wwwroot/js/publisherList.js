$(document).ready(function () {
    $(".notification").delay(3000).fadeOut("slow");
});

/* ================= ACTION COLUMN ================= */
function publisherActionFormatter(cellValue, options, row) {

    var token = $('input[name="__RequestVerificationToken"]').val();

    return `
<div style="white-space:nowrap;">
    <a href="/Publisher/EditPublisher/${row.publisherID}"
       class="btn btn-sm btn-warning me-1"
       title="Edit Publisher">
        <i class="bi bi-pencil-square"></i>
    </a>

    <form method="post"
          action="/Publisher/DeletePublisher"
          style="display:inline;">
        <input type="hidden" name="publisherID" value="${row.publisherID}" />
        <input type="hidden" name="__RequestVerificationToken" value="${token}" />
        <button type="submit"
                class="btn btn-sm btn-danger"
                title="Delete Publisher">
            <i class="bi bi-trash"></i>
        </button>
    </form>
</div>
`;
}

/* ================= STATUS ================= */
function publisherStatusFormatter(value) {
    return value
        ? "<span class='badge bg-success'><i class='bi bi-check-circle'></i> Active</span>"
        : "<span class='badge bg-danger'><i class='bi bi-x-circle'></i> Inactive</span>";
}

/* ================= GRID ================= */
$(function () {

    $("#publisherGrid").jqGrid({
        url: '/Publisher/GetPublishersForGrid',
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
                formatter: publisherActionFormatter,
                cellattr: () => "style='white-space:nowrap;'"
            },
            { label: "ID", name: "publisherID", key: true, hidden: true },
            { label: "Publisher Name", name: "publisherName", width: 200, sortable: true },
            {
                label: "Status",
                name: "isActive",
                width: 100,
                align: "center",
                formatter: publisherStatusFormatter,
                stype: "select",
                searchoptions: {
                    value: ":All;true:Active;false:Inactive"
                }
            }
        ],

        pager: "#publisherPager",
        rowNum: 10,
        rowList: [10, 20, 50],
        autowidth: true,
        shrinkToFit: true,
        height: "auto",
        loadonce: true,
        viewrecords: true,
        caption: "<i class='bi bi-building'></i> Publisher List",

        jsonReader: {
            root: "rows",
            records: "records",
            repeatitems: false,
            id: "publisherID"
        }
    });

    $("#publisherGrid").jqGrid('filterToolbar', {
        stringResult: true,
        searchOnEnter: false,
        defaultSearch: "cn"
    });

    setTimeout(function () {
        $(".ui-search-input input").attr("placeholder", "🔍 Search");
    }, 200);
});
