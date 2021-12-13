$(document).ready(function () {
    table = $("#tableEmployee").DataTable({
        pageLength: 5,
        lengthMenu: [[5, 10, 15, 20, -1], [5, 10, 15, 20, 'All']],
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            {
                title: 'List Employees',
                extend: 'copy',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            },
            {
                title: 'List Employees',
                extend: 'csv',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            },
            {
                title: 'List Employees',
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            },
            {
                title: 'List Employees',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                title: 'List Employees',
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            }
        ],

        "ajax": {
            "url": "/employees/getall",
            "dataSrc": "",
            "order": [[1, 'asc']]
        },
        "columns": [
            {
                "data": null,
                "render": function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
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
                "data": null,
                "render": function (data, type, row) {
                    return `<button type="button" class="btn btn-info" data-toggle="modal"
                                onclick="getData('${row["nik"]}')" data-target="#DetailEmployee"
                                data-placement="top" title="Details">
                                    <i class="fas fa-info-circle"></i>
                             </button>
                             <button type="submit" class="btn btn-danger" onclick="Delete('${row["nik"]}')"
                                data-placement="top" title="Delete">
                                    <i class="fas fa-trash"></i>
                                    
                             </button>
                             <button type="button" class="btn btn-success" data-toggle="modal" onclick="getDataUpdate('${row["nik"]}')" data-target="#form-edit"
                                data-placement="top" title="Edit">
                                    <i class="fas fa-edit"></i>
                                    
                             </button>`;

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
        /*url: "https://localhost:44341/API/Employees/" + nik*/
        url: "/employees/get/" + nik
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
                        <tr>
                            <td>Phone Number</td>
                            <td>:</td>
                            <td>${result.phone}</td>
                        </tr>
                    </table>
                    </div>`
        $('.datainfo').html(text);
    }).fail((error) => {
        console.log(error);
    });
}

function Insert() {
    var obj = new Object();
    obj.nik = $("#nik").val();
    obj.firstName = $("#firstName").val();
    obj.lastName = $("#lastName").val();
    obj.email = $("#email").val();
    obj.phone = $("#phone").val();
    obj.salary = $("#salary").val();
    obj.gender = $("#gender").val();
    console.log(obj)
    $.ajax({
        /*headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },*/
        /*url: "https://localhost:44341/api/Employees/",*/
        url: "/employees/post",
        type: "Post",
        data:obj,
        /*'data': JSON.stringify(obj),*/
        'dataType': 'json',
        success: function (result) {
            console.log(result)
            if (result == 200) {
                Swal.fire(
                    'Good job!',
                    'Data berhasil di Submit!',
                    'success'
                ),
                    setTimeout(function () { $('#CreateModal').modal('hide'); }, 2000),
                    table.ajax.reload(),
                    setTimeout(function () { $("#createAccount")[0].reset(); }, 4000)
            }
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Submit Gagal!'
            })
        },
        error: function (error) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Submit Gagal!'
            })
        }
    })
}
function Delete(nik) {
    Swal.fire({
        title: 'Are you sure to Delete this Field?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url:"/employees/Delete/"+nik,
                type: "Delete",
                success: function (result) {
                    console.log(result)
                    Swal.fire({
                        icon: 'success',
                        title: 'Deleted!',
                        text: 'Your file has been deleted.'
                    })
                    table.ajax.reload()
                },
                error: function (error) {
                    alert("Delete Fail");
                }
            });
        }
    })
}

/*function Delete(nik) {
    $.ajax({
        *//*headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },*//*
        url: "https://localhost:44341/API/Employees/" + nik,
        type: "DELETE",
        dataType: 'json'
    }).done((result) => {
        console.log(result);
        Swal.fire({
            icon: 'success',
            title: 'Success!',
            text: 'Data Has Been Deleted',
            showConfirmButton: false,
            timer: 1500
        })
        table.ajax.reload();
    }).fail((error) => {
        console.log(error);
    });
}*/

function getDataUpdate(nik) {
    $.ajax({
        /*url: "https://localhost:44341/API/Employees/" + nik,*/
        url:"/employees/get/"+nik,
        success: function (result) {
            console.log(result)
            var data = result
            $("#updatenik").attr("value", data.nik)
            $("#updatefirstName").attr("value", data.firstName)
            $("#updatelastName").attr("value", data.lastName)
            $("#updateemail").attr("value", data.email)
            $("#updatephone").attr("value", data.phone)
            $("#updatesalary").attr("value", data.salary)
        },
        error: function (error) {
            console.log(error)
        }
    })
}
function Update() {
    var obj = new Object();
    obj.nik = $("#updatenik").val();
    obj.firstName = $("#updatefirstName").val();
    obj.lastName = $("#updatelastName").val();
    obj.email = $("#updateemail").val();
    obj.phone = $("#updatephone").val();
    obj.salary = $("#updatesalary").val();
    console.log(obj)
    $.ajax({
        /*headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },*/
        /*url: "https://localhost:44341/api/Employees/",*/
        url:"/employees/put/",
        type: "Put",
        /*'data': JSON.stringify(obj),*/
        data: obj,
        'dataType': 'json',
        success: function (result) {
            Swal.fire({
                icon: 'success',
                title: 'Good job!',
                text: 'Your data has been saved!'
            }),
            table.ajax.reload()
        },
        error: function (error) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Submit Fail!'
            })
        }
    })
}

/*$("#form-register").validate({

    rules: {
        "nik": {
            required: true
        },
        "firstName": {
            required: true
        },
        "lastName": {
            required: true
        },
        "email": {
            required: true,
            email: true
        },
        "phone": {
            required: true
        }
    },
    errorPlacement: function (error, element) { },
    highlight: function (element) {
        $(element).closest('.form-control').addClass('is-invalid');
    },
    unhighlight: function (element) {
        $(element).closest('.form-control').removeClass('is-invalid');
    }

});*/

/*chart-1*/
/*var options = {
    chart: {
        type: 'bar'
    },
    series: [{
        name: 'sales',
        data: [30, 40, 45, 50, 49, 60, 70, 91, 125]
    }],
    xaxis: {
        categories: [1991, 1992, 1993, 1994, 1995, 1996, 1997, 1998, 1999]
    }
}
var chart = new ApexCharts(document.querySelector("#chart-1"), options);
chart.render();*/

/*$.ajax({
    url: "https://localhost:44341/API/Employees",
    success: function (result) {
        console.log(result)
        var data = result
        var male = 0;
        var female = 0;

        $.each(data, function (key, val) {
            if (val.gender == 0) {
                male++
            }
            else {
                female++
            }
        })
        var options = {
            chart: {
                type: 'bar'
            },
            series: [{
                *//*name: 'gender',*//*
                data: [male, female]
            }],
            xaxis: {
                categories: ["Male", "Female"]
            }
        }
        var chart = new ApexCharts(document.querySelector("#chart-1"), options);
        chart.render();
    },
    error: function (error) {
        console.log(error)
    }
})*/
/*chart-1*/
$.ajax({
    url: "https://localhost:44341/API/Employees",
    success: function (result) {
        console.log(result)
        var data = result
        var male = 0;
        var female = 0;

        $.each(data, function (key, val) {
            if (val.gender == 0) {
                male++
            }
            else {
                female++
            }
        });

        var options = {
            series: [male, female],
            labels: ["Male", "Female"],
            chart: {
                type: 'donut',
                toolbar: {
                    show: true,
                    offsetX: 0,
                    offsetY: 0,
                    tools: {
                        download: true,
                        selection: true,
                        zoom: true,
                        zoomin: true,
                        zoomout: true,
                        pan: true,
                        reset: true,
                        customIcons:[]
                    },
                    export: {
                        csv: {
                            filename: undefined,
                            columnDelimiter: ',',
                            headerCategory: 'category',
                            headerValue: 'value',
                            dateFormatter(timestamp) {
                                return new Date(timestamp).toDateString()
                            }
                        },
                        svg: {
                            filename:undefined,
                        },
                        png: {
                            filename:undefined,
                        }
                    },
                    autoSelected:'zoom'
                },

            },
            responsive: [{
                breakpoint: 480,
                options: {
                    chart: {
                        width: "40%"
                    },
                    legend: {
                        position: 'bottom'
                    }
                }
            }]
        };
        var chart1 = new ApexCharts(document.querySelector("#chart-1"), options);
        chart1.render();
    },
    error: function (error) {
        console.log(error)
    }
})

/*chart-2*/
/*var options2 = {
    chart: {
        type: 'donut'
    },
    series: [44, 55, 13, 33],
    labels: ['Apple', 'Mango', 'Orange', 'Watermelon']
}
var chart2 = new ApexCharts(document.querySelector("#chart-2"), options2);
chart2.render();*/
/*chart-2*/
$.ajax({
    url: "https://localhost:44341/API/Universities/CountUniv",
    success: function (result) {
        console.log(result)
        /*var data = result
        var undip = 0;
        var ui = 0;
        var udinus = 0;
        var unbra = 0;

        $.each(data, function (key, val) {
            if (val.universityId == 1) {
                undip++
            }
            else if (val.universityId == 2) {
                ui++
            }
            else if (val.universityId == 3) {
                udinus++
            }
            else {
                unbra++
            }
        });*/
        const univName = [];
        const countEmp = [];
        $.each(result.result, function (key, val) {
            univName.push(val.universityName);
            countEmp.push(val.countStudent);
            console.log(val.universityName);
            console.log(val.countStudent);
    });
        var options2 = {
            chart: {
                type: 'bar'
            },
            series: [{
                    name:"Employees",
                    data: countEmp
            }],
            xaxis: {
                categories: univName
            }
        }
        var chart2 = new ApexCharts(document.querySelector("#chart-2"), options2);
        chart2.render();
    },
    error: function (error) {
        console.log(error)
    }
})

/*chart-3*/
var options3 = {
    chart: {
        height: 350,
        type: "treemap"
    },
    series: [
        {
            data: [
                {
                    x: "New Delhi",
                    y: 218,
                },
                {
                    x: "Kolkata",
                    y: 149,
                },
                {
                    x: "Mumbai",
                    y: 184,
                },
                {
                    x: "Ahmedabad",
                    y: 55,
                },
                {
                    x: "Bangaluru",
                    y: 84,
                },
                {
                    x: "Pune",
                    y: 31,
                },
                {
                    x: "Chennai",
                    y: 70,
                }
            ],
        },
    ]
}
var chart3 = new ApexCharts(document.querySelector("#chart-3"), options3);
chart3.render();

function InsertRegister() {
    var obj = new Object(); //sesuaikan sendiri nama objectnya dan beserta isinya
    //ini ngambil value dari tiap inputan di form nya
    obj.nik = $("#inputNIK").val();
    obj.firstName = $("#inputFirstName").val();
    obj.lastName = $("#inputLastName").val();
    obj.phone = $("#inputPhone").val();
    obj.email = $("#inputEmail").val();
    obj.password = $("#inputPassword").val();
    obj.salary = $("#inputSalary").val();
    //obj.birthdate = $("#inputdateBirth").val();
    obj.gpa = $("#inputGPA").val();
    /*var gender = $('#inputGender input:radio:checked').val()
    obj.gender = gender;*/
    obj.degree = $("#inputDegree").val();
    obj.universityId = $("#inputUniversityId").val();

    console.log(obj);
    $.ajax({
        type: "POST",
        url: "/Employees/PostRegister",
        dataType: 'json',
        data: obj
    }).done((result) => {
        console.log(result);
        if (result == 200) {
            Swal.fire({
                icon: 'success',
                title: 'Berhasil Disimpan',
                showConfirmButton: false,
                timer: 1500
            })
        } else if (result == 400) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Gagal Disimpan'
            })
        }
    }).fail((error) => {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Gagal Disimpan'
        })
    })
}