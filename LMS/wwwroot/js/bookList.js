function actionFormatter(cellValue, options, row) {
    return `
    <div style="white-space:nowrap;">
        <button class="btn btn-sm btn-warning me-1"
                onclick="openEditModal(${row.bookID})">
            <i class="bi bi-pencil-square"></i>
        </button>

        <button class="btn btn-sm btn-danger"
                onclick="confirmDelete(${row.bookID})">
            <i class="bi bi-trash"></i>
        </button>
    </div>`;
}


function openAddModal() {
    $('#bookForm')[0].reset();
    $('#BookId').val(0);
    $('#bookModalTitle').text('Add Book');
    $('#bookForm').attr('action', '/Book/AddBook');

    const modal = new bootstrap.Modal(document.getElementById('bookModal'));
    modal.show();
}

function openEditModal(bookId) {
    $.get('/Book/EditBook', { bookID: bookId }, function (data) {

        $('#BookId').val(data.bookId);
        $('#Title').val(data.title);
        $('#ISBN').val(data.isbn);
        $('#Price').val(data.price);
        $('#Quantity').val(data.quantity);
        $('#PublisherID').val(data.publisherID);
        $('#CategoryID').val(data.categoryID);

        $('#bookModalTitle').text('Edit Book');
        $('#bookForm').attr('action', '/Book/AddBook');

        const modal = new bootstrap.Modal(document.getElementById('bookModal'));
        modal.show();
    });
}


function confirmDelete(bookId) {
    $('#deleteBookId').val(bookId);

    const modalEl = document.getElementById('deleteModal');
    const modal = new bootstrap.Modal(modalEl);
    modal.show();
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
                align: "right"
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

});