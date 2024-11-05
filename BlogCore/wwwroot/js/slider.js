var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable()
{
    dataTable = $("#tblSliders").DataTable({
        "ajax": {
            "url": "/admin/sliders/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "20%" },
            {
                "data": "imageUrl",
                "render": function (image) {
                    return `<img src="../${image}" width="120px"></img>`
                }
            },
            {
                "data": "status",
                "render": function (status) {
                    return status ? "Active" : "Disabled"
                }
            },
            { "data": "createdAt", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                      <a href="/Admin/Sliders/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                        <i class="far fa-edit"></i> Edit
                      </a>
                    &nbsp;
                      <a onclick=Delete("/Admin/Sliders/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
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