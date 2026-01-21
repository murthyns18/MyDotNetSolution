function actionFormatter(cellValue, options, row) {

    var token = $('input[name="__RequestVerificationToken"]').val();

    return `
    <div style="white-space:nowrap;">
      <a href="/Book/EditBook?q=${Encrypt('bookID=' + row.bookId)}"
       class="btn btn-sm btn-warning me-1"
       title="Edit Book">
        <i class="bi bi-pencil-square"></i>
    </a>

    <form method="post"
          action="/Book/DeleteBook"
          style="display:inline;">
        <input type="hidden" name="bookID" value="${row.bookId}" />
        <input type="hidden" name="__RequestVerificationToken" value="${token}" />
        <button type="submit"
                class="btn btn-sm btn-danger"
                title="Delete Book">
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

    $("#bookGrid").jqGrid({
        url: '/Book/GetBooksForGrid',
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
            { label: "ID", name: "bookID", key: true, hidden: true },
            { label: "Title", name: "title", width: 200, sortable: true },
            {
                label: "Price",
                name: "price",
                width: 90,
                align: "right",
                formatter: "currency",
                formatoptions: { prefix: "₹ " }
            },
            { label: "Quantity", name: "quantity", width: 90, align: "center" },
            { label: "Publisher", name: "publisherName", width: 150 },
            { label: "Category", name: "categoryName", width: 150 },
            {
                label: "Status",
                name: "isActive",
                width: 100,
                align: "center",
                formatter: statusFormatter,
                stype: "select",
                searchoptions: {
                    value: ":All;true:Active;false:Inactive"
                }
            }
        ],

        pager: "#bookPager",
        rowNum: 10,
        rowList: [10, 20, 50],
        autowidth: true,
        shrinkToFit: true,
        height: "auto",
        loadonce: true,
        viewrecords: true,
        caption: "<i class='bi bi-book'></i> Book List",

        jsonReader: {
            root: "rows",
            records: "records",
            repeatitems: false,
            id: "bookID"
        }
    });

    /* Enable column search */
    $("#bookGrid").jqGrid('filterToolbar', {
        stringResult: true,
        searchOnEnter: false,
        defaultSearch: "cn"
    });

    /* Search placeholder */
    setTimeout(function () {
        $(".ui-search-input input").attr("placeholder", "🔍 Search");
    }, 200);
});
