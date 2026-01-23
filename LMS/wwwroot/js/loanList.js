function actionFormatter(cellValue, options, row) {

    let returnBtn = '';
    if (!row.returnDate) {
        returnBtn = `
            <button class="btn btn-sm btn-success me-1 btn-return" data-id="${row.loanId}">
                <i class="bi bi-arrow-return-left"></i>
            </button>`;
    }

    return `
        <div class="text-nowrap">
            ${returnBtn}
            <button class="btn btn-sm btn-danger btn-delete" data-id="${row.loanId}">
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

// delete loan
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

// return loan
function returnLoan(loanId) {
    confirm("Are you sure you want to return this loan?", function () {
        $.ajax({
            url: '/Loan/ReturnLoan',
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
                App.alert("Return failed");
            }
        });
    });
}

// events
$(document).on('click', '.btn-delete', function () {
    deleteLoan($(this).data('id'));
});

$(document).on('click', '.btn-return', function () {
    returnLoan($(this).data('id'));
});

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
        { name: "loanId", key: true, hidden: true },

        { label: "User ID", name: "userId", width: 80, align: "center" },
        { label: "Total Qty", name: "totalQty", width: 80, align: "center" },
        { label: "Loan Date", name: "loanDate", width: 110 },
        { label: "Due Date", name: "dueDate", width: 110 },
        { label: "Return Date", name: "returnDate", width: 110 },

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
