function dateFormatter(value) {
    if (!value) return "";
    return new Date(value).toLocaleDateString("en-IN");
}

function dueDateFormatter(value, options, row) {
    if (!value) return "";

    var due = new Date(value);
    var today = new Date();

    if (!row.returnDate && due < today) {
        return `<span class="text-danger fw-bold">
                    ${due.toLocaleDateString("en-IN")}
                </span>`;
    }

    return due.toLocaleDateString("en-IN");
}

/* STATUS FORMATTER (DISPLAY) */
function loanStatusFormatter(cellValue, options, row) {

    if (row.returnDate) {
        return "<span class='badge bg-secondary'>Returned</span>";
    }

    if (row.dueDate && new Date(row.dueDate) < new Date()) {
        return "<span class='badge bg-danger'>Overdue</span>";
    }

    return "<span class='badge bg-success'>Active</span>";
}

/* STATUS UNFORMATTER (FOR SEARCH) */
function loanStatusUnFormatter(cellValue, options, cell) {
    return $(cell).text(); // Extract text from badge
}

function loanActionFormatter(cellValue, options, row) {

    var token = $('input[name="__RequestVerificationToken"]').val();
    var isReturned = row.returnDate !== null;
    var isOverdue = row.dueDate && new Date(row.dueDate) < new Date();

    return `
    <div style="white-space:nowrap;">
        <button class="btn btn-sm btn-info me-1"
                title="View Loan"
                onclick="viewLoanDetails(${row.loanId})">
            <i class="bi bi-eye"></i>
        </button>

        ${isReturned ? "" : `
        <form method="post"
              action="/Loan/ReturnLoan"
              style="display:inline;">
            <input type="hidden" name="loanId" value="${row.loanId}" />
            <input type="hidden" name="__RequestVerificationToken" value="${token}" />
            <button class="btn btn-sm btn-success"
                    title="Return Loan"
                    ${isOverdue ? "disabled" : ""}>
                <i class="bi bi-arrow-return-left"></i>
            </button>
        </form>`}
    </div>`;
}

function viewLoanDetails(loanId) {
    $.get("/Loan/LoanDetails", { loanId }, function (html) {
        $("#loanDetailsBody").html(html);
        $("#loanDetailsModal").modal("show");
    });
}

$(function () {

    $("#loanGrid").jqGrid({
        url: '/Loan/GetLoansForGrid',
        datatype: "json",
        mtype: "GET",

        colModel: [
            {
                label: "Action",
                width: 110,
                fixed: true,
                align: "center",
                sortable: false,
                search: false,
                formatter: loanActionFormatter,
                cellattr: () => "style='white-space:nowrap;'"
            },
            { name: "loanId", key: true, hidden: true },

            { label: "Borrower", name: "borrowerName", width: 200 },

            { label: "Loan Date", name: "loanDate", width: 120, formatter: dateFormatter },

            { label: "Due Date", name: "dueDate", width: 120, formatter: dueDateFormatter },

            {
                label: "Status",
                width: 120,
                align: "center",
                formatter: loanStatusFormatter,
                unformat: loanStatusUnFormatter,
                stype: "select",
                searchoptions: {
                    value: ":All;Active:Active;Overdue:Overdue;Returned:Returned"
                }
            }
        ],

        pager: "#loanPager",
        rowNum: 10,
        rowList: [10, 20, 50],

        autowidth: true,
        shrinkToFit: true,
        height: "auto",
        loadonce: true,
        viewrecords: true,

        caption: "<i class='bi bi-journal-text'></i> Loan List",

        rowattr: function (row) {
            if (!row.returnDate && row.dueDate && new Date(row.dueDate) < new Date()) {
                return { class: "table-warning" };
            }
        },

        jsonReader: {
            root: "rows",
            records: "records",
            repeatitems: false,
            id: "loanId"
        }
    });

    /* 🔍 ENABLE FILTER TOOLBAR (MUST BE HERE) */
    $("#loanGrid").jqGrid('filterToolbar', {
        stringResult: true,
        searchOnEnter: false,
        defaultSearch: "cn"
    });
});

/* 🔁 FULL WIDTH RESIZE HANDLER */
$(window).on("resize", function () {
    var newWidth = $("#loanGrid").closest(".container-fluid").width();
    $("#loanGrid").jqGrid("setGridWidth", newWidth, true);
});
