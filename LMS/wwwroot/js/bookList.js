function actionFormatter(cellValue, options, row) {
    return `
        <div class="text-nowrap">
            <button class="btn btn-sm btn-warning me-1 btn-edit" data-id="${row.bookID}">
                <i class="bi bi-pencil-square"></i>
            </button>

            <button class="btn btn-sm btn-danger btn-delete" data-id="${row.bookID}" data-title="${row.title}">
                <i class="bi bi-trash"></i>
            </button>
        </div>`;
}

function statusFormatter(value) {
    return value ? "<span class='badge bg-success'>Active</span>" : "<span class='badge bg-danger'>Inactive</span>";
}

function reloadBookGrid() {
    $("#bookGrid")
        .jqGrid('setGridParam', { page: 1 })
        .trigger('reloadGrid');
}

//modal for add
function openAddModal() {
    $('#bookForm')[0].reset();
    $('#BookId').val(0);
    $('#bookModalTitle').text('Add Book');
    $('#bookForm').attr('action', '/Book/AddBook');
    $('#bookModal').modal('show');
}

//modal for edit
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

            $('#IsActive').val(data.isActive.toString());
            $('#statusContainer').removeClass('d-none');

            $('#bookModalTitle').text('Edit Book');
            $('#bookForm').attr('action', '/Book/AddBook');
            $('#bookModal').modal('show');
        })
        .fail(function () {
            App.alert('Failed to load book details.');
        });
}


// delete using confirm
function deleteBook(bookId, title) {
    confirm(`Are you sure you want to delete this book? "${title}"`, function () {
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

// events for edit delete
$(document).on('click', '.btn-edit', function () {
    openEditModal($(this).data('id'));
});

$(document).on('click', '.btn-delete', function () {
    deleteBook($(this).data('id'), $(this).data('title'));
});

function getPublisherFilter() {
    let result = ":All";
    $.ajax({
        url: apiURL + "Publisher/PublisherList",
        data: { publisherID: -1 },
        async: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + TOKEN);
        },
        success: function (data) {
            data.forEach(p => {
                result += `;${p.publisherName}:${p.publisherName}`;
            });
        }
    });
    return result;
}

function getCategoryFilter() {
    let result = ":All";
    $.ajax({
        url: apiURL + "Category/CategoryList",
        data: { categoryID: -1 },
        async: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + TOKEN);
        },
        success: function (data) {
            data.forEach(c => {
                result += `;${c.categoryName}:${c.categoryName}`;
            });
        }
    });
    return result;
}

// grid
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
        { name: "bookID", key: true, hidden: true },
        { label: "Title", name: "title", width: 200 },
        { label: "Price", name: "price", width: 80, align: "right" },
        { label: "Quantity", name: "quantity", width: 80, align: "right" },
        {
            label: "Publisher",
            name: "publisherName",
            width: 150,
            stype: "select",
            searchoptions: { value: getPublisherFilter(), sopt: ["eq"] }
        },
        {
            label: "Category",
            name: "categoryName",
            width: 150,
            stype: "select",
            searchoptions: { value: getCategoryFilter(), sopt: ["eq"] }
        },
        {
            label: "Status",
            name: "isActive",
            width: 90,
            formatter: statusFormatter,
            stype: "select",
            align: "center",
            searchoptions: { value: ":All;true:Active;false:Inactive", sopt: ["eq"] }
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
