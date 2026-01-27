
function actionFormatter(cellValue, options, row) {

    let html = `
        <div class="text-nowrap">
            <button class="btn btn-sm btn-danger btn-delete me-1"
                    data-id="${row.loanId}" data-user="${row.userName}">
                <i class="bi bi-trash"></i>
            </button>
    `;

    if (row.status === "Active") {
        html += `
            <button class="btn btn-sm btn-success btn-return"
                    data-id="${row.loanId}" data-user="${row.userName}">
                <i class="bi bi-arrow-return-left"></i>
            </button>
        `;
    }

    html += `</div>`;
    return html;
}

function statusFormatter(value) {
    if (isExport) {
        return value;
    }

    return value === "Closed"
        ? "<span class='badge bg-secondary'>Closed</span>"
        : "<span class='badge bg-success'>Active</span>";
}


function dateFormatter(cellValue) {
    if (!cellValue) {
        return "<span class='text-danger fw-semibold'>Not Returned</span>";
    }

    const d = new Date(cellValue);
    if (isNaN(d)) {
        return "<span class='text-danger fw-semibold'>Not Returned</span>";
    }

    return d.toLocaleDateString('en-GB');
}


function reloadLoanGrid() {
    $("#loanGrid")
        .jqGrid('setGridParam', { page: 1 })
        .trigger('reloadGrid');
}

function deleteLoan(loanId, loan) {
    confirm(`Are you sure you want to delete this loan? "${loan}"`, function () {
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

function returnLoan(loanId, userName) {
    confirm(`Return loan for "${userName}"?`, function () {
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

$(document).on('click', '.btn-delete', function () {
    deleteLoan($(this).data('id'), $(this).data('user'));
});

$(document).on('click', '.btn-return', function () {
    returnLoan($(this).data('id'), $(this).data('user'));
});

$(function () {

    const colModels = [
        {
            label: "Action",
            name: "action",
            width: 120,
            align: "center",
            sortable: false,
            search: false,
            exportcol: false,
            formatter: actionFormatter
        },
        { name: "loanId", key: true, hidden: true },
        { label: "User Name", name: "userName", width: 150 },
        { label: "Total Qty", name: "totalQty", width: 80, align: "right" },
        {
            label: "Loan Date",
            name: "loanDate",
            width: 110,
            align: "center",
            formatter: dateFormatter
        },
        {
            label: "Due Date",
            name: "dueDate",
            width: 110,
            align: "center",
            formatter: dateFormatter
        },
        {
            label: "Return Date",
            name: "returnDate",
            width: 130,
            align: "center",
            formatter: dateFormatter
        },
        {
            label: "Status",
            name: "status",
            width: 120,
            align: "center",
            formatter: statusFormatter,
            stype: "select",
            searchoptions: {
                value: ":All;Active:Active;Closed:Closed"
            }
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

    $("#loanGrid").jqGrid('filterToolbar', {
        searchOnEnter: false,
        defaultSearch: "cn",
        stringResult: true
    });
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
