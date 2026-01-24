function actionFormatter(cellValue, options, row) {

    return `
        <div class="text-nowrap">
            <button class="btn btn-sm btn-danger btn-delete"
                    data-id="${row.loanId}">
                <i class="bi bi-trash"></i>
            </button>
        </div>`;
}

function statusFormatter(value, options, row) {
    return row.returnDate
        ? "<span class='badge bg-secondary'>Returned</span>"
        : "<span class='badge bg-success'>Active</span>";
}

function reloadLoanGrid() {
    $("#loanGrid")
        .jqGrid('setGridParam', { page: 1 })
        .trigger('reloadGrid');
}

/* ---------- DELETE ---------- */
function deleteLoan(loanId) {
    confirm("Are you sure you want to delete this loan?", function () {
        $.ajax({
            url: '/Loan/DeleteLoan',
            type: 'POST',
            data: {
                loanId: loanId,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (res) {
                App.alert(res.message);
                reloadLoanGrid();
            },
            error: function () {
                App.alert("Delete failed");
            }
        });
    });
}

/* ---------- EVENTS ---------- */
$(document).on('click', '.btn-delete', function () {
    deleteLoan($(this).data('id'));
});

/* ---------- GRID ---------- */
$(function () {

    const colModels = [
        {
            label: "Action",
            name: "action",
            width: 80,
            align: "center",
            sortable: false,
            search: false,
            formatter: actionFormatter
        },
        { name: "loanId", key: true, hidden: true },
        { label: "User Name", name: "userName", width: 150 },
        { label: "Total Qty", name: "totalQty", width: 80, align: "right" },
        { label: "Loan Date", name: "loanDate", width: 110, align: "center" },
        { label: "Due Date", name: "dueDate", width: 110, align: "center" },
        { label: "Return Date", name: "returnDate", width: 110, align: "center" },
        {
            label: "Status",
            name: "status",
            width: 90,
            align: "center",
            formatter: statusFormatter,
            search: false
        }
    ];

    App.CreateJQGrid(
        '#loanGrid',
        apiURL + 'Loan/LoanList?loanId=0',
        'json',
        [],
        colModels,
        TOKEN,
        true,
        false,
        "55vh"
    );
});


function openAddLoanModal() {
    selectedBooks = [];
    $("#loanDetailsContainer").empty();
    $("#tblSelectedBooks tbody").html(`
        <tr>
            <td colspan="4" class="text-center text-muted">
                No books added
            </td>
        </tr>
    `);

    $("#loanModal").modal("show");
}
