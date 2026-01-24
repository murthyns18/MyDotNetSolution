function actionFormatter(cellValue, options, row) {
    return `
        <div class="text-nowrap">
            <button class="btn btn-sm btn-warning me-1 btn-edit" data-id="${row.publisherID}">
                <i class="bi bi-pencil-square"></i>
            </button>
            <button class="btn btn-sm btn-danger btn-delete"
                    data-id="${row.publisherID}"
                    data-name="${row.publisherName}">
                <i class="bi bi-trash"></i>
            </button>
        </div>`;
}

function statusFormatter(value) {
    return value
        ? "<span class='badge bg-success'>Active</span>"
        : "<span class='badge bg-danger'>Inactive</span>";
}

function reloadPublisherGrid() {
    $("#publisherGrid")
        .jqGrid('setGridParam', { page: 1 })
        .trigger('reloadGrid');
}

/* ---------------- ADD ---------------- */
function openAddPublisherModal() {
    $('#publisherForm')[0].reset();
    $('#PublisherID').val(0);

    // ❌ hide status on add
    $('#statusContainer').addClass('d-none');

    $('#publisherModalTitle').text('Add Publisher');
    $('#publisherModal').modal('show');
}

/* ---------------- EDIT ---------------- */
function openEditPublisherModal(publisherId) {
    $.get('/Publisher/EditPublisher', { publisherID: publisherId })
        .done(function (data) {

            $('#PublisherID').val(data.publisherID);
            $('#PublisherName').val(data.publisherName);

            $('input[name="IsActive"][value="' + data.isActive + '"]').prop('checked', true);

            // ✅ show status on edit
            $('#statusContainer').removeClass('d-none');

            $('#publisherModalTitle').text('Edit Publisher');
            $('#publisherModal').modal('show');
        })
        .fail(function () {
            App.alert('Failed to load publisher details.');
        });
}

/* ---------------- DELETE ---------------- */
function deletePublisher(id, name) {
    confirm(`Are you sure you want to delete "${name}"?`, function () {
        $.ajax({
            url: '/Publisher/DeletePublisher',
            type: 'POST',
            data: {
                publisherID: id,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function () {
                App.alert("Publisher deleted successfully");
                reloadPublisherGrid();
            }
        });
    });
}

/* ---------------- EVENTS ---------------- */
$(document).on('click', '.btn-edit', function () {
    openEditPublisherModal($(this).data('id'));
});

$(document).on('click', '.btn-delete', function () {
    deletePublisher($(this).data('id'), $(this).data('name'));
});

/* ---------------- GRID ---------------- */
$(function () {

    const colModels = [
        {
            label: "Action",
            name: "action",
            width: 90,
            align: "center",
            sortable: false,
            search: false,
            formatter: actionFormatter
        },
        { name: "publisherID", key: true, hidden: true },
        { label: "Publisher Name", name: "publisherName", width: 200 },
        {
            label: "Status",
            name: "isActive",
            width: 100,
            align: "center",
            formatter: statusFormatter,
            stype: "select",
            searchoptions: {
                value: ":All;true:Active;false:Inactive",
                sopt: ["eq"]
            }
        }
    ];

    App.CreateJQGrid(
        '#publisherGrid',
        apiURL + 'Publisher/PublisherList',
        'json',
        [],
        colModels,
        TOKEN,
        true,
        false,
        "55vh"
    );
});
