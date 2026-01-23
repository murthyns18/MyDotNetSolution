// ================= TOKEN =================
$.ajaxSetup({
    beforeSend: function (xhr) {
        if (typeof TOKEN !== "undefined" && TOKEN) {
            xhr.setRequestHeader("Authorization", "Bearer " + TOKEN);
        }
    }
});

let selectedBooks = [];

/* ================= USERS ================= */
function loadUsers() {
    $.ajax({
        url: apiURL + "User/UserList",
        type: "GET",
        data: { userId: -1 },
        success: function (data) {
            $("#ddlUser").empty()
                .append(`<option value="">-- Select User --</option>`);

            data.forEach(u => {
                $("#ddlUser").append(
                    `<option value="${u.userId}">${u.name}</option>`
                );
            });
        }
    });
}

$("#ddlUser").on("change", function () {
    const userId = $(this).val();
    if (!userId) return;

    $.ajax({
        url: apiURL + "User/UserList",
        type: "GET",
        data: { userId: userId },
        success: function (data) {
            const u = data[0];
            $("#txtName").val(u.name);
            $("#txtEmail").val(u.email);
            $("#txtAddress").val(u.address);
            $("#txtMobile").val(u.mobileNumber);
        }
    });
});

/* ================= PUBLISHERS ================= */
function loadPublishers() {
    $.ajax({
        url: apiURL + "Publisher/PublisherList",
        type: "GET",
        data: { publisherID: -1 },
        success: function (data) {
            $("#ddlPublisher").empty()
                .append(`<option value="">-- Select Publisher --</option>`);

            data.forEach(p => {
                $("#ddlPublisher").append(
                    `<option value="${p.publisherId}">${p.publisherName}</option>`
                );
            });
        }
    });
}

$("#ddlPublisher").on("change", function () {
    const publisherId = $(this).val();

    $("#ddlBook").empty()
        .append(`<option value="">-- Select Book --</option>`);

    if (!publisherId) return;

    $.ajax({
        url: apiURL + "Book/BookList",
        type: "GET",
        data: { bookID: -1 },
        success: function (data) {
            data
                .filter(b => b.PublisherId == publisherId) // ✅ FIX
                .forEach(b => {
                    $("#ddlBook").append(
                        `<option value="${b.bookID}">${b.title}</option>`
                    );
                });
        }
    });
});

/* ================= ADD BOOK ================= */
function addBook() {

    const bookId = $("#ddlBook").val();
    const bookName = $("#ddlBook option:selected").text();

    if (!bookId) {
        App.alert("Please select a book");
        return;
    }

    if (selectedBooks.length >= 4) {
        App.alert("Maximum 4 books allowed");
        return;
    }

    if (selectedBooks.some(b => b.bookId == bookId)) {
        App.alert("Book already added");
        return;
    }

    selectedBooks.push({ bookId: parseInt(bookId) });
    renderSelectedBooks();
}

/* ================= REMOVE BOOK ================= */
function removeBook(bookId) {
    selectedBooks = selectedBooks.filter(b => b.bookId !== bookId);
    renderSelectedBooks();
}

/* ================= RENDER TABLE ================= */
function renderSelectedBooks() {

    const tbody = $("#tblSelectedBooks tbody");
    tbody.empty();

    if (selectedBooks.length === 0) {
        tbody.append(`
            <tr>
                <td colspan="3" class="text-center text-muted">
                    No books added
                </td>
            </tr>
        `);
        return;
    }

    selectedBooks.forEach(b => {
        const name = $(`#ddlBook option[value='${b.bookId}']`).text();

        tbody.append(`
            <tr>
                <td>${name}</td>
                <td class="text-center">1</td>
                <td class="text-center">
                    <button class="btn btn-sm btn-danger"
                            onclick="removeBook(${b.bookId})">
                        <i class="bi bi-trash"></i>
                    </button>
                </td>
            </tr>
        `);
    });
}

/* ================= SUBMIT ================= */
function submitLoan() {

    const userId = $("#ddlUser").val();

    if (!userId) {
        App.alert("Please select user");
        return;
    }

    if (selectedBooks.length === 0) {
        App.alert("Add at least one book");
        return;
    }

    const payload = {
        userId: parseInt(userId),
        loanDetails: selectedBooks
    };

    $.ajax({
        url: "/Loan/CreateLoan",
        type: "POST",
        data: {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
            loan: payload
        },
        success: function (res) {
            App.alert(res.message);
            window.location.href = "/Loan/LoanList";
        },
        error: function () {
            App.alert("Loan creation failed");
        }
    });
}

/* ================= INIT ================= */
$(function () {
    loadUsers();
    loadPublishers();
    renderSelectedBooks();
});
