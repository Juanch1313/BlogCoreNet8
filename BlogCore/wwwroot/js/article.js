var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable()
{
    dataTable = $("#tblArticles").DataTable({
        "ajax": {
            "url": "/admin/articles/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "20%" },
            { "data": "category.name", "width": "15%" },
            { "data": "createdAt", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                      <a href="/Admin/Articles/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                        <i class="far fa-edit"></i> Edit
                      </a>
                    &nbsp;
                      <a onclick=Delete("/Admin/Articles/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                        <i class="far fa-trash-alt"></i> Delete
                      </a>
                    </div>
                    `;
                }, "width": "30%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "This content will disapear forever!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete",
        closeOnconfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                }
            }
        });
    });
}