
$(document).ready(function () {
    $('#tableEmployee').DataTable({
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'copy',
                title: 'Copy of Employee List',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'csv',
                title: 'Employee List as CSV',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'excel',
                title: 'Employee List in Excel',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'pdf',
                title: 'Employee List in PDF',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'print',
                title: 'Print Employee List',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            }
        ],
        pageLength: 5,
        lengthMenu: [[5, 10, 20, -1], [5, 10, 20, 'All']],
        responsive: true,
        "ajax": {
            "url": "https://localhost:44341/API/Employees",
            "dataSrc": ""
        },
        "columns": [
            {
                "data": "nik"
            },
            {
                "data": "firstName"
            },
            {
                "data": "lastName"
            },
            {
                "data": "email"
            },
            {
                "data": "phone"
            },
            {
                "data": null,
                "render": function (data, type, row) {
                    return `<button type="button" class="btn btn-primary" data-toggle="modal"
                                onclick="getData('${row["nik"]}')" data-target="#modalInfo">
                                Info</button>`;
                }
            }
        ]
    });
});

function dateConversion(dates) {
    var date = new Date(dates)
    var newDate = ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '/' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate())) + '/' + date.getFullYear()
    return newDate
}
function getData(nik) {
    $.ajax({
        url: "https://localhost:44341/API/Employees/" + nik
    }).done((result) => {
        console.log(result)
        var text = ''
        text = `<div class = "text-center">
                    <table class= "table bg-light table-hover text-info text-center">
                        <tr>
                            <td>Name</td>
                            <td>:</td>
                            <td>${result.firstName} ${result.lastName}</td>
                        </tr>
                        <tr>
                            <td>BirthDate</td>
                            <td>:</td>
                            <td>${dateConversion(result.birthDate)}</td>
                        </tr>
                        <tr>
                            <td>Salary</td>
                            <td>:</td>
                            <td>Rp. ${result.salary}</td>
                        </tr>
                    </table>
                    </div>`
        $('.modal-body').html(text);
    }).fail((error) => {
        console.log(error);
    });
}
function Insert() {
    var obj = new Object(); //sesuaikan sendiri nama objectnya dan beserta isinya
    //ini ngambil value dari tiap inputan di form nya
    obj.NIK = $("#nik").val();
    obj.FirstName = $("#firstName").val();
    obj.LastName = $("#lastName").val();
    obj.Email = $("#email").val();
    obj.Phone = $("#phone").val();
    console.log(obj);
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        url: "https://localhost:44341/API/Employees/",
        type: "POST",
        dataType: 'json',
        data: JSON.stringify(obj)
    }).done((result) => {
        alert("Berhasil");
    }).fail((error) => {
        alert("Gagal");
    });
}
