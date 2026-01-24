function actionFormatter(cellValue, options, row) {
    return `
        <div class="text-nowrap">
            <button class="btn btn-sm btn-warning me-1 btn-edit" data-id="${row.categoryID}">
                <i class="bi bi-pencil-square"></i>
            </button>
            <button class="btn btn-sm btn-danger btn-delete"
                    data-id="${row.categoryID}"
                    data-name="${row.categoryName}">
                <i class="bi bi-trash"></i>     
            </button>
        </div>`;
}

function statusFormatter(value) {
    return value
        ? "<span class='badge bg-success'>Active</span>"
        : "<span class='badge bg-danger'>Inactive</span>";
}

function reloadCategoryGrid() {
    $("#categoryGrid")
        .jqGrid('setGridParam', { page: 1 })
        .trigger('reloadGrid');
}

/* ---------- ADD ---------- */
function openAddCategoryModal() {
    $('#categoryForm')[0].reset();
    $('#CategoryID').val(0);

    $('#statusContainer').addClass('d-none');

    $('#categoryModalTitle').text('Add Category');
    $('#categoryModal').modal('show');
}

/* ---------- EDIT ---------- */
function openEditCategoryModal(categoryId) {
    $.get('/Category/EditCategory', { categoryID: categoryId })
        .done(function (data) {

            $('#CategoryID').val(data.categoryID);
            $('#CategoryName').val(data.categoryName);
            $('input[name="IsActive"][value="' + data.isActive + '"]').prop('checked', true);

            $('#statusContainer').removeClass('d-none');

            $('#categoryModalTitle').text('Edit Category');
            $('#categoryModal').modal('show');
        })
        .fail(function () {
            App.alert('Failed to load category details.');
        });
}

/* ---------- DELETE ---------- */
function deleteCategory(id, name) {
    confirm(`Are you sure you want to delete "${name}"?`, function () {
        $.ajax({
            url: '/Category/DeleteCategory',
            type: 'POST',
            data: {
                categoryID: id,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function () {
                App.alert("Category deleted successfully");
                reloadCategoryGrid();
            }
        });
    });
}

/* ---------- EVENTS ---------- */
$(document).on('click', '.btn-edit', function () {
    openEditCategoryModal($(this).data('id'));
});

$(document).on('click', '.btn-delete', function () {
    deleteCategory($(this).data('id'), $(this).data('name'));
});

/* ---------- GRID ---------- */
$(function () {

    const colModels = [
        { label: "Action", name: "action", width: 90, align: "center", sortable: false, search: false, formatter: actionFormatter },
        { name: "categoryID", key: true, hidden: true },
        { label: "Category Name", name: "categoryName", width: 220 },
        {
            label: "Status",
            name: "isActive",
            width: 90,
            align: "center",
            formatter: statusFormatter,
            stype: "select",
            searchoptions: { value: ":All;true:Active;false:Inactive", sopt: ["eq"] }
        }
    ];

    App.CreateJQGrid(
        '#categoryGrid',
        apiURL + 'Category/CategoryList',
        'json',
        [],
        colModels,
        TOKEN,
        true,
        false,
        "55vh"
    );
});
