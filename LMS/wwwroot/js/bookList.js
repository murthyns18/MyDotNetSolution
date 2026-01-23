// ---------------- ACTION FORMATTERS ----------------
function actionFormatter(cellValue, options, row) {
    return `
        <div class="text-nowrap">
            <button class="btn btn-sm btn-warning me-1 btn-edit"
                    data-id="${row.bookID}">
                <i class="bi bi-pencil-square"></i>
            </button>

            <button class="btn btn-sm btn-danger btn-delete"
                    data-id="${row.bookID}">
                <i class="bi bi-trash"></i>
            </button>
        </div>`;
}

function statusFormatter(value) {
    return value
        ? "<span class='badge bg-success'>Active</span>"
        : "<span class='badge bg-danger'>Inactive</span>";
}

function reloadBookGrid() {
    $("#bookGrid")
        .jqGrid('setGridParam', {
            datatype: 'json',
            page: 1
        })
        .trigger('reloadGrid');
}

// ---------------- MODAL HANDLERS ----------------
function openAddModal() {
    $('#bookForm')[0].reset();
    $('#BookId').val(0);
    $('#bookModalTitle').text('Add Book');
    $('#bookForm').attr('action', '/Book/AddBook');
    $('#bookModal').modal('show');
}

function openEditModal(bookId) {
    $.get('/Book/EditBook', { bookID: bookId })
        .done(function (data) {
            $('#BookId').val(data.bookId);
            $('#Title').val(data.title);
            $('#ISBN').val(data.isbn);
            $('#Price').val(data.price);
            $('#Quantity').val(data.quantity);
            $('#PublisherID').val(data.publisherID);
            $('#CategoryID').val(data.categoryID);

            $('#bookModalTitle').text('Edit Book');
            $('#bookModal').modal('show');
        })
        .fail(function () {
            App.alert('Failed to load book details.');
        });
}

// ---------------- DELETE USING GLOBAL CONFIRM ----------------
function deleteBook(bookId) {

    confirm("Are you sure you want to delete this book?", function () {

        $.ajax({
            url: '/Book/DeleteBook',
            type: 'POST',
            data: {
                bookID: bookId,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function () {
                App.alert("Book deleted successfully");
                reloadBookGrid();
            },

            error: function () {
                App.alert("Delete failed");
            }
        });
    });
}

// ---------------- EVENTS ----------------
$(document).on('click', '.btn-edit', function () {
    openEditModal($(this).data('id'));
});

$(document).on('click', '.btn-delete', function () {
    deleteBook($(this).data('id'));
});

// ---------------- GRID INIT (USING LAYOUT) ----------------
$(function () {

    const colModels = [
        {
            label: "Action",
            name: "action",
            width: 90,
            sortable: false,
            search: false,
            align:"center",
            formatter: actionFormatter
        },
        { name: "bookID", key: true, hidden: true },
        { label: "Title", name: "title", width: 200, align: "left" },
        { label: "Price", name: "price", width: 80, align: "right" },
        { label: "Quantity", name: "quantity", width: 80, align: "right" },
        { label: "Publisher", name: "publisherName", width: 150, align: "left" },
        { label: "Category", name: "categoryName", width: 150, align: "left" },
        {
            label: "Status",
            name: "isActive",
            width: 90,
            formatter: statusFormatter,
            stype: "select",
            align:"center",
            searchoptions: { value: ":All;true:Active;false:Inactive" }
        }
    ];

    App.CreateJQGrid(
        '#bookGrid',
        apiURL + 'Book/BookList',
        'json',
        [],
        colModels,
        TOKEN,
        true,
        false,
        "55vh"
    );
});
